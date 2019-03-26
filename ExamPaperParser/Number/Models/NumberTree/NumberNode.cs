using System;
using System.Collections.Generic;
using System.Text;
using ExamPaperParser.Number.Models.DecoratedNumbers;

namespace ExamPaperParser.Number.Models.NumberTree
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

        public string Header { get; set; } = string.Empty;

        public string Body { get; set; } = string.Empty;

        public BaseNumberNode Parent { get; }

        public override int Level { get; }
    }
}
