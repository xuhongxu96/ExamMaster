using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExamPaperParser.Base;
using ExamPaperParser.DataView;
using ExamPaperParser.Number.Models.Numbers;

namespace ExamPaperParser.Number.Parsers.NumberParsers
{
    /// <summary>
    /// 甲乙丙丁, 
    /// 子丑寅卯
    /// </summary>
    public class ChineseIdeographNumberParser : BaseNumberParser
    {
        // ideographTraditional
        private static readonly char[] TraditionalChars = { '甲', '乙', '丙', '丁', '戊', '己', '庚', '辛', '壬', '癸' };

        private static readonly Dictionary<char, int> TraditionalCharToInt = new Dictionary<char, int>();

        // ideographZodiac
        private static readonly char[] ZodiacChars = { '子', '丑', '寅', '卯', '辰', '巳', '午', '未', '申', '酉', '戌', '亥' };

        private static readonly Dictionary<char, int> ZodiacCharToInt = new Dictionary<char, int>();

        private readonly bool _willConsumeZodiac;
        private readonly int _maxConsumeTraditionalCharNumber;

        static ChineseIdeographNumberParser()
        {
            for (var i = 0; i < TraditionalChars.Length; ++i)
            {
                TraditionalCharToInt[TraditionalChars[i]] = i + 1;
            }

            for (var i = 0; i < ZodiacChars.Length; ++i)
            {
                ZodiacCharToInt[ZodiacChars[i]] = i + 1;
            }
        }

        public ChineseIdeographNumberParser(bool consumeFromStart = true, int maxConsumeTraditionalCharNumber = 4, bool consumeZodiac = false)
            : base(consumeFromStart)
        {
            _willConsumeZodiac = consumeZodiac;
            _maxConsumeTraditionalCharNumber = maxConsumeTraditionalCharNumber;
            if (_maxConsumeTraditionalCharNumber > 10)
            {
                _maxConsumeTraditionalCharNumber = 10;
            }
        }

        protected override string MatchRegex => string.Join("|", TraditionalChars.Concat(ZodiacChars));

        protected override BaseNumber ConstructNumber(string rawNumber, int number)
        {
            var type = ChineseIdeaographType.Zodiac;
            if (TraditionalCharToInt.ContainsKey(rawNumber[0]))
            {
                type = ChineseIdeaographType.Traditional;
            }

            return new ChineseIdeographNumber(type, rawNumber, number);
        }

        protected override int? ParseRawNumber(string rawNumber)
        {
            if (TraditionalCharToInt.TryGetValue(rawNumber[0], out var n)
                && n <= _maxConsumeTraditionalCharNumber)
            {
                return n;
            }

            if (_willConsumeZodiac)
            {
                return ZodiacCharToInt[rawNumber[0]];
            }

            return null;
        }
    }
}
