using System;
using System.Collections.Generic;
using System.Text;

namespace ExamPaperParser.Number.Models.Numbers
{
    public class ArabicNumber : BaseNumber
    {
        public ArabicNumber(string rawNumber, int number, bool isHalfWidth) : base(rawNumber, number)
        {
            IsHalfWidth = isHalfWidth;
        }

        public bool IsHalfWidth { get; set; }
    }
}
