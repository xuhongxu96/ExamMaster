﻿using System;
using System.Collections.Generic;
using System.Text;
using ExamPaperParser.Parser.Order.Models.Numbers;
using ExamPaperParser.Parser.Order.Parsers.NumberParsers.Helpers;

namespace ExamPaperParser.Parser.Order.Parsers.NumberParsers
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
