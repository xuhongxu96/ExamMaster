using System;
using System.Collections.Generic;
using System.Text;
using ExamPaperParser.Number.Models.Numbers;

namespace ExamPaperParser.Number.Models.DecoratedNumbers
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
