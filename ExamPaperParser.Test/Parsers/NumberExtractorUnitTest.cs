using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExamPaperParser.Order.Extractors;
using ExamPaperParser.Order.Parsers.DecoratedNumberParsers;
using ExamPaperParser.Order.Parsers.NumberParsers;
using FormattedFileParser.Parsers.Docx;
using Xunit;
using Xunit.Abstractions;

namespace ExamPaperParser.Test.Parsers
{
    public class NumberExtractorUnitTest
    {
        private readonly ITestOutputHelper _output;

        public NumberExtractorUnitTest(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void Test()
        {
            var numberParser = new UniversalNumberParser();
            var parser = new UniversalDecoratedNumberParser(numberParser);
            var extractor = new NumberExtractor(parser);

            using (var docxParser = new DocxParser("test1.docx"))
            {
                var res = docxParser.Parse();

                foreach (var number in extractor.Extract(res))
                {
                    _output.WriteLine(number);
                    _output.WriteLine("---");
                }
            }
        }
    }
}
