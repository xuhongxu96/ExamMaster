using System;
using System.Collections.Generic;
using System.Text;

namespace FormattedFileParser.Models.Parts.Texts
{
    public struct HyperlinkPart : ITextPart
    {
        public string Content { get; set; }
        public TextStyle Style { get; set; }
        public string Link { get; set; } 
    }
}
