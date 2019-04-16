using System;
using System.Collections.Generic;
using System.Text;
using ExamPaperParser.Number.Models;
using ExamPaperParser.Number.Models.Numbers;
using ExamPaperParser.Number.Parsers.NumberParsers.Helpers;

namespace ExamPaperParser.Number.Parsers.NumberParsers
{
    public class UpperChineseNumberParser : BaseNumberParser
    {
        public UpperChineseNumberParser(bool consumeFromStart = true) : base(consumeFromStart)
        {
        }

        protected override string MatchRegex => @"[壹贰叁弎肆伍陆柒捌玖拾佰仟萬亿]+";

        protected override BaseNumber ConstructNumber(string rawNumber, int number)
        {
            return new ChineseNumber(rawNumber, number, false);
        }

        protected override int? ParseRawNumber(string rawNumber)
        {
            return ChineseNumberHelper.ChineseNumberToInt(rawNumber);
        }
    }
}
