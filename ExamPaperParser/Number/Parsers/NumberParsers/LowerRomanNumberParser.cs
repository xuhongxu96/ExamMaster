using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using ExamPaperParser.Number.Models.Numbers;
using ExamPaperParser.Number.Parsers.NumberParsers.Helpers;

namespace ExamPaperParser.Number.Parsers.NumberParsers
{
    /// <summary>
    /// ⅰ, ⅱ, ⅲ or
    /// i, ii, iii, iv, vi, xi
    /// </summary>
    public class LowerRomanNumberParser : BaseNumberParser
    {
        public LowerRomanNumberParser(bool consumeFromStart = true) : base(consumeFromStart)
        {
        }

        protected override string MatchRegex => @"([\u2170-\u217Fixvlcdm]+)";

        protected override BaseNumber ConstructNumber(string rawNumber, int number)
        {
            return new RomanNumber(rawNumber, number, true);
        }

        protected override int? ParseRawNumber(string rawNumber)
        {
            return RomanNumberHelper.RomanToInt(rawNumber);
        }
    }
}
