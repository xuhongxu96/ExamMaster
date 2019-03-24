using System;
using System.Collections.Generic;
using System.Text;

namespace ExamPaperParser.Number.Models.Numbers
{
    public class BaseNumber
    {
        public BaseNumber(string rawNumber, int number)
        {
            RawNumber = rawNumber;
            IntNumber = number;
        }

        public string RawNumber { get; set; }

        public int IntNumber { get; set; }
    }
}
