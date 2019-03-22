using System;
using System.Collections.Generic;
using System.Text;
using ExamPaperParser.Parser.Order.Models;
using ExamPaperParser.Parser.Order.Models.Numbers;
using ExamPaperParser.Parser.Order.Parsers.NumberParsers.Helpers;

namespace ExamPaperParser.Parser.Order.Parsers.NumberParsers
{
    public class UpperChineseNumberParser : BaseNumberParser
    {
        protected override string MatchRegex => @"^[壹贰叁弎肆伍陆柒捌玖拾佰仟萬亿]+";

        protected override BaseNumber ConstructNumber(string rawNumber, int number)
        {
            return new ChineseNumber(rawNumber, number, false);
        }

        protected override int ParseRawNumber(string rawNumber)
        {
            return ChineseNumberHelper.ChineseNumberToInt(rawNumber);
        }
    }
}
