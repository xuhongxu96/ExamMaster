using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using ExamPaperParser.DataView;
using ExamPaperParser.Number.Manager;
using ExamPaperParser.Number.Models.DecoratedNumbers;
using ExamPaperParser.Number.Models.Numbers;
using ExamPaperParser.Number.Models.NumberTree;
using ExamPaperParser.Number.Parsers.NumberParsers;
using ExamPaperParser.Number.Visitors;

namespace ExamPaperParser.Number.Postprocessors
{
    public class ChoiceQuestionPostprocessor : BaseVisitNodePostprocessor
    {
        private readonly LowerChineseNumberParser _lowerChineseNumberParser = new LowerChineseNumberParser();
        private readonly Regex _suffix = new Regex(@"^[项组个][是为]", RegexOptions.Compiled);

        protected override bool NumberNodeVisitor_OnVisited(NumberNode node, int level, out List<Exception> exceptions)
        {
            exceptions = new List<Exception>();
            Exception? exception;
            if (IsChoiceQuestion(node, out exception))
            {
                if (exception != null)
                {
                    exceptions.Add(exception);
                }

                node.Body = $"{node.Body}\n{NodeHelper.ConcatNodeContent(node)}".Trim();

                node.ChildDifferentiator = null;
                node.Children.Clear();

                return false;
            }

            return true;
        }

        private Exception? CheckLastChoiceContent(NumberNode node)
        {
            var lastChoiceContent = NodeHelper.ConcatNodeContent(node.Children.Last(), false);
            if (lastChoiceContent.Length > 200)
            {
                return new FormatException($"Too long choice content. Maybe there lacks a question number\n{lastChoiceContent}");
            }

            return null;
        }

        private bool IsChoiceQuestion(NumberNode node, out Exception? e)
        {
            e = null;
            if (node.Children.Count > 1)
            {
                var firstChildNumber = node.Children.First().DecoratedNumber;
                if (firstChildNumber is UndecoratedNumber || firstChildNumber is DelimiterDecoratedNumber)
                {
                    if (firstChildNumber.Number is AlphabeticalNumber alphabeticalNumber
                        && alphabeticalNumber.IsHalfWidth
                        && !alphabeticalNumber.IsLower)
                    {
                        e = CheckLastChoiceContent(node);
                        return true;
                    }
                }
            }

            IDataView data = new StringDataView(node.Header);

            while (!data.EndOfStream)
            {
                var index = data.CurrentView.IndexOf("的");
                if (index == -1)
                {
                    e = null;
                    return false;
                }

                data = data.CloneByDelta(index + 1);
                var choiceCount = _lowerChineseNumberParser.Consume(data).SingleOrDefault();
                if (choiceCount != null)
                {
                    data = choiceCount.DataView;
                    if (_suffix.IsMatch(data.CurrentView.ToString()))
                    {
                        e = CheckLastChoiceContent(node);

                        return true;
                    }
                }
            }

            return false;
        }
    }
}
