using System;
using System.Collections.Generic;
using System.Text;

namespace FormattedFileParser.Models.Parts.Paragraphs.Style
{
    public enum NumberingStyle
    {
        UpperLetter, LowerLetter, UpperRoman, LowerRoman,
        Decimal, DecimalEnclosedCircle, DecimalEnclosedFullstop, DecimalEnclosedParen, DecimalZero,
        DecimalFullWidth,
        ChineseCounting
    }

    public class Numbering
    {
        public Numbering(NumberingStyle style, int startFrom, string template, string suffix)
        {
            Style = style;
            StartFrom = startFrom;
            Template = template;
            Suffix = suffix;
        }

        public NumberingStyle Style { get; set; }

        public int StartFrom { get; set; }

        public string Template { get; set; }

        public string Suffix { get; set; }
    }
}
