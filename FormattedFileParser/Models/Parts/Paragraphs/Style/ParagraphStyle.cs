using System;
using System.Collections.Generic;
using System.Text;

namespace FormattedFileParser.Models.Parts.Paragraphs.Style
{
    public enum Justification
    {
        Left, Center, Right, Justified, Other
    }

    public class ParagraphStyle
    {
        public Indentation Indent { get; set; } = new Indentation();

        public Justification Justification { get; set; } = Justification.Justified;
    }
}
