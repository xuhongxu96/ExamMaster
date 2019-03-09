using ExamPaperParser.Parser.Number.Models.NumberChar;
using ExamPaperParser.Parser.Number.Models.NumberDecoration;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExamPaperParser.Parser.Number.Models
{
    public struct NumberStyle
    {
        public BaseNumberChar CharType { get; set; }
        public BaseNumberDecoration Decoration { get; set; }
    }
}
