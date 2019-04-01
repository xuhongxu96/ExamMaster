using System;
using System.Collections.Generic;
using System.Text;

namespace ExamPaperParser.Number.Models.Numbers
{
    public class AlphabeticalNumber : BaseNumber
    {
        public AlphabeticalNumber(string rawNumber, int number, bool isLower, bool isHalfWidth) : base(rawNumber, number)
        {
            IsLower = isLower;
            IsHalfWidth = isHalfWidth;
        }

        public bool IsLower { get; }

        public bool IsHalfWidth { get; }
    }
}
