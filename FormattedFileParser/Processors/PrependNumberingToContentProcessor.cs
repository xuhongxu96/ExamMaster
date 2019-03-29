using System;
using System.Collections.Generic;
using System.Text;
using FormattedFileParser.Models;
using FormattedFileParser.Models.Parts.Paragraphs;

namespace FormattedFileParser.Processors
{
    public class PrependNumberingToContentProcessor
    {
        public void Process(ParsedFile file)
        {
            foreach (var part in file.Parts)
            {
                if (part is ParagraphPart paragraphPart)
                {

                }
            }
        }
    }
}
