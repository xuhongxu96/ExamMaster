using System;
using System.Collections.Generic;
using System.Text;
using ExamPaperParser.Number.Models.Numbers;

namespace ExamPaperParser.Number.Models.DecoratedNumbers
{
    public class DelimiterDecoratedNumber : BaseDecoratedNumber
    {
        public DelimiterDecoratedNumber(BaseNumber number, string rawRepresentation, string delimiter)
            : base(number, rawRepresentation)
        {
            Delimiter = delimiter;
        }

        public string Delimiter { get; set; }
    }
}
