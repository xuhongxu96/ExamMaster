using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using ExamPaperParser.Parser.DataView;
using ExamPaperParser.Parser.Order.Models.DecoratedNumbers;
using ExamPaperParser.Parser.Order.Models.Numbers;
using ExamPaperParser.Parser.Order.Parsers.NumberParsers;

namespace ExamPaperParser.Parser.Order.Parsers.DecoratedNumberParsers
{
    public class DelimiterDecoratedNumberParser : BaseDecoratedNumberParser
    {
        private static Regex delimiterRegex = new Regex(@"^[.,:)\]}>。，、：）】]", RegexOptions.Compiled);

        public DelimiterDecoratedNumberParser(IEnumerable<BaseNumberParser> numberParsers) : base(numberParsers)
        {
        }

        public override BaseDecoratedNumber? Consume(IDataView data)
        {
            var oldPos = data.Position;

            string result = "";
            BaseNumber? number;
            string delimiter;
            Match m;

            if ((number = ConsumeNumber(data)) == null)
            {
                data.Position = oldPos;
                return null;
            }

            result += number.RawNumber;

            if (!(m = delimiterRegex.Match(data.CurrentView.ToString())).Success)
            {
                data.Position = oldPos;
                return null;
            }
            data.Position += m.Index + m.Length;
            result += delimiter = m.Value;

            return new DelimiterDecoratedNumber(number, result, delimiter);
        }
    }
}
