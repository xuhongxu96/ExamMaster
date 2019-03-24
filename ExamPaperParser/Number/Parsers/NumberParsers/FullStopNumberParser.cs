using System;
using System.Collections.Generic;
using System.Text;
using ExamPaperParser.Number.Models.Numbers;

namespace ExamPaperParser.Number.Parsers.NumberParsers
{
    public class FullStopNumberParser : BaseNumberParser
    {
        protected override string MatchRegex => @"^[\u2488-\u249B]";

        protected override BaseNumber ConstructNumber(string rawNumber, int number)
        {
            return new FullStopNumber(rawNumber, number);
        }

        protected override int ParseRawNumber(string rawNumber)
        {
            return rawNumber[0] - '\u2488' + 1;
        }
    }
}
