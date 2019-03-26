using System;
using System.Collections.Generic;
using System.Text;
using ExamPaperParser.Number.Models.NumberTree;
using ExamPaperParser.Number.Visitors;

namespace ExamPaperParser.Number.Postprocessors
{
    public abstract class BaseVisitNodePostprocessor : IPostprocessor
    {
        private readonly INumberNodeVisitor _numberNodeVisitor;

        public BaseVisitNodePostprocessor()
            : this(new NumberNodeVisitor())
        {
        }

        public BaseVisitNodePostprocessor(INumberNodeVisitor numberNodeVisitor)
        {
            _numberNodeVisitor = numberNodeVisitor;
            _numberNodeVisitor.OnVisited += NumberNodeVisitor_OnVisited;
        }

        public void Process(NumberRoot root)
        {
            _numberNodeVisitor.Visit(root);
        }

        protected abstract bool NumberNodeVisitor_OnVisited(NumberNode node, int level);
    }
}
