using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using ExamPaperParser.Number.Differentiators;
using ExamPaperParser.Number.Manager.Exceptions;
using ExamPaperParser.Number.Models.DecoratedNumbers;
using ExamPaperParser.Number.Models.Numbers;
using ExamPaperParser.Number.Models.NumberTree;

namespace ExamPaperParser.Number.Manager
{
    public class NumberManager : INumberManager
    {
        private readonly static Regex ReadingQuestionKeywords = new Regex(@"阅读", RegexOptions.Compiled);
        private readonly HashSet<string> _readingParagraphNumberDifferentiator = new HashSet<string>();

        private INumberDifferentiator _numberDifferentiator;

        /// <summary>
        /// Props of level for each differentiator
        /// </summary>
        private Dictionary<string, LevelProps> _differentiatorLevelPropsMapping = new Dictionary<string, LevelProps>();

        /// <summary>
        /// All differentiators on the chain from root node to current node
        /// Differentiators on other chains won't be in this set
        /// </summary>
        private HashSet<string> _differentiatorSetInCurrentChain = new HashSet<string>();

        private IDictionary<string, int> _allowFirstNumberForDifferentiators = new Dictionary<string, int>();

        public NumberRoot Root { get; private set; } = new NumberRoot();

        public NumberNode? Current { get; private set; } = null;

        public NumberManager(INumberDifferentiator numberDifferentiator)
        {
            _numberDifferentiator = numberDifferentiator;

            _readingParagraphNumberDifferentiator.Add(
                _numberDifferentiator.GetDecoratedNumberDifferentiator(
                    new BracketDecoratedNumber(new ArabicNumber("", 1, true), "", "(", ")")));

            _readingParagraphNumberDifferentiator.Add(
                _numberDifferentiator.GetDecoratedNumberDifferentiator(
                    new UndecoratedNumber(new CircledNumber("", 1))));
        }

        public Backup Save()
        {
            return new Backup
            {
                DifferentiatorLevelPropsMapping = new Dictionary<string, LevelProps>(_differentiatorLevelPropsMapping),
                DifferentiatorSet = new HashSet<string>(_differentiatorSetInCurrentChain),
                Current = Current,
            };
        }

        public void Load(Backup backup)
        {
            _differentiatorLevelPropsMapping = backup.DifferentiatorLevelPropsMapping;
            _differentiatorSetInCurrentChain = backup.DifferentiatorSet;
            Current = backup.Current;
        }

        public void Reset()
        {
            _differentiatorLevelPropsMapping.Clear();
            _differentiatorSetInCurrentChain.Clear();
            _allowFirstNumberForDifferentiators.Clear();

            Root = new NumberRoot();
            Current = null;
            // GC.Collect();
        }

        private NumberNode? GoUpLevel(NumberNode current, string targetDifferentiator, bool doChanges = true)
        {
            while (current.Parent.ChildDifferentiator != targetDifferentiator)
            {
                if (current.Parent.ChildDifferentiator != null && doChanges)
                {
                    _differentiatorSetInCurrentChain.Remove(current.Parent.ChildDifferentiator);
                }

                var parent = current.Parent;
                if (parent is NumberNode nodeParent)
                {
                    current = nodeParent;
                }
                else
                {
                    return null;
                }
            }

            return current;
        }

        private void ContinueLevel(BaseDecoratedNumber currentNumber, string differentiator, int paragraphOrder)
        {
            if (Current == null)
            {
                throw new ArgumentNullException("Current node cannot be null");
            }

            if (Current.DecoratedNumber.Number.IntNumber + 1 != currentNumber.Number.IntNumber)
            {
                throw new DiscontinuousNumberException(currentNumber, Current.DecoratedNumber.Number.IntNumber + 1);
            }

            var node = new NumberNode(Current.Parent, currentNumber, paragraphOrder);
            Current.Parent.Children.Add(node);
            Current = node;

            _differentiatorLevelPropsMapping[differentiator] = new LevelProps
            {
                Level = Current.Level,
                MaxNumber = currentNumber.Number.IntNumber,
            };
        }

