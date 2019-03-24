using System;
using System.Collections.Generic;
using System.Text;
using ExamPaperParser.Number.Differentiators;
using ExamPaperParser.Number.Manager.Exceptions;
using ExamPaperParser.Number.Models.DecoratedNumbers;

namespace ExamPaperParser.Number.Manager
{
    public class NumberManager
    {
        private INumberDifferentiator _numberDifferentiator;

        private Dictionary<string, int> _differentiatorMaxNumberMapping = new Dictionary<string, int>();
        private HashSet<string> _differentiatorSet = new HashSet<string>();
        private NumberNode? _current = null;

        public NumberRoot Root { get; private set; } = new NumberRoot();

        struct Backup
        {
            public Dictionary<string, int> DifferentiatorLevelMapping { get; set; }

            public HashSet<string> DifferentiatorSet { get; set; }

            public NumberNode? Current { get; set; }
        }

        private Stack<Backup> _backups = new Stack<Backup>();

        public NumberManager(INumberDifferentiator numberDifferentiator)
        {
            _numberDifferentiator = numberDifferentiator;
        }

        public void PushCurrent()
        {
            _backups.Push(new Backup
            {
                DifferentiatorLevelMapping = new Dictionary<string, int>(_differentiatorMaxNumberMapping),
                DifferentiatorSet = new HashSet<string>(_differentiatorSet),
                Current = _current,
            });
        }

        public void PopCurrent()
        {
            var backup = _backups.Pop();

            _differentiatorMaxNumberMapping = backup.DifferentiatorLevelMapping;
            _differentiatorSet = backup.DifferentiatorSet;
            _current = backup.Current;

            GC.Collect();
        }

        public void Reset()
        {
            _backups.Clear();
            _differentiatorMaxNumberMapping.Clear();
            _differentiatorSet.Clear();

            Root = new NumberRoot();
            _current = null;

            GC.Collect();
        }

        private NumberNode? GoUpLevel(string targetDifferentiator)
        {
            if (_current == null)
            {
                throw new ArgumentNullException("Current node cannot be null");
            }

            while (_current.Parent.ChildDifferentiator != targetDifferentiator)
            {
                if (_current.Parent.ChildDifferentiator != null)
                {
                    _differentiatorSet.Remove(_current.Parent.ChildDifferentiator);
                }

                var parent = _current.Parent;
                if (parent is NumberNode nodeParent)
                {
                    _current = nodeParent;
                }
                else
                {
                    return null;
                }
            }

            return _current;
        }

        private void ContinueLevel(BaseDecoratedNumber currentNumber, string differentiator, int paragraphOrder)
        {
            if (_current == null)
            {
                throw new ArgumentNullException("Current node cannot be null");
            }

            if (_current.DecoratedNumber.Number.IntNumber + 1 != currentNumber.Number.IntNumber)
            {
                throw new DiscontinuousNumberException(currentNumber, _current.DecoratedNumber.Number.IntNumber + 1);
            }

            var node = new NumberNode(_current.Parent, currentNumber, paragraphOrder);
            _current.Parent.Children.Add(node);
            _current = node;

            _differentiatorMaxNumberMapping[differentiator] = currentNumber.Number.IntNumber;
        }

        private void AddNew(BaseNumberNode parent, BaseDecoratedNumber child, string differentiator, int paragraphOrder)
        {
            var node = new NumberNode(parent, child, paragraphOrder);
            parent.ChildDifferentiator = differentiator;
            parent.Children.Add(node);

            _differentiatorSet.Add(differentiator);
            _differentiatorMaxNumberMapping[differentiator] = child.Number.IntNumber;

            _current = node;
        }

        public NumberNode AddNumber(BaseDecoratedNumber decoratedNumber, int paragraphOrder)
        {
            var differentiator = _numberDifferentiator.GetDecoratedNumberDifferentiator(decoratedNumber);
            var number = decoratedNumber.Number.IntNumber;

            if (_current == null)
            {
                // Init should start from 1
                if (number != 1)
                {
                    throw new StartFromNonFirstNumberException(decoratedNumber);
                }

                AddNew(Root, decoratedNumber, differentiator, paragraphOrder);
            }
            else if (_current.Parent.ChildDifferentiator == differentiator)
            {
                // Continue current level
                ContinueLevel(decoratedNumber, differentiator, paragraphOrder);
            }
            else if (_differentiatorSet.Contains(differentiator))
            {
                // Back to up level of current chain
                _current = GoUpLevel(differentiator) ?? throw new InvalidOperationException(
                    $"Differentiator \"{differentiator}\" should exist in up level");

                // Continue this level
                ContinueLevel(decoratedNumber, differentiator, paragraphOrder);
            }
            else if (_differentiatorMaxNumberMapping.TryGetValue(differentiator, out var maxNumber)
                && number == maxNumber + 1)
            {
                // Continue previous level
                AddNew(_current, decoratedNumber, differentiator, paragraphOrder);
            }
            else
            {
                // New Level, should start from 1
                if (number != 1)
                {
                    throw new StartFromNonFirstNumberException(decoratedNumber);
                }

                AddNew(_current, decoratedNumber, differentiator, paragraphOrder);
            }

            if (_current == null)
            {
                throw new InvalidOperationException("Current node shouldn't be null now");
            }

            return _current;
        }
    }
}
