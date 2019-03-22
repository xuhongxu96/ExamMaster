using System;
using System.Collections.Generic;
using System.Text;

namespace ExamPaperParser.Parser.Order.Models.Numbers
{
    public class ChineseNumber : BaseNumber
    {
        public ChineseNumber(string rawNumber, int number, bool isLower) : base(rawNumber, number)
        {
            IsLower = isLower;
        }

        public bool IsLower { get; set; }
    }
}