        private void AddNew(BaseNumberNode parent, BaseDecoratedNumber child, string differentiator, int paragraphOrder)
        {
            var node = new NumberNode(parent, child, paragraphOrder);
            parent.ChildDifferentiator = differentiator;
            parent.Children.Add(node);

            Current = node;
            _differentiatorSetInCurrentChain.Add(differentiator);

            _differentiatorLevelPropsMapping[differentiator] = new LevelProps
            {
                Level = Current.Level,
                MaxNumber = child.Number.IntNumber,
            };
        }

        public void SetAllowFirstNumberForDifferentatiators(IDictionary<string, int> allowFirstNumbers)
        {
            _allowFirstNumberForDifferentiators = new Dictionary<string, int>(allowFirstNumbers);
        }

        public Dictionary<string, int> GetAllowFirstNumberForDifferentiators()
        {
            return _differentiatorLevelPropsMapping.ToDictionary(o => o.Key, o => o.Value.MaxNumber + 1);
        }

        private bool IsCurrentReadingQuestion()
        {
            if (Current == null)
            {
                return false;
            }

            var bodyLines = Current.Body.Split('\n');
            var bodyFirstLine = "";
            if (bodyLines.Length > 0)
            {
                bodyFirstLine = bodyLines[0];
            }

            if (ReadingQuestionKeywords.IsMatch(Current.Header) 
                || ReadingQuestionKeywords.IsMatch(bodyFirstLine))
            {
                return true;
            }

            return false;
        }

        public NumberNode AddNumber(BaseDecoratedNumber decoratedNumber, int paragraphOrder)
        {
            var differentiator = _numberDifferentiator.GetDecoratedNumberDifferentiator(decoratedNumber);
            var number = decoratedNumber.Number.IntNumber;
                if (_readingParagraphNumberDifferentiator.Contains(differentiator))
                {
                    throw new ReadingParagraphNumberException(decoratedNumber);
                }

            if (Current == null)
            {
                // Init should start from 1
                if (number == 1
                    || (_allowFirstNumberForDifferentiators.TryGetValue(differentiator, out var maxNumber)
                    && number == maxNumber))
                {
                    AddNew(Root, decoratedNumber, differentiator, paragraphOrder);
                }
                else
                {
                    throw new StartFromNonFirstNumberException(decoratedNumber);
                }
            }
            else if (Current.Parent.ChildDifferentiator == differentiator)
            {
                // Continue current level
                ContinueLevel(decoratedNumber, differentiator, paragraphOrder);
            }
            else if (_differentiatorSetInCurrentChain.Contains(differentiator))
            {
                // Back to up level of current chain
                Current = GoUpLevel(Current, differentiator) ?? throw new InvalidOperationException(
                    $"Differentiator \"{differentiator}\" should exist in up level");

                // Continue this level
                ContinueLevel(decoratedNumber, differentiator, paragraphOrder);
            }
            else if (_differentiatorLevelPropsMapping.TryGetValue(differentiator, out var levelProps))
            // && number == levelProps.MaxNumber + 1)
            {
                if (_numberDifferentiator.AllowedDifferentiatorToSpanParents.Contains(differentiator))
                {
                    // Continue previous level
                    AddNew(Current, decoratedNumber, differentiator, paragraphOrder);
                }
                else if (number == 1
                    || (_allowFirstNumberForDifferentiators.TryGetValue(differentiator, out var maxNumber)
                    && number == maxNumber))
                {
                    // Don't allow to span parents, then check if it's a new level.
                    // If so, add new level
                    AddNew(Current, decoratedNumber, differentiator, paragraphOrder);
                }
                else
                {
                    throw new StartFromNonFirstNumberException(decoratedNumber);
                }
            }
            else
            {
                // New Level, should start from 1
                if (number == 1
                    || (_allowFirstNumberForDifferentiators.TryGetValue(differentiator, out var maxNumber)
                    && number == maxNumber))
                {
                    AddNew(Current, decoratedNumber, differentiator, paragraphOrder);
                }
                else
                {
                    throw new StartFromNonFirstNumberException(decoratedNumber);
                }
            }

            if (Current == null)
            {
                throw new InvalidOperationException("Current node shouldn't be null now");
            }

            return Current;
        }
    }
}
