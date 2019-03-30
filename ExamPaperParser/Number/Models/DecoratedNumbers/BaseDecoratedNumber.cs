using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using ExamPaperParser.Number.Models.Numbers;

namespace ExamPaperParser.Number.Models.DecoratedNumbers
{
    [DebuggerDisplay("{RawRepresentation} ({Number})")]
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
