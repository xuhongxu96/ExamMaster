using System;
using System.Collections.Generic;
using System.Text;

namespace ExamPaperParser.Order.Models.Numbers
{
    /// <summary>
    /// ⑴, ⑵
    /// </summary>
    public class ParenthesizedNumber : BaseNumber
    {
        public ParenthesizedNumber(string rawNumber, int number) : base(rawNumber, number)
        {
        }
    }
}
