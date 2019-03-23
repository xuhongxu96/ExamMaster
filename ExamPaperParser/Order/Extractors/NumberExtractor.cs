using ExamPaperParser.DataView;
using ExamPaperParser.Helpers;
using ExamPaperParser.Order.Models;
using ExamPaperParser.Order.Parsers.DecoratedNumberParsers;
using FormattedFileParser.Models;
using FormattedFileParser.Models.Parts.Paragraphs;
using FormattedFileParser.Models.Parts.Texts;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExamPaperParser.Order.Extractors
{
    public class NumberExtractor
    {
        private IDecoratedNumberParser _decoratedNumberParser;

        public NumberExtractor(IDecoratedNumberParser decoratedNumberParser)
        {
            _decoratedNumberParser = decoratedNumberParser;
        }

        private IEnumerable<string> ExtractFromParagraph(ParagraphPart paragraph)
        {
            paragraph = paragraph.TrimStart();
            var content = paragraph.Content.Trim();
            var data = new StringDataView(content);

            foreach (var item in _decoratedNumberParser.Consume(data))
            {
                yield return $"({item.Result.GetType().Name}, {item.Result.Number.GetType().Name}) {item.Result.Number.Number}\n\n{content}";
            }
        }

        public IEnumerable<string> Extract(ParsedFile file)
        {
            foreach (var part in file.Parts)
            {
                if (part is ParagraphPart paragraph)
                {
                    foreach (var item in ExtractFromParagraph(paragraph))
                    {
                        yield return item;
                    }
                }
            }
        }
    }
}
