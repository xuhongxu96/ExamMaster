using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using ExamPaperParser.DataView;
using ExamPaperParser.Number.Models.NumberTree;
using ExamPaperParser.Number.Parsers.NumberParsers;
using FormattedFileParser.Exceptions;

namespace ExamPaperParser.Number.Postprocessors
{
    public class SelectiveQuestionPostprocessor : BaseVisitNodePostprocessor
    {
        private static Regex leftRegex = new Regex(@"[(\[{<（【请从].*?选", RegexOptions.Compiled);
        private static Regex rightRegex = new Regex(@"[类题空组个道项]+([^分]*?)[，,。)\]}>）】]", RegexOptions.Compiled);

        private UniversalNumberParser _numberParser = new UniversalNumberParser(
            new LowerChineseNumberParser(false),
            new HalfWidthArabicNumberParser(false),
            new FullWidthArabicNumberParser(false));

        protected override bool NumberNodeVisitor_OnVisited(NumberNode node, int level, out List<ParagraphFormatException> exceptions)
        {
            exceptions = new List<ParagraphFormatException>();

            var m = leftRegex.Match(node.Header);
            var data = new StringDataView(node.Header);

            while (m.Success)
            {
                data.Position = m.Index + m.Length;
                var numbers = _numberParser.Consume(data);
                if (numbers.Any())
                {
                    var number = numbers.First();
                    var afterNumberData = number.DataView;
                    var rightMatch = rightRegex.Match(afterNumberData.CurrentView.ToString());
                    if (rightMatch.Success)
                    {
                        node.SelectiveDescription = node.Header.Substring(
                                m.Index,
                                afterNumberData.Position + rightMatch.Index + rightMatch.Length - m.Index);
                        node.SelectCount = number.Result.IntNumber;

                        return false;
                    }
                }

                m = m.NextMatch();
            }

            return true;
        }
    }
}
