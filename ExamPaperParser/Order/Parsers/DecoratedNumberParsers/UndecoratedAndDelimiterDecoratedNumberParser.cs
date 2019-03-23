using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using ExamPaperParser.Base;
using ExamPaperParser.DataView;
using ExamPaperParser.Order.Models.DecoratedNumbers;
using ExamPaperParser.Order.Models.Numbers;
using ExamPaperParser.Order.Parsers.NumberParsers;

namespace ExamPaperParser.Order.Parsers.DecoratedNumberParsers
{
    public class UndecoratedAndDelimiterDecoratedNumberParser : BaseDecoratedNumberParser
    {
        private static Regex delimiterRegex = new Regex(@"^[.,:)\]}>．。，、：）】]", RegexOptions.Compiled);

        public UndecoratedAndDelimiterDecoratedNumberParser(INumberParser numberParser) : base(numberParser)
        {
        }

        public override IEnumerable<ParsedResult<BaseDecoratedNumber>> Consume(IDataView data)
        {
            Match m;

            foreach (var numberResult in ConsumeNumber(data))
            {
                data = numberResult.DataView;
                var number = numberResult.Result;

                string delimiter;

                if (!(m = delimiterRegex.Match(data.CurrentView.ToString())).Success)
                {
                    yield return new ParsedResult<BaseDecoratedNumber>(
                        result: new UndecoratedNumber(number),
                        dataView: data);
                    yield break;
                }

                data = data.CloneByDelta(m.Index + m.Length);
                delimiter = m.Value;

                yield return new ParsedResult<BaseDecoratedNumber>(
                    result: new DelimiterDecoratedNumber(number, number.RawNumber + delimiter, delimiter),
                    dataView: data);
            }
        }
    }
}
