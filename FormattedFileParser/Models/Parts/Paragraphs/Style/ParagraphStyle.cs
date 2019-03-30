using System;
using System.Collections.Generic;
using System.Text;

namespace FormattedFileParser.Models.Parts.Paragraphs.Style
{
    public enum Justification
    {
        Left, Center, Right, Justified, Other
    }

    public struct ParagraphStyle
    {
        public Indentation Indent { get; set; } 

        public Justification Justification { get; set; } 

        public NumberingIndex? NumberingIndex { get; set; }
    }
}
