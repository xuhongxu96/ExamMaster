using FormattedFileParser.Models;
using FormattedFileParser.Models.Parts.Paragraphs;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExamPaperParser
{
    public class PaperParser
    {
        public void Parse(ParsedFile parsedFile)
        {
            foreach (var part in parsedFile.Parts)
            {
                if (part is ParagraphPart paragraph)
                {
                }
            }
        }
    }
}
