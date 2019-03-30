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
        private readonly List<Exception> _exceptions = new List<Exception>();

        public BaseVisitNodePostprocessor()
            : this(new NumberNodeVisitor())
        {
        }

        public BaseVisitNodePostprocessor(INumberNodeVisitor numberNodeVisitor)
        {
            _numberNodeVisitor = numberNodeVisitor;
            _numberNodeVisitor.OnVisited += NumberNodeVisitor_OnVisited_CatchException;
        }

        public List<Exception> Process(NumberRoot root)
        {
            _exceptions.Clear();
            _numberNodeVisitor.Visit(root);
            return _exceptions;
        }

        private bool NumberNodeVisitor_OnVisited_CatchException(NumberNode node, int level)
        {
            List<Exception> exceptions; 
            var ret = NumberNodeVisitor_OnVisited(node, level, out exceptions);
            _exceptions.AddRange(exceptions);

            return ret;
        }

        protected abstract bool NumberNodeVisitor_OnVisited(NumberNode node, int level, out List<Exception> exceptions);
    }
}
