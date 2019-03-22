﻿using System;
using System.Collections.Generic;
using System.Text;
using ExamPaperParser.Parser.Order.Models;
using ExamPaperParser.Parser.Order.Models.Numbers;

namespace ExamPaperParser.Parser.Order.Parsers.NumberParsers
{
    public class ParenthesizedNumberParser : BaseNumberParser
    {
        protected override string MatchRegex => @"^[\u2474-\u2487]";

        protected override BaseNumber ConstructNumber(string rawNumber, int number)
        {
            return new ParenthesizedNumber(rawNumber, number);
        }

        protected override int ParseRawNumber(string rawNumber)
        {
            return rawNumber[0] - '\u2474' + 1;
        }
    }
}
