using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using ExamPaperParser.Number.Models.NumberTree;
using FormattedFileParser.Exceptions;

namespace ExamPaperParser.Number.Postprocessors
{
    public class RemoveArticleNumberPostprocessor : BaseVisitNodePostprocessor
    {
        private static readonly Regex _readingRegex = new Regex(@"阅读", RegexOptions.Compiled);

        protected override bool NumberNodeVisitor_OnVisited(NumberNode node, int level, out List<ParagraphFormatException> exceptions)
        {
            exceptions = new List<ParagraphFormatException>();

            var isReading = new Lazy<bool>(() => _readingRegex.IsMatch(node.Header)
                || _readingRegex.IsMatch(node.Body));

            var isContentLongEnough = new Lazy<bool>(() => node.Children.Count > 3
                && node.Children.Sum(o => o.Header.Length) > 200);

            var hasNoScore = new Lazy<bool>(() => !node.Children
                .Any(o => o.Score != 0));

            var onlyLastChildHasBodyOrNoBody = new Lazy<bool>(
                () =>
                {
                    var result = node.Children.FirstOrDefault(o => !string.IsNullOrWhiteSpace(o.Body));
                    return result == null || result == node.Children.Last();
                });

            var onlyLastChildHasChildren = new Lazy<bool>(() => node.Children.FirstOrDefault(o => o.Children.Count > 0)
                == node.Children.Last());

            if (isReading.Value
                && isContentLongEnough.Value
                && hasNoScore.Value
                && onlyLastChildHasBodyOrNoBody.Value
                && onlyLastChildHasChildren.Value)
            {
                var lastChild = node.Children.Last();

                node.Body = $"{node.Body}\n{NodeHelper.ConcatNodeContent(node, recursive: false)}".Trim();

                node.Children.Clear();
                node.Children.AddRange(lastChild.Children);

                return false;
            }

            return true;
        }
    }
}
