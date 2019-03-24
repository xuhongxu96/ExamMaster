using System;
using System.Collections.Generic;
using System.Text;

namespace ExamPaperParser.Number.Models.Numbers
{
    public class RomanNumber : BaseNumber
    {
        public RomanNumber(string rawNumber, int number, bool isLower) : base(rawNumber, number)
        {
            IsLower = isLower;
        }

        public bool IsLower { get; set; }
    }
}
