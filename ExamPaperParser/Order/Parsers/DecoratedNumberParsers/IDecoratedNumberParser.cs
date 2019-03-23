using System;
using System.Collections.Generic;
using System.Text;
using ExamPaperParser.Base;
using ExamPaperParser.Order.Models.DecoratedNumbers;

namespace ExamPaperParser.Order.Parsers.DecoratedNumberParsers
{
    public interface IDecoratedNumberParser : IParser<BaseDecoratedNumber>
    {
    }
}
