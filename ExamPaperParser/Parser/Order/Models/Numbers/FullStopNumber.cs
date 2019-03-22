using System;
using System.Collections.Generic;
using System.Text;

namespace ExamPaperParser.Parser.Order.Models.Numbers
{
    /// <summary>
    /// ⒈,⒉
    /// </summary>
    public class FullStopNumber : BaseNumber
    {
        public FullStopNumber(string rawNumber, int number) : base(rawNumber, number)
        {
        }
    }
}
