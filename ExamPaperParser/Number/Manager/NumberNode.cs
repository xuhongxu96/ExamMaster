using System;
using System.Collections.Generic;
using System.Text;
using ExamPaperParser.Number.Models.DecoratedNumbers;

namespace ExamPaperParser.Number.Manager
{
    public class NumberNode : BaseNumberNode
    {
        public NumberNode(BaseNumberNode parent, BaseDecoratedNumber number, int paragraphOrder)
        {
            DecoratedNumber = number;
            ParagraphOrder = paragraphOrder;
            Parent = parent;

            Level = parent.Level + 1;
        }

        public BaseDecoratedNumber DecoratedNumber { get; }

        public int ParagraphOrder { get; }

        public bool HasAdjacentNumber { get; set; } = false;

        public BaseNumberNode Parent { get; }

        public override int Level { get; }
    }
}
