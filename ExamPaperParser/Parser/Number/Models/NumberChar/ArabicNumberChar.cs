using System;
using System.Collections.Generic;
using System.Text;

namespace ExamPaperParser.Parser.Number.Models.NumberChar
{
    /// <summary>
    /// 1, 2, 3 or
    /// １, ２, ３
    /// </summary>
    public class ArabicNumberChar : BaseNumberChar
    {
        public bool IsHalfWidth { get; set; }

        public ArabicNumberChar(bool isHalfWidth)
        {
            IsHalfWidth = isHalfWidth;
        }
    }
}
