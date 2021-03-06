﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace FormattedFileParser.Models.Parts.Texts
{
    [DebuggerDisplay("Content = {Content}, Style = {Style}")]
    public struct TextPart : ITextPart
    {
        public string Content { get; set; }

        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        public TextStyle Style { get; set; } 
    }
}
