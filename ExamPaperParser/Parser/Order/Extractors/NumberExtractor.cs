using ExamPaperParser.Parser.Helpers;
using ExamPaperParser.Parser.Order.Models;
using FormattedFileParser.Models;
using FormattedFileParser.Models.Parts.Paragraphs;
using FormattedFileParser.Models.Parts.Texts;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExamPaperParser.Parser.Order.Extractors
{
    public class NumberExtractor
    {
        private IEnumerable<string> ExtractFromParagraph(ParagraphPart paragraph)
        {
            paragraph = paragraph.TrimStart();
            var content = paragraph.Content;
            yield return "";
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
