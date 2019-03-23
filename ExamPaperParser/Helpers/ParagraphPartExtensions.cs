using FormattedFileParser.Models.Parts;
using FormattedFileParser.Models.Parts.Paragraphs;
using FormattedFileParser.Models.Parts.Texts;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExamPaperParser.Helpers
{
    public static class ParagraphPartExtensions
    {
        public static ParagraphPart TrimStart(this ParagraphPart paragraphPart)
        {
            var parts = new List<IPart>();
            var isStart = true;
            foreach (var part in paragraphPart.Parts)
            {
                if (part is TextPart textPart)
                {
                    if (string.IsNullOrWhiteSpace(textPart.Content))
                    {
                        if (isStart)
                        {
                            continue;
                        }
                    }
                    else if (isStart)
                    {
                        isStart = false;
                    }
                }

                parts.Add(part);
            }

            paragraphPart.Parts = parts;
            return paragraphPart;
        }
    }
}
