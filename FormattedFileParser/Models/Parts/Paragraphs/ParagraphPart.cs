using FormattedFileParser.Models.Parts.Paragraphs.Style;
using FormattedFileParser.Models.Parts.Texts;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
namespace FormattedFileParser.Models.Parts.Paragraphs
{
    [DebuggerDisplay("Content = {Content}, PartCount = {Parts.Count}")]
    public class ParagraphPart : Part, IMainPart
    {
        public int Order { get; set; } = -1;

        public IList<Part> Parts { get; set; } = new List<Part>();

        public ParagraphStyle Style { get; set; } = new ParagraphStyle();
    }
}
