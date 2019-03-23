using System;
using System.Collections.Generic;
using System.Text;

namespace ExamPaperParser.Order.Parsers.NumberParsers.Helpers
{
    public static class RomanNumberHelper
    {
        private static Dictionary<char, string> RomanSymbolMapping = new Dictionary<char, string>
        {
            { '\u2170', "i" },
            { '\u2171', "ii" },
            { '\u2172', "iii" },
            { '\u2173', "iv" },
            { '\u2174', "v" },
            { '\u2175', "vi" },
            { '\u2176', "vii" },
            { '\u2177', "viii" },
            { '\u2178', "ix" },
            { '\u2179', "x" },
            { '\u217A', "xi" },
            { '\u217B', "xii" },
            { '\u217C', "l" },
            { '\u217D', "c" },
            { '\u217E', "d" },
            { '\u217F', "m" },
        };

        public static string RomanToAscii(string roman)
        {
            var result = new StringBuilder();
            foreach (var ch in roman)
            {
                if (ch >= '\u2170' && ch <= '\u217F')
                {
                    result.Append(RomanSymbolMapping[ch]);
                }
                else if (ch >= '\u2160' && ch <= '\u216F')
                {
                    result.Append(RomanSymbolMapping[(char)(ch + 0xF)]);
                }
                else
                {
                    result.Append(ch);
                }
            }

            return result.ToString();
        }

        private static Dictionary<char, int> RomanCharNumberMapping = new Dictionary<char, int>
        {
            { 'i', 1 },
            { 'v', 5 },
            { 'x', 10 },
            { 'l', 50 },
            { 'c', 100 },
            { 'd', 500 },
            { 'm', 1000 },
        };

        public static int RomanToInt(string roman, bool convertToAscii = true)
        {
            if (convertToAscii)
                roman = RomanToAscii(roman);

            if (roman.Length == 0) return 0;
            roman = roman.ToLower();

            int total = 0;
            int last_value = 0;
            for (int i = roman.Length - 1; i >= 0; i--)
            {
                int new_value = RomanCharNumberMapping[roman[i]];

                // See if we should add or subtract.
                if (new_value < last_value)
                    total -= new_value;
                else
                {
                    total += new_value;
                    last_value = new_value;
                }
            }

            return total;
        }
    }
}
