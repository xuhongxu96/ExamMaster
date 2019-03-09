using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace FormattedFileParser.Models.Parts.Texts
{
    public interface ITextPart : IPart
    {
        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        TextStyle Style { get; set; } 
    }
}
