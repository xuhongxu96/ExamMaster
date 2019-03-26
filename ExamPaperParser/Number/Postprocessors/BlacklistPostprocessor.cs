using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using ExamPaperParser.Number.Models.NumberTree;
using ExamPaperParser.Number.Visitors;

namespace ExamPaperParser.Number.Postprocessors
{
    public class BlacklistPostprocessor : IPostprocessor
    {
        private readonly INumberNodeVisitor _numberNodeVisitor;
        private readonly Regex _blackRegex;

        public BlacklistPostprocessor(Regex blackRegex, INumberNodeVisitor numberNodeVisitor)
        {
            _blackRegex = blackRegex;

            _numberNodeVisitor = numberNodeVisitor;
            _numberNodeVisitor.OnVisited += NumberNodeVisitor_OnVisited;
        }

        private bool NumberNodeVisitor_OnVisited(NumberNode node, int level)
        {
            var newBodySb = new StringBuilder();

            using (var reader = new StringReader(node.Body))
            {
                string? line;
                while ((line = reader.ReadLine()) != null)
                {
                    line = line.Trim();

                    if (!_blackRegex.IsMatch(line))
                    {
                        newBodySb.AppendLine(line);
                    }
                }
            }

            node.Body = newBodySb.ToString();

            return true;
        }

        public BlacklistPostprocessor(Regex blackRegex)
            : this(blackRegex, new NumberNodeVisitor())
        {
        }

        public void Process(NumberRoot root)
        {
            _numberNodeVisitor.Visit(root);
        }
    }
}
