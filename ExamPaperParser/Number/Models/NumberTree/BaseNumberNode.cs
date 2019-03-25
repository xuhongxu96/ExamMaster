using System;
using System.Collections.Generic;
using System.Text;

namespace ExamPaperParser.Number.Models.NumberTree
{
    public abstract class BaseNumberNode
    {
        public string? ChildDifferentiator { get; set; } = null;

        public List<NumberNode> Children { get; } = new List<NumberNode>();

        public abstract int Level { get; }
    }
}
