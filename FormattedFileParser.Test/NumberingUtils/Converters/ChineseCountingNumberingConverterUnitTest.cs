using System;
using System.Collections.Generic;
using System.Text;
using FormattedFileParser.NumberingUtils.Converters;
using Xunit;

namespace FormattedFileParser.Test.NumberingUtils.Converters
{
    public class ChineseCountingNumberingConverterUnitTest
    {
        [Fact]
        public void Convert()
        {
            var converter = new ChineseCountingNumberingConverter();
            Assert.Equal("十三亿二千一百三十三万零二十一", converter.Convert(1321330021));
            Assert.Equal("二百一十万三千九百九十", converter.Convert(2103990));
            Assert.Equal("一万二千零三", converter.Convert(12003));
            Assert.Equal("二", converter.Convert(2));
            Assert.Equal("十", converter.Convert(10));
            Assert.Equal("十一", converter.Convert(11));
            Assert.Equal("一百", converter.Convert(100));
            Assert.Equal("一百零三", converter.Convert(103));
            Assert.Equal("一千零一", converter.Convert(1001));
            Assert.Equal("一千零三十", converter.Convert(1030));
            Assert.Equal("一千二百零一", converter.Convert(1201));
            Assert.Equal("一千二百", converter.Convert(1200));
            Assert.Equal("一千三百二十一", converter.Convert(1321));
            Assert.Equal("一万零一百零一", converter.Convert(10101));
            Assert.Equal("一万一千零一", converter.Convert(11001));
            Assert.Equal("九万零二十一", converter.Convert(90021));
            Assert.Equal("二百一十九万零二十一", converter.Convert(2190021));
            Assert.Equal("二十三万九千八百八十三", converter.Convert(239883));
        }
    }
}
