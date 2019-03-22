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
    public class BracketDecoratedNumberParser : BaseDecoratedNumberParser
    {
        private static Regex leftBracketRegex = new Regex(@"^[(\[{<（【]", RegexOptions.Compiled);
        private static Regex rightBracketRegex = new Regex(@"^[)\]}>）】]", RegexOptions.Compiled);

        public BracketDecoratedNumberParser(IEnumerable<BaseNumberParser> numberParsers) : base(numberParsers)
        {
        }

        public override BaseDecoratedNumber? Consume(IDataView data)
        {
            var oldPos = data.Position;

            var result = "";
            string left, right;
            BaseNumber number;
            Match m;

            if (!(m = leftBracketRegex.Match(data.CurrentView.ToString())).Success)
            {
                data.Position = oldPos;
                return null;
            }

            data.Position += m.Index + m.Length;
            result += left = m.Value;
            if ((number = ConsumeNumber(data)) == null)
            {
                data.Position = oldPos;
                return null;
            }

            result += number.RawNumber;

            if (!(m = rightBracketRegex.Match(data.CurrentView.ToString())).Success)
            {
                data.Position = oldPos;
                return null;
            }

            data.Position += m.Index + m.Length;
            result += right = m.Value;

            return new BracketDecoratedNumber(number, result, left, right);
        }
    }
}
