using System;
using System.Collections.Generic;
using System.Text;
using ExamPaperParser.Number.Models.DecoratedNumbers;

namespace ExamPaperParser.Number.Models.PaperNumbers
{
    public class PaperNumber
    {
        public PaperNumber(int paragraphOrder, ISet<BaseDecoratedNumber> decoratedNumber)
        {
            ParagraphOrder = paragraphOrder;
            DecoratedNumber = decoratedNumber;
        }

        public int ParagraphOrder { get; set; }

        public ISet<BaseDecoratedNumber> DecoratedNumber { get; set; }
    }
}
