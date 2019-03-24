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
            return file.Parts
                .Where(o => o is ParagraphPart paragraph && paragraph.Parts.Any())
                .Max(o => ((ParagraphPart)o).Parts.Where(o => o is TextPart).Max(o => ((TextPart)o).Style.Size));
        }
    }
}
