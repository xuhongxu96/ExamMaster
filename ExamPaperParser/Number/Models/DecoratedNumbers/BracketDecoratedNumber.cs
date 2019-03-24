using System;
using System.Collections.Generic;
using System.Text;
using ExamPaperParser.Number.Models.Numbers;

namespace ExamPaperParser.Number.Models.DecoratedNumbers
{
    public class BracketDecoratedNumber : BaseDecoratedNumber
    {
        public BracketDecoratedNumber(BaseNumber number, string rawRepresentation, string leftBracket, string rightBracket)
            : base(number, rawRepresentation)
        {
            LeftBracket = leftBracket;
            RightBracket = rightBracket;
        }

        public string LeftBracket { get; set; }

        public string RightBracket { get; set; }
    }
}
