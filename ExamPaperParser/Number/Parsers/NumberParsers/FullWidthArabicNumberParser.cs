using System;
using System.Collections.Generic;
using System.Text;
using ExamPaperParser.Number.Models.Numbers;

namespace ExamPaperParser.Number.Parsers.NumberParsers
{
    /// <summary>
    /// ０-９
    /// </summary>
    public class FullWidthArabicNumberParser : BaseNumberParser
    {
        public FullWidthArabicNumberParser(bool consumeFromStart = true) : base(consumeFromStart)
        {
        }

        protected override string MatchRegex => @"[\uFF10-\uFF19]+";

        protected override BaseNumber ConstructNumber(string rawNumber, int number)
        {
            return new ArabicNumber(rawNumber, number, false);
        }

        protected override int ParseRawNumber(string rawNumber)
        {
            var result = 0;

            for (var i = 0; i < rawNumber.Length; ++i)
            {
                result *= 10;
                result += rawNumber[i] - '\uFF10';
            }

            return result;
        }
    }
}
