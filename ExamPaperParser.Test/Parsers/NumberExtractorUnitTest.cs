using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExamPaperParser.Number.Extractors;
using ExamPaperParser.Number.Manager;
using ExamPaperParser.Number.Parsers.DecoratedNumberParsers;
using ExamPaperParser.Number.Parsers.NumberParsers;
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
        
        private void VisitNode(NumberNode node, int level)
        {
            var padding = string.Join("", Enumerable.Repeat("  ", level));
            _output.WriteLine($"{padding}- {node.DecoratedNumber.RawRepresentation}");

            foreach (var child in node.Children)
            {
                VisitNode(child, level + 1);
            }
        }

        private void VisitRoot(NumberRoot root)
        {
            foreach (var child in root.Children)
            {
                VisitNode(child, 0);
            }
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
                var root = extractor.Extract(res);

                VisitRoot(root);
            }
        }
    }
}
