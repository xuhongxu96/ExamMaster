using System;
using System.Collections.Generic;
using System.Text;

namespace ExamPaperParser.Parser.Number.Models
{
    public struct LeveledNumber
    {
        public int Number { get; set; }

        public NumberStyle Style { get; set; }
    }
}
