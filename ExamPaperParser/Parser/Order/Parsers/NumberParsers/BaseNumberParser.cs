using ExamPaperParser.Parser.DataView;
using ExamPaperParser.Parser.Order.Models.Numbers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace ExamPaperParser.Parser.Order.Parsers.NumberParsers
{
    public abstract class BaseNumberParser : IParser<BaseNumber>
    {
        protected abstract string MatchRegex { get; }
        protected abstract int ParseRawNumber(string rawNumber);
        protected abstract BaseNumber ConstructNumber(string rawNumber, int number);

        protected Regex regex;

        public BaseNumberParser()
        {
            regex = new Regex(MatchRegex, RegexOptions.Compiled);
        }

        public BaseNumber? Consume(IDataView data)
        {
            var m = regex.Match(data.CurrentView.ToString());
            if (!m.Success)
            {
                return null;
            }

            data.Position += m.Index + m.Length;
            var rawNumber = m.Value;
            return ConstructNumber(rawNumber, ParseRawNumber(rawNumber));
        }
    }
}
