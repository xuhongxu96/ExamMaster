using System;
using System.Collections.Generic;
using System.Text;

namespace ExamPaperParser.Number.Models.Numbers
{
    public enum ChineseIdeaographType
    {
        /// <summary>
        /// 甲乙丙丁
        /// </summary>
        Traditional,
        /// <summary>
        /// 子丑寅卯
        /// </summary>
        Zodiac,
    }

    /// <summary>
    /// 甲乙丙丁, 
    /// 子丑寅卯
    /// </summary>
    public class ChineseIdeographNumber : BaseNumber
    {
        public ChineseIdeographNumber(ChineseIdeaographType type, string rawNumber, int number) : base(rawNumber, number)
        {
            IdeaographType = type;
        }

        public ChineseIdeaographType IdeaographType { get; }
    }
}
