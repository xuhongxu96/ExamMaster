using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using ExamPaperParser.Parser.Order.Models.Numbers;
using ExamPaperParser.Parser.Order.Parsers.NumberParsers.Helpers;

namespace ExamPaperParser.Parser.Order.Parsers.NumberParsers
{
    /// <summary>
    /// ⅰ, ⅱ, ⅲ or
    /// i, ii, iii, iv, vi, xi
    /// </summary>
    public class LowerRomanNumberParser : BaseNumberParser
    {
        protected override string MatchRegex => @"^([u2170-\u217Fixvlcdm]+)";

        protected override BaseNumber ConstructNumber(string rawNumber, int number)
        {
            return new RomanNumber(rawNumber, number, true);
        }

        protected override int ParseRawNumber(string rawNumber)
        {
            return RomanNumberHelper.RomanToInt(rawNumber);
        }
    }
}
