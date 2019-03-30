using System;
using System.Collections.Generic;
using System.Text;
using ExamPaperParser.Number.Models.Numbers;

namespace ExamPaperParser.Number.Parsers.NumberParsers
{
    public class FullWidthAlphabeticalNumberParser : BaseNumberParser
    {
        public FullWidthAlphabeticalNumberParser(bool consumeFromStart = true) : base(consumeFromStart)
        {
        }

        protected override string MatchRegex => @"[\uFF21-\uFF3A\uFF41-\uFF5A]";

        protected override BaseNumber ConstructNumber(string rawNumber, int number)
        {
            bool isLower = rawNumber[0] <= '\uFF5A' && rawNumber[0] >= '\uFF41';
            return new AlphabeticalNumber(rawNumber, number, isLower, false);
        }

        protected override int ParseRawNumber(string rawNumber)
        {
            bool isLower = rawNumber[0] <= '\uFF5A' && rawNumber[0] >= '\uFF41';
            if (isLower)
            {
                return rawNumber[0] - '\uFF41' + 1;
            }
            else
            {
                return rawNumber[0] - '\uFF21' + 1;
            }
        }
    }
}
