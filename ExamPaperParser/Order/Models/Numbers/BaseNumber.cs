using System;
using System.Collections.Generic;
using System.Text;

namespace ExamPaperParser.Order.Models.Numbers
{
    public class BaseNumber
    {
        public BaseNumber(string rawNumber, int number)
        {
            RawNumber = rawNumber;
            Number = number;
        }

        public string RawNumber { get; set; }

        public int Number { get; set; }
    }
}
