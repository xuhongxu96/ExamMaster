using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using ExamPaperParser.Base;
using ExamPaperParser.DataView;
using ExamPaperParser.Number.Models.DecoratedNumbers;
using ExamPaperParser.Number.Models.Numbers;
using ExamPaperParser.Number.Parsers.NumberParsers;

namespace ExamPaperParser.Number.Parsers.DecoratedNumberParsers
{
    public class BracketDecoratedNumberParser : BaseDecoratedNumberParser
    {
        private static Regex leftBracketRegex = new Regex(@"^[(\[{<（【]", RegexOptions.Compiled);
        private static Regex rightBracketRegex = new Regex(@"^[)\]}>）】]", RegexOptions.Compiled);

        public BracketDecoratedNumberParser(INumberParser numberParser) : base(numberParser)
        {
        }

        public override IEnumerable<ParsedResult<BaseDecoratedNumber>> Consume(IDataView data)
        {
            if (data.EndOfStream)
            {
                yield break;
            }

            string left;
            Match m;

            if (!(m = leftBracketRegex.Match(data.CurrentView.ToString())).Success)
            {
                yield break;
            }

            data = data.CloneByDelta(m.Index + m.Length);
            left = m.Value;

            foreach (var numberResult in ConsumeNumber(data))
            {
                data = numberResult.DataView;
                var number = numberResult.Result;

                if (!(m = rightBracketRegex.Match(data.CurrentView.ToString())).Success)
                {
                    continue;
                }

                data = data.CloneByDelta(m.Index + m.Length);
                var right = m.Value;

                yield return new ParsedResult<BaseDecoratedNumber>(
                    result: new BracketDecoratedNumber(number, left + number.RawNumber + right, left, right),
                    dataView: data);
            }
        }
    }
}
