using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FormattedFileParser.Models;
using FormattedFileParser.Models.Parts.Paragraphs;
using FormattedFileParser.Models.Parts.Texts;

namespace ExamPaperParser.Helpers
{
    public static class ParsedFileExtensions
    {
        public static double GetMaxTextSize(this ParsedFile file)
        {
            var paragraphParts = file.Parts
                            .Where(o => o is ParagraphPart paragraph && paragraph.Parts.Any())
                            .ToArray();

            if (paragraphParts.Any())
            {
                return paragraphParts.Max(o => ((ParagraphPart)o).Parts.Where(o => o is TextPart).Max(o => ((TextPart)o).Style.Size));
            }

            return 0;
        }

        public static double GetAvgTextSize(this ParsedFile file)
        {
            var paragraphParts = file.Parts
                            .Where(o => o is ParagraphPart paragraph && paragraph.Parts.Any())
                            .ToArray();

            if (paragraphParts.Any())
            {
                return paragraphParts.Average(o => ((ParagraphPart)o).Parts.Where(o => o is TextPart).Max(o => ((TextPart)o).Style.Size));
            }

            return 0;
        }
    }
}
