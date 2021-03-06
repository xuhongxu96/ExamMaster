﻿using FormattedFileParser.Models.Parts;
using FormattedFileParser.Models.Parts.Paragraphs;
using FormattedFileParser.Models.Parts.Texts;
using System;
using System.Collections.Generic;
using System.Linq;
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
                        textPart.Content = textPart.Content.TrimStart();
                        isStart = false;
                        parts.Add(textPart);
                        continue;
                    }
                }

                parts.Add(part);
            }

            paragraphPart.Parts = parts;
            return paragraphPart;
        }

        public static double GetMaxTextSize(this ParagraphPart paragraphPart)
        {
            return paragraphPart.Parts.Where(o => o is TextPart).Max(o => ((TextPart)o).Style.Size);
        }

        public static double GetAverageTextSize(this ParagraphPart paragraphPart)
        {
            return paragraphPart.Parts.Where(o => o is TextPart).Average(o => ((TextPart)o).Style.Size);
        }
    }
}
