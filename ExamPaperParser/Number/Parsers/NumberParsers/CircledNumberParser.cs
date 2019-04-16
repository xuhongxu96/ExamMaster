using System;
using System.Collections.Generic;
using System.Text;
using ExamPaperParser.Number.Models.Numbers;

namespace ExamPaperParser.Number.Parsers.NumberParsers
{
    public class CircledNumberParser : BaseNumberParser
    {
        public CircledNumberParser(bool consumeFromStart = true) : base(consumeFromStart)
        {
        }

        protected override string MatchRegex => @"[\u2460-\u2473]";

        protected override BaseNumber ConstructNumber(string rawNumber, int number)
        {
            return new CircledNumber(rawNumber, number);
        }

        protected override int? ParseRawNumber(string rawNumber)
        {
            return rawNumber[0] - '\u2460' + 1;
        }
    }
}
