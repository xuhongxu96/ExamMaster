using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using ExamPaperParser.DataView;
using ExamPaperParser.Helpers;
using ExamPaperParser.Number.Extractors.Exceptions;
using ExamPaperParser.Number.Manager;
using ExamPaperParser.Number.Models.DecoratedNumbers;
using ExamPaperParser.Number.Models.Numbers;
using ExamPaperParser.Number.Models.NumberTree;
using ExamPaperParser.Number.Parsers.NumberParsers;
using ExamPaperParser.Number.Visitors;
using FormattedFileParser.Exceptions;

namespace ExamPaperParser.Number.Postprocessors
{
    public class ChoiceQuestionPostprocessor : BaseVisitNodePostprocessor
    {
        private readonly LowerChineseNumberParser _lowerChineseNumberParser = new LowerChineseNumberParser();
        private readonly Regex _suffix = new Regex(@"^[项组个][是为]", RegexOptions.Compiled);

        protected override bool NumberNodeVisitor_OnVisited(NumberNode node, int level, out List<ParagraphFormatException> exceptions)
        {
            exceptions = new List<ParagraphFormatException>();
            if (IsChoiceQuestion(node))
            {
                var exception = CheckLastChoiceContent(node);
                if (exception != null)
                {
                    exceptions.Add(exception);
                }

                node.IsChoiceQuestion = true;
                node.Body = $"{node.Body}\n{NodeHelper.ConcatNodeContent(node)}".Trim();
                node.ChildDifferentiator = null;
                node.Children.Clear();

                return false;
            }

            return true;
        }

        private ParagraphFormatException? CheckLastChoiceContent(NumberNode node)
        {
            if (node.Children.Any())
            {
                var lastChoiceContent = NodeHelper.ConcatNodeContent(node.Children.Last(), false);
                if (lastChoiceContent.Length > 200)
                {
                    return new NumberException(
                        "选项内容过长，可能是中间缺少了题号，以致未能正确分割",
                        string.Join(" ", NumberNodeHelper.GetNumberChain(node)),
                        lastChoiceContent);
                }
            }

            return null;
        }

        private bool IsChoiceQuestion(NumberNode node)
        {
            if (node.Children.Count > 1)
            {
                var firstChildNumber = node.Children.First().DecoratedNumber;
                if (firstChildNumber is UndecoratedNumber || firstChildNumber is DelimiterDecoratedNumber)
                {
                    if (firstChildNumber.Number is AlphabeticalNumber alphabeticalNumber
                        && alphabeticalNumber.IsHalfWidth
                        && !alphabeticalNumber.IsLower)
                    {
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
                    return false;
                }

                data = data.CloneByDelta(index + 1);
                var choiceCount = _lowerChineseNumberParser.Consume(data).SingleOrDefault();
                if (choiceCount != null)
                {
                    data = choiceCount.DataView;
                    if (_suffix.IsMatch(data.CurrentView.ToString()))
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
