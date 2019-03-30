using System;
using System.Collections.Generic;
using System.Text;

namespace FormattedFileParser.NumberingUtils.Managers
{
    public enum NumberingStyle
    {
        None, Other,
        UpperLetter, LowerLetter, UpperRoman, LowerRoman,
        Decimal, DecimalEnclosedCircle, DecimalEnclosedFullstop, DecimalEnclosedParen, DecimalZero,
        DecimalFullWidth,
        ChineseCounting,
    }

    public struct NumberingDefinition
    {
        public NumberingStyle Style { get; set; }

        public int StartFrom { get; set; }

        public string Template { get; set; }

        public string Suffix { get; set; }
    }
}
