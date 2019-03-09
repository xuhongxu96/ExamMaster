using System;
using System.Collections.Generic;
using System.Text;

namespace ExamPaperParser.Parser.Number.Models.NumberDecoration
{
    public class BracketDecoration
    {
        public enum BracketType
        {
            None,
            /// <summary>
            /// ()
            /// </summary>
            Parenthese,
            /// <summary>
            /// （）
            /// </summary>
            ParentheseFullWidth,
            /// <summary>
            /// []
            /// </summary>
            SquareBracket,
            /// <summary>
            /// 【】
            /// </summary>
            SquareBracketFullWidth,
            /// <summary>
            /// ⟨⟩
            /// </summary>
            AngleBracket,
            /// <summary>
            /// &lt;&gt;
            /// </summary>
            AngleBracketInequalitySign,
            /// <summary>
            /// {}
            /// </summary>
            CurlyBracket,
        }

        public BracketType Left { get; set; }
        public BracketType Right { get; set; }

        public BracketDecoration(BracketType left, BracketType right)
        {
            Left = left;
            Right = right;
        }
    }
}
