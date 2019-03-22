using FormattedFileParser.Models.Parts.Paragraphs.Style;
using FormattedFileParser.Models.Parts.Texts;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
namespace FormattedFileParser.Models.Parts.Paragraphs
{
    [DebuggerDisplay("Content = {Content}, PartCount = {Parts.Count}")]
    public struct ParagraphPart : IMainPart
    {
        public string Content => string.Join("", Parts.Select(o => o.Content));

        public int Order { get; set; } 

        public IList<IPart> Parts { get; set; } 

        public ParagraphStyle Style { get; set; } 
    }
}
