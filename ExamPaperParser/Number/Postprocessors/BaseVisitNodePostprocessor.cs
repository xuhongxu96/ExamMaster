using System;
using System.Collections.Generic;
using System.Text;
using ExamPaperParser.Number.Models.NumberTree;
using ExamPaperParser.Number.Visitors;
using FormattedFileParser.Exceptions;

namespace ExamPaperParser.Number.Postprocessors
{
    public abstract class BaseVisitNodePostprocessor : IPostprocessor
    {
        private readonly INumberNodeVisitor _numberNodeVisitor;
        private readonly List<ParagraphFormatException> _exceptions = new List<ParagraphFormatException>();

        public BaseVisitNodePostprocessor()
            : this(new NumberNodeVisitor())
        {
        }

        public BaseVisitNodePostprocessor(INumberNodeVisitor numberNodeVisitor)
        {
            _numberNodeVisitor = numberNodeVisitor;
            _numberNodeVisitor.OnVisited += NumberNodeVisitor_OnVisited_CatchException;
        }

        public List<ParagraphFormatException> Process(NumberRoot root)
        {
            _exceptions.Clear();
            _numberNodeVisitor.Visit(root);
            return _exceptions;
        }

        private bool NumberNodeVisitor_OnVisited_CatchException(NumberNode node, int level)
        {
            List<ParagraphFormatException> exceptions;
            var ret = NumberNodeVisitor_OnVisited(node, level, out exceptions);
            _exceptions.AddRange(exceptions);

            return ret;
        }

        protected abstract bool NumberNodeVisitor_OnVisited(NumberNode node, int level, out List<ParagraphFormatException> exceptions);
    }
}
