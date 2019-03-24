using System;
using System.Collections.Generic;
using System.Text;

namespace ExamPaperParser.Number.Parsers.NumberParsers.Helpers
{
    public static class ChineseNumberHelper
    {
        private static Dictionary<char, char> ChineseNumberReplacementMapping = new Dictionary<char, char>
        {
            { '两', '二' },
            { '壹', '一' },
            { '贰', '二' },
            { '叁', '三' },
            { '弎', '三' },
            { '肆', '四' },
            { '伍', '五' },
            { '陆', '六' },
            { '柒', '七' },
            { '捌', '八' },
            { '玖', '九' },
            { '拾', '十' },
            { '佰', '十' },
            { '仟', '十' },
            { '萬', '十' },
        };

        private static Dictionary<char, int> ChineseNumberMapping = new Dictionary<char, int>
        {
            { '零', 0 },
            { '一', 1 },
            { '二', 2 },
            { '三', 3 },
            { '四', 4 },
            { '五', 5 },
            { '六', 6 },
            { '七', 7 },
            { '八', 8 },
            { '九', 9 },
        };

        private static Dictionary<char, int> ChineseUnitMapping = new Dictionary<char, int>
        {
            { '十', 10 },
            { '百', 100 },
            { '千', 1_000 },
            { '万', 10_000 },
            { '亿', 100_000_000 },
        };

        public static int ChineseNumberToInt(string rawNumber)
        {
            foreach (var item in ChineseNumberReplacementMapping)
            {
                rawNumber = rawNumber.Replace(item.Key, item.Value);
            }

            var result = 0;
            var currentNum = 0;
            var lastUnit = int.MaxValue;
            bool hasZero = false;

            for (var i = 0; i < rawNumber.Length; ++i)
            {
                if (ChineseNumberMapping.TryGetValue(rawNumber[i], out var n))
                {
                    if (n == 0)
                    {
                        hasZero = true;
                    }
                    currentNum += n;
                }
                else if (ChineseUnitMapping.TryGetValue(rawNumber[i], out var unit))
                {
                    hasZero = false;

                    if (unit < lastUnit)
                    {
                        if (currentNum == 0)
                        {
                            result = unit;
                        }
                        else
                        {
                            result += currentNum * unit;
                        }
                    }
                    else
                    {
                        result += currentNum;
                        result *= unit;
                    }
                    currentNum = 0;
                    lastUnit = unit;
                }
            }

            if (hasZero)
            {
                result += currentNum;
            }
            else if (lastUnit == int.MaxValue)
            {
                result += currentNum;
            }
            else
            {
                result += currentNum * (lastUnit / 10);
            }

            return result;
        }
    }
}
