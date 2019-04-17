using System;
using System.Collections.Generic;
using System.Text;
using ExamPaperParser.Number.Models.DecoratedNumbers;
using ExamPaperParser.Number.Models.Numbers;

namespace ExamPaperParser.Number.Differentiators
{
    public class SimpleNumberDifferentiator : INumberDifferentiator
    {
        public string GetNumberDifferentiator(BaseNumber number)
        {
            switch (number)
            {
                case AlphabeticalNumber alphabeticalNumber:
                    if (alphabeticalNumber.IsLower)
                    {
                        return "a";
                    }
                    else
                    {
                        return "A";
                    }
                case ArabicNumber arabicNumber:
                    return "1";
                case ChineseNumber chineseNumber:
                    if (chineseNumber.IsLower)
                    {
                        return "一";
                    }
                    else
                    {
                        return "壹";
                    }
                case CircledNumber _:
                    return "CIRCLE 1";
                case FullStopNumber _:
                    return "1";
                case ParenthesizedNumber _:
                    return "(1)";
                case ParenthesizedAlphabeticalNumber parenthesizedAlphabeticalNumber:
                    return "(a)";
                case RomanNumber romanNumber:
                    if (romanNumber.IsLower)
                    {
                        return "i";
                    }
                    else
                    {
                        return "I";
                    }
                default:
                    return string.Empty;
            }
        }

        private Dictionary<string, string> _normalizedBracketMapping = new Dictionary<string, string>
        {
            { "（", "(" },
            { "）", ")" },
            { "【", "[" },
            { "】", "]" },
        };

        private Dictionary<string, string> _normalizedDelimiterMapping = new Dictionary<string, string>
        {
            { " ", "" },
            { "．", "" },
            { ".", "" },
            { "。", "" },
            { ",", "" },
            { "，", "" },
            { ":", "" },
            { "：", "" },
            { "、", "" },
            { "）", ")" },
            { "】", "]" },
        };

        public IReadOnlyCollection<string> AllowedDifferentiatorToSpanParents { get; } = new List<string>
        {
            "一",
            "1",
        };

        private string NormalizeBracket(string bracket)
        {
            if (_normalizedBracketMapping.TryGetValue(bracket, out var normBracket))
            {
                return normBracket;
            }

            return bracket;
        }

        private string NormalizeDelimiter(string delimiter)
        {
            if (_normalizedDelimiterMapping.TryGetValue(delimiter, out var normDelimiter))
            {
                return normDelimiter;
            }

            return delimiter;
        }

        public string GetDecoratedNumberDifferentiator(BaseDecoratedNumber number)
        {
            var numberDifferentiator = GetNumberDifferentiator(number.Number);

            switch (number)
            {
                case BracketDecoratedNumber bracketDecoratedNumber:
                    var left = NormalizeBracket(bracketDecoratedNumber.LeftBracket);
                    var right = NormalizeBracket(bracketDecoratedNumber.RightBracket);
                    return $"{left}{numberDifferentiator}{right}";
                case DelimiterDecoratedNumber delimiterDecoratedNumber:
                    var delimiter = NormalizeDelimiter(delimiterDecoratedNumber.Delimiter);
                    if (number.Number is ChineseNumber)
                    {
                        return $"{numberDifferentiator}";
                    }
                    return $"{numberDifferentiator}{delimiter}".ToUpper();
                case UndecoratedNumber undecoratedNumber:
                    return $"{numberDifferentiator}".ToUpper();
                default:
                    return string.Empty;
            }
        }
    }
}
