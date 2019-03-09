using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace FormattedFileParser.Models.Parts.Texts
{
    [DebuggerDisplay("Content = {Content}, Style = {Style}")]
    public class TextPart : Part
    {
        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        public TextStyle Style { get; set; } = new TextStyle();
    }
}
