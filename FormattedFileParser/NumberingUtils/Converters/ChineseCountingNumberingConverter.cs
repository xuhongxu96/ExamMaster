using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FormattedFileParser.Models.Parts.Paragraphs.Style;

namespace FormattedFileParser.NumberingUtils.Converters
{
    public class ChineseCountingNumberingConverter : INumberingConverter
    {
        private static readonly string[] ChineseDigits = new string[]
        {
            "零", "一", "二", "三", "四", "五", "六", "七", "八", "九", "十",
        };

        private struct Unit
        {
            public int Factor;
            public string Text;

            public Unit(int factor, string text)
            {
                Factor = factor;
                Text = text;
            }
        }

        private static readonly List<Unit> ChineseUnits = new List<Unit>
        {
            new Unit(1, ""),
            new Unit(10, "十"),
            new Unit(10, "百"),
            new Unit(10, "千"),
            new Unit(10, "万"),
            new Unit(10000, "亿"),
        };

        public NumberingStyle Style => NumberingStyle.ChineseCounting;

        private static List<int> GetNumberForUnit(int number)
        {
            var numberPerUnit = new List<int>();
            var tempNumber = number;

            foreach (var unit in ChineseUnits.Skip(1))
            {
                numberPerUnit.Add(tempNumber % unit.Factor);
                tempNumber /= unit.Factor;

                if (tempNumber == 0)
                {
                    break;
                }
            }

            if (tempNumber != 0)
            {
                numberPerUnit.Add(tempNumber);
            }

            return numberPerUnit;
        }

        public string Convert(int number)
        {
            if (number <= 10)
            {
                return ChineseDigits[number];
            }
            else if (number < 20)
            {
                return ChineseDigits[10] + ChineseDigits[number % 10];
            }

            var numberPerUnit = GetNumberForUnit(number);

            var needZero = false;
            var sb = new StringBuilder();
            for (var i = numberPerUnit.Count - 1; i >= 0; --i)
            {
                var n = numberPerUnit[i];
                var chineseN = Convert(n);

                var unit = ChineseUnits[i];

                if (i < numberPerUnit.Count - 1 && i > 0)
                {
                    var nextUnit = ChineseUnits[i + 1];
                    if (n < nextUnit.Factor / unit.Factor)
                    {
                        needZero = true;
                    }
                }

                if (n != 0)
                {
                    if (needZero)
                    {
                        sb.Append(ChineseDigits[0]);
                        needZero = false;
                    }
                    sb.Append(chineseN);
                    sb.Append(unit.Text);
                }
            }

            return sb.ToString();
        }
    }
}
