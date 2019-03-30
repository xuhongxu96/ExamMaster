using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using ExamPaperParser.Number.Models;
using ExamPaperParser.Number.Models.Numbers;
using ExamPaperParser.Number.Parsers.NumberParsers.Helpers;

namespace ExamPaperParser.Number.Parsers.NumberParsers
{
    /// <summary>
    /// Ⅰ, Ⅱ, Ⅲ or
    /// I, II, III or
    /// </summary>
    public class UpperRomanNumberParser : BaseNumberParser
    {
        public UpperRomanNumberParser(bool consumeFromStart = true) : base(consumeFromStart)
        {
        }

        protected override string MatchRegex => @"[\u2160-\u216FIXVLCDM]+";

        protected override BaseNumber ConstructNumber(string rawNumber, int number)
        {
            return new RomanNumber(rawNumber, number, false);
        }

        protected override int ParseRawNumber(string rawNumber)
        {
            return RomanNumberHelper.RomanToInt(rawNumber);
        }
    }
}
