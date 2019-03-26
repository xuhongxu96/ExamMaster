using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using ExamPaperParser.Number.Models.NumberTree;

namespace ExamPaperParser.Number.Postprocessors
{
    public class QuestionScorePostprocessor : BaseVisitNodePostprocessor
    {
        Regex _currentScoreRegex = new Regex(@"[(（]\s*(?<score>\d+(\.\d+)?)\s*分\s*[)）]", RegexOptions.Compiled);
        Regex _sumScoreRegex = new Regex(@"[共|总][计]?\s*(?<score>\d+(\.\d+)?)\s*分", RegexOptions.Compiled);
        Regex _childScoreRegex = new Regex(@"[每|各].*?[题]\s*(?<score>\d+(\.\d+)?)\s*分", RegexOptions.Compiled);

        protected override bool NumberNodeVisitor_OnVisited(NumberNode node, int level)
        {
            var m = _childScoreRegex.Match(node.Header);
            if (m.Success)
            {
                var score = double.Parse(m.Groups["score"].Value);
                foreach (var child in node.Children)
                {
                    child.Score = score;
                }
            }

            m = _sumScoreRegex.Match(node.Header);
            if (m.Success)
            {
                var score = double.Parse(m.Groups["score"].Value);
                node.Score = score;
            }

            if (node.Score != 0)
            {
                return true;
            }

            m = _currentScoreRegex.Match(node.Header);
            if (m.Success)
            {
                var score = double.Parse(m.Groups["score"].Value);
                node.Score = score;
            }

            return true;
        }
    }
}
