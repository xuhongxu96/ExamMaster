using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Text;

namespace FormattedFileParser.Models.Parts.Texts
{
    public class TextStyle : IEquatable<TextStyle>
    {
        public int Size { get; set; } = 0;
        public bool IsBold { get; set; } = false;
        public bool IsItalic { get; set; } = false;
        public bool IsUnderlined { get; set; } = false;
        public bool IsEmphasized { get; set; } = false;
        public string TextColor { get; set; } = "000000";

        public override bool Equals(object obj)
        {
            return Equals(obj as TextStyle);
        }

        public bool Equals(TextStyle other)
        {
            return other != null &&
                   Size == other.Size &&
                   IsBold == other.IsBold &&
                   IsItalic == other.IsItalic &&
                   IsUnderlined == other.IsUnderlined &&
                   IsEmphasized == other.IsEmphasized &&
                   TextColor == other.TextColor;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Size, IsBold, IsItalic, IsUnderlined, IsEmphasized, TextColor);
        }

        public override string ToString()
        {
            var items = new List<string>();
            items.Add($"Size = {Size}");
            items.Add($"Color = {TextColor}");

            if (IsBold)
                items.Add("Bold");
            if (IsItalic)
                items.Add("Italic");
            if (IsUnderlined)
                items.Add("Underlined");
            if (IsEmphasized)
                items.Add("Emphasized");

            return string.Join(", ", items);
        }

        public static bool operator ==(TextStyle left, TextStyle right)
        {
            return EqualityComparer<TextStyle>.Default.Equals(left, right);
        }

        public static bool operator !=(TextStyle left, TextStyle right)
        {
            return !(left == right);
        }
    }
}
