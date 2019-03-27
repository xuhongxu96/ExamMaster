using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using ExamPaperParser.Base;
using ExamPaperParser.DataView;
using ExamPaperParser.Helpers;
using ExamPaperParser.Number.Models.DecoratedNumbers;
using ExamPaperParser.Number.Models.Numbers;
using ExamPaperParser.Number.Parsers.NumberParsers;

namespace ExamPaperParser.Number.Parsers.DecoratedNumberParsers
{
    public class UndecoratedAndDelimiterDecoratedNumberParser : BaseDecoratedNumberParser
    {
        private static Regex delimiterRegex = new Regex(@"^[\s.,:)\]}>．。，、：）】]", RegexOptions.Compiled);
        private static Regex beginWithAlphaRegex = new Regex(@"^[a-zA-Z]", RegexOptions.Compiled);

        public UndecoratedAndDelimiterDecoratedNumberParser(INumberParser numberParser) : base(numberParser)
        {
        }

        public override IEnumerable<ParsedResult<BaseDecoratedNumber>> Consume(IDataView data)
        {
            if (data.EndOfStream)
            {
                yield break;
            }

            Match m;

            foreach (var numberResult in ConsumeNumber(data))
            {
                data = numberResult.DataView;
                var number = numberResult.Result;

                string delimiter;

                if (!(m = delimiterRegex.Match(data.CurrentView.ToString())).Success)
                {
                    if (number is RomanNumber && data.CurrentView.Length > 0 && beginWithAlphaRegex.IsMatch(data.CurrentView.Slice(0, 1).ToString()))
                    {
                        continue;
                    }

                    if (number is ChineseNumber 
                        && data.CurrentView.ToString().ReplaceBetween("[(（]", "[)）]").Length > 14)
                    {
                        continue;
                    }

                    yield return new ParsedResult<BaseDecoratedNumber>(
                        result: new UndecoratedNumber(number),
                        dataView: data);

                    continue;
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
