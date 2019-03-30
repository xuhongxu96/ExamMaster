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

        protected override bool NumberNodeVisitor_OnVisited(NumberNode node, int level)
        {
            if (IsChoiceQuestion(node))
            {
                node.Body = $"{node.Body}\n{NodeHelper.ConcatNodeContent(node)}".Trim();

                node.ChildDifferentiator = null;
                node.Children.Clear();

                return false;
            }

            return true;
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
