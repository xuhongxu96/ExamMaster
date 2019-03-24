using System;
using System.Collections.Generic;
using System.Text;
using ExamPaperParser.Number.Models.Numbers;

namespace ExamPaperParser.Number.Models.DecoratedNumbers
{
    public class UndecoratedNumber : BaseDecoratedNumber
    {
        public UndecoratedNumber(BaseNumber number)
            : base(number, number.RawNumber)
        {
        }
    }
}
