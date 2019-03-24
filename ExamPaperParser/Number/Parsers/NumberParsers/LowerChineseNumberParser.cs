using System;
using System.Collections.Generic;
using System.Text;
using ExamPaperParser.Number.Models.Numbers;
using ExamPaperParser.Number.Parsers.NumberParsers.Helpers;

namespace ExamPaperParser.Number.Parsers.NumberParsers
{
    public class LowerChineseNumberParser : BaseNumberParser
    {
        protected override string MatchRegex => @"^[一二两三四五六七八九十百千万亿]+";

        protected override BaseNumber ConstructNumber(string rawNumber, int number)
        {
            return new ChineseNumber(rawNumber, number, true);
        }

        protected override int ParseRawNumber(string rawNumber)
        {
            return ChineseNumberHelper.ChineseNumberToInt(rawNumber);
        }
    }
}
