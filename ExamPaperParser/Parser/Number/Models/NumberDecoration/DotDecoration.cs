using System;
using System.Collections.Generic;
using System.Text;

namespace ExamPaperParser.Parser.Number.Models.NumberDecoration
{
    /// <summary>
    /// ⒈, ⒉, ⒊ or
    /// 1., 2., 3.
    /// </summary>
    public class DotDecoration : BaseNumberDecoration
    {
        public enum DotType
        {
            /// <summary>
            /// ⒈, ⒉, ⒊
            /// </summary>
            NumberWithFullStop,
            /// <summary>
            /// 1., 2., 3.
            /// </summary>
            NumberAndDot,
        }

        public DotType Type { get; set; }

        public DotDecoration(DotType type)
        {
            Type = type;
        }
    }
}
