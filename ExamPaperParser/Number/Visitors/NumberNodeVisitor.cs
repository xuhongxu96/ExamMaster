using System;
using System.Collections.Generic;
using System.Text;
using ExamPaperParser.Number.Models.NumberTree;

namespace ExamPaperParser.Number.Visitors
{
    public class NumberNodeVisitor : INumberNodeVisitor
    {
        public event OnVisited OnVisited;

        private void VisitNode(NumberNode node, int level)
        {
            if (!OnVisited.Invoke(node, level))
            {
                return;
            }

            foreach (var child in node.Children)
            {
                VisitNode(child, level + 1);
            }
        }

        public void Visit(NumberRoot root)
        {
            foreach (var child in root.Children)
            {
                VisitNode(child, 0);
            }
        }
    }
}
