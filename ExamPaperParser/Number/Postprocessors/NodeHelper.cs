using System;
using System.Collections.Generic;
using System.Text;
using ExamPaperParser.Number.Models.NumberTree;

namespace ExamPaperParser.Number.Postprocessors
{
    public static class NodeHelper
    {
        public static string ConcatNodeContent(NumberNode node, bool root = true, bool recursive = true)
        {
            var sb = new StringBuilder();
            if (!root)
            {
                sb.AppendLine($"{node.DecoratedNumber.RawRepresentation}{node.Header}\n{node.Body}".Trim());
            }

            if (root || recursive)
            {
                foreach (var child in node.Children)
                {
                    sb.AppendLine(ConcatNodeContent(child, false, recursive));
                }
            }

            return sb.ToString();
        }
    }
}
