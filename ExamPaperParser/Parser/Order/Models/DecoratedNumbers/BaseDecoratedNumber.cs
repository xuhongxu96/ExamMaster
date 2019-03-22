using System;
using System.Collections.Generic;
using System.Text;
using ExamPaperParser.Parser.Order.Models.Numbers;

namespace ExamPaperParser.Parser.Order.Models.DecoratedNumbers
{
    public class BaseDecoratedNumber
    {
        public BaseDecoratedNumber(BaseNumber number, string rawRepresentation)
        {
            Number = number;
            RawRepresentation = rawRepresentation;
        }

        public BaseNumber Number { get; set; }

        public string RawRepresentation { get; set; }
    }
}
