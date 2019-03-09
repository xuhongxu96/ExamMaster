using FormattedFileParser.Parsers.Docx;
using System;
using Xunit;

namespace FormattedFileParser.Test
{
    public class FormattedFileParserUnitTest
    {
        [Fact]
        public void Parse()
        {
            var parser = new DocxParser("test1.docx");
            var res = parser.Parse();
        }
    }
}
