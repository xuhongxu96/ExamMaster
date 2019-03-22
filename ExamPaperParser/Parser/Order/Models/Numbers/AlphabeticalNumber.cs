using System;
using System.Collections.Generic;
using System.Text;

namespace ExamPaperParser.Parser.Order.Models.Numbers
{
    public class AlphabeticalNumber : BaseNumber
    {
        public AlphabeticalNumber(string rawNumber, int number, bool isLower) : base(rawNumber, number)
        {
            IsLower = isLower;
        }

        public bool IsLower { get; set; }
    }
}
