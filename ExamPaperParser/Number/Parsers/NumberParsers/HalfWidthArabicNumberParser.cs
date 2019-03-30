using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using ExamPaperParser.Number.Models.Numbers;

namespace ExamPaperParser.Number.Parsers.NumberParsers
{
    /// <summary>
    /// 0-9
    /// </summary>
    public class HalfWidthArabicNumberParser : BaseNumberParser
    {
        public HalfWidthArabicNumberParser(bool consumeFromStart = true) : base(consumeFromStart)
        {
        }

        protected override string MatchRegex => @"[0-9]+";

        protected override BaseNumber ConstructNumber(string rawNumber, int number)
        {
            return new ArabicNumber(rawNumber, number, true);
        }

        protected override int ParseRawNumber(string rawNumber)
        {
            return int.Parse(rawNumber);
        }
    }
}
