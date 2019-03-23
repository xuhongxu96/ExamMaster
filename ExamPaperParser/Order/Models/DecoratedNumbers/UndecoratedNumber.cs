using System;
using System.Collections.Generic;
using System.Text;
using ExamPaperParser.Order.Models.Numbers;

namespace ExamPaperParser.Order.Models.DecoratedNumbers
{
    public class UndecoratedNumber : BaseDecoratedNumber
    {
        public UndecoratedNumber(BaseNumber number)
            : base(number, number.RawNumber)
        {
        }
    }
}
