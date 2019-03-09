using System;
using System.Collections.Generic;
using System.Text;

namespace FormattedFileParser.Models.Parts.Paragraphs.Style
{
    public struct Indentation
    {
        public int Left { get; set; } 

        public int Right { get; set; }

        public int FirstLine { get; set; } 
    }
}
