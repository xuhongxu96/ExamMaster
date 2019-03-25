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

namespace ExamPaperParser.Number.Postprocessors
{
    public class ChoiceQuestionPostprocessor : IPostprocessor
    {
        private LowerChineseNumberParser _lowerChineseNumberParser = new LowerChineseNumberParser();
        private Regex _suffix = new Regex(@"^[项组个][是为]", RegexOptions.Compiled);

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

            IDataView data = new StringDataView(node.Content);

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

        private string ConcatNodeContent(NumberNode node, bool root = true)
        {
            var sb = new StringBuilder();
            if (!root)
            {
                sb.AppendLine($"{node.DecoratedNumber.RawRepresentation}{node.Header}\n{node.Content}".Trim());
            }

            foreach (var child in node.Children)
            {
                sb.AppendLine(ConcatNodeContent(child, false));
            }

            return sb.ToString();
        }

        private void VisitNode(NumberNode node, int level)
        {
            if (IsChoiceQuestion(node))
            {
                node.Content = $"{node.Content}\n{ConcatNodeContent(node)}".Trim();

                node.ChildDifferentiator = null;
                node.Children.Clear();
            }

            foreach (var child in node.Children)
            {
                VisitNode(child, level + 1);
            }
        }

        private void VisitRoot(NumberRoot root)
        {
            foreach (var child in root.Children)
            {
                VisitNode(child, 0);
            }
        }

        public void Process(NumberRoot root)
        {
            VisitRoot(root);
        }
    }
}
