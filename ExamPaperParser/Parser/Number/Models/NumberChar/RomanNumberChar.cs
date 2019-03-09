using System;
using System.Collections.Generic;
using System.Text;

namespace ExamPaperParser.Parser.Number.Models.NumberChar
{
    /// <summary>
    /// Ⅰ, Ⅱ, Ⅲ or
    /// I, II, III or
    /// ⅰ, ⅱ, ⅲ or
    /// i, ii, iii or
    /// </summary>
    public class RomanNumberChar : BaseNumberChar
    {
        public bool IsLowerCase { get; set; } 

        public RomanNumberChar(bool isLowerCase) 
        {
            IsLowerCase = isLowerCase;
        }

    }
}
