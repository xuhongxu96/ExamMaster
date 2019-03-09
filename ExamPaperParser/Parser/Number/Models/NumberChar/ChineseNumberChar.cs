using System;
using System.Collections.Generic;
using System.Text;

namespace ExamPaperParser.Parser.Number.Models.NumberChar
{
    /// <summary>
    /// 一, 二, 三 or 
    /// 壹, 贰, 弎
    /// </summary>
    public class ChineseNumberChar : BaseNumberChar
    {
        public bool IsLowerCase { get; set; }

        public ChineseNumberChar(bool isLowerCase)
        {
            IsLowerCase = isLowerCase;
        }
    }
}
