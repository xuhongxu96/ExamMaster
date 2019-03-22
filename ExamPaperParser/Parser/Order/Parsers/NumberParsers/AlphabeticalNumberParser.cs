using System;
using System.Collections.Generic;
using System.Text;
using ExamPaperParser.Parser.Order.Models.Numbers;

namespace ExamPaperParser.Parser.Order.Parsers.NumberParsers
{
    public class AlphabeticalNumberParser : BaseNumberParser
    {
        protected override string MatchRegex => @"^[a-z][A-Z]";

        protected override BaseNumber ConstructNumber(string rawNumber, int number)
        {
            bool isLower = rawNumber[0] <= 'z' && rawNumber[0] >= 'a';
            return new AlphabeticalNumber(rawNumber, number, isLower);
        }

        protected override int ParseRawNumber(string rawNumber)
        {
            bool isLower = rawNumber[0] <= 'z' && rawNumber[0] >= 'a';
            if (isLower)
            {
                return rawNumber[0] - 'a' + 1;
            }
            else
            {
                return rawNumber[0] - 'A' + 1;
            }
        }
    }
}
