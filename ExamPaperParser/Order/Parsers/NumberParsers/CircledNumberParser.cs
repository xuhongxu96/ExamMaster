using System;
using System.Collections.Generic;
using System.Text;
using ExamPaperParser.Order.Models.Numbers;

namespace ExamPaperParser.Order.Parsers.NumberParsers
{
    public class CircledNumberParser : BaseNumberParser
    {
        protected override string MatchRegex => @"^[\u2460-\u2473]";

        protected override BaseNumber ConstructNumber(string rawNumber, int number)
        {
            return new CircledNumber(rawNumber, number);
        }

        protected override int ParseRawNumber(string rawNumber)
        {
            return rawNumber[0] - '\u2460' + 1;
        }
    }
}
