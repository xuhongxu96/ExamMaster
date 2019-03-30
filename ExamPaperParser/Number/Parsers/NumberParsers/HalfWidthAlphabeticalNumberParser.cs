using System;
using System.Collections.Generic;
using System.Text;
using ExamPaperParser.Number.Models.Numbers;

namespace ExamPaperParser.Number.Parsers.NumberParsers
{
    public class HalfWidthAlphabeticalNumberParser : BaseNumberParser
    {
        public HalfWidthAlphabeticalNumberParser(bool consumeFromStart = true) : base(consumeFromStart)
        {
        }

        protected override string MatchRegex => @"[a-zA-Z]";

        protected override BaseNumber ConstructNumber(string rawNumber, int number)
        {
            bool isLower = rawNumber[0] <= 'z' && rawNumber[0] >= 'a';
            return new AlphabeticalNumber(rawNumber, number, isLower, true);
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
