using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using ExamPaperParser.Number.Models.DecoratedNumbers;

namespace ExamPaperParser.Number.Models.NumberTree
{
    [DebuggerDisplay("Header = {Header}, Content = {Content}")]
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

        public BaseNumberNode Parent { get; }

        public override int Level { get; }

        public int ParagraphOrder { get; }

        public string Header { get; set; } = string.Empty;

        public string Body { get; set; } = string.Empty;

        public double Score { get; set; } = 0.0;

        public bool IsChoiceQuestion { get; set; } = false;

        public string SelectiveDescription { get; set; } = "";

        public int SelectCount { get; set; } = 0;
    }
}
