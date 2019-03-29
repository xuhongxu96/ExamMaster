using System;
using System.Collections.Generic;
using System.Text;

namespace FormattedFileParser.Models.Parts.Paragraphs.Style
{
    public enum NumberingStyle
    {
        None,
        UpperLetter, LowerLetter, UpperRoman, LowerRoman,
        Decimal, DecimalEnclosedCircle, DecimalEnclosedFullstop, DecimalEnclosedParen, DecimalZero,
        DecimalFullWidth,
        ChineseCounting
    }

    public struct Numbering
    {
        public int GroupId { get; set; }

        public int Level { get; set; }

        public NumberingStyle Style { get; set; }

        public int StartFrom { get; set; }

        public string Template { get; set; }

        public string Suffix { get; set; }
    }
}
