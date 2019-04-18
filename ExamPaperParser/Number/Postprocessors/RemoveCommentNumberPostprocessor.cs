using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using ExamPaperParser.Number.Models.NumberTree;
using FormattedFileParser.Exceptions;

namespace ExamPaperParser.Number.Postprocessors
{
    public class RemoveCommentNumberPostprocessor : BaseVisitNodePostprocessor
    {
        private static readonly Regex _readingRegex = new Regex(@"阅读", RegexOptions.Compiled);
        private static readonly Regex _commentRegex = new Regex(@"[:：]", RegexOptions.Compiled);

        protected override bool NumberNodeVisitor_OnVisited(NumberNode node, int level, out List<ParagraphFormatException> exceptions)
        {
            exceptions = new List<ParagraphFormatException>();

            var isReading = new Lazy<bool>(() => _readingRegex.IsMatch(node.Header)
                || _readingRegex.IsMatch(node.Body));

            var anyComment = new Lazy<bool>(() => node.Children.Any(o => _commentRegex.IsMatch(o.Header)));

            var isContentShortEnough = new Lazy<bool>(
                () => node.Children.Min(o => o.Header.Replace(" ", "").Replace("\t", "").Length) < 10);

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

            if (isReading.Value)
            {

            }

            if (isReading.Value
                && anyComment.Value
                && isContentShortEnough.Value
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
