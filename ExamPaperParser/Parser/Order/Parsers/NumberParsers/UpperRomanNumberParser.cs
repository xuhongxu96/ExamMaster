using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using ExamPaperParser.Parser.Order.Models;
using ExamPaperParser.Parser.Order.Models.Numbers;
using ExamPaperParser.Parser.Order.Parsers.NumberParsers.Helpers;

namespace ExamPaperParser.Parser.Order.Parsers.NumberParsers
{
    /// <summary>
    /// Ⅰ, Ⅱ, Ⅲ or
    /// I, II, III or
    /// </summary>
    public class UpperRomanNumberParser : BaseNumberParser
    {
        protected override string MatchRegex => @"^([u2160-\u216FIXVLCDM]+)";

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
