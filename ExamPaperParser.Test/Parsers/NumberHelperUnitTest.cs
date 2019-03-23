using System;
using System.Collections.Generic;
using System.Text;
using ExamPaperParser.Order.Parsers.NumberParsers.Helpers;
using Xunit;

namespace ExamPaperParser.Test.Parsers
{
    public class NumberHelperUnitTest
    {
        [Fact]
        public void RomanNumberToInt()
        {
            Assert.Equal(10, RomanNumberHelper.RomanToInt("x"));
            Assert.Equal(11, RomanNumberHelper.RomanToInt("xi"));
            Assert.Equal(6, RomanNumberHelper.RomanToInt("vi"));
            Assert.Equal(12, RomanNumberHelper.RomanToInt("xii"));
            Assert.Equal(16, RomanNumberHelper.RomanToInt("xvi"));
        }

        [Fact]
        public void ChineseNumberToInt()
        {
            Assert.Equal(2_376_053, ChineseNumberHelper.ChineseNumberToInt("两百三十七万六千零五十三"));
            Assert.Equal(2_000_013, ChineseNumberHelper.ChineseNumberToInt("两百万零一十三"));
            Assert.Equal(1250, ChineseNumberHelper.ChineseNumberToInt("一千二百五"));
            Assert.Equal(1205, ChineseNumberHelper.ChineseNumberToInt("一千二百零五"));
            Assert.Equal(12000, ChineseNumberHelper.ChineseNumberToInt("一万二"));
            Assert.Equal(10002, ChineseNumberHelper.ChineseNumberToInt("一万零二"));
            Assert.Equal(10020, ChineseNumberHelper.ChineseNumberToInt("一万零二十"));
            Assert.Equal(10200, ChineseNumberHelper.ChineseNumberToInt("一万零二百"));
            Assert.Equal(13000, ChineseNumberHelper.ChineseNumberToInt("一万零三千"));
            Assert.Equal(10350, ChineseNumberHelper.ChineseNumberToInt("一万三百五"));
            Assert.Equal(1, ChineseNumberHelper.ChineseNumberToInt("一"));
            Assert.Equal(9, ChineseNumberHelper.ChineseNumberToInt("九"));
            Assert.Equal(10, ChineseNumberHelper.ChineseNumberToInt("十"));
            Assert.Equal(13, ChineseNumberHelper.ChineseNumberToInt("十三"));
            Assert.Equal(80, ChineseNumberHelper.ChineseNumberToInt("八十"));
            Assert.Equal(23, ChineseNumberHelper.ChineseNumberToInt("二十三"));
            Assert.Equal(181, ChineseNumberHelper.ChineseNumberToInt("一百八十一"));
            Assert.Equal(100, ChineseNumberHelper.ChineseNumberToInt("一百"));
            Assert.Equal(1000, ChineseNumberHelper.ChineseNumberToInt("一千"));
        }
    }
}
