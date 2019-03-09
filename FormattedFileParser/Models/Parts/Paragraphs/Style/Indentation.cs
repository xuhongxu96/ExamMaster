using System;
using System.Collections.Generic;
using System.Text;

namespace FormattedFileParser.Models.Parts.Paragraphs.Style
{
    public class Indentation
    {
        public int Left { get; set; } = 0;

        public int Right { get; set; } = 0;

        public int FirstLine { get; set; } = 0;
    }
}
