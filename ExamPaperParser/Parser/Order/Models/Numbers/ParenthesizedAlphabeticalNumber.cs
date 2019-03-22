using System;
using System.Collections.Generic;
using System.Text;

namespace ExamPaperParser.Parser.Order.Models.Numbers
{
    public class ParenthesizedAlphabeticalNumber : BaseNumber
    {
        public ParenthesizedAlphabeticalNumber(string rawNumber, int number) : base(rawNumber, number)
        {
        }
    }
}
