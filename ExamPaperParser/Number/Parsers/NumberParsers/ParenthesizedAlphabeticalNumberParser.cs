using System;
using System.Collections.Generic;
using System.Text;
using ExamPaperParser.Number.Models.Numbers;

namespace ExamPaperParser.Number.Parsers.NumberParsers
{
    public class ParenthesizedAlphabeticalNumberParser : BaseNumberParser
    {
        protected override string MatchRegex => @"^[\u249C-\u24B5]";

        protected override BaseNumber ConstructNumber(string rawNumber, int number)
        {
            return new ParenthesizedAlphabeticalNumber(rawNumber, number);
        }

        protected override int ParseRawNumber(string rawNumber)
        {
            return rawNumber[0] - '\u249C' + 1;
        }
    }
}
