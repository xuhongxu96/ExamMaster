using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using ExamPaperParser.Number.Extractors;
using ExamPaperParser.Number.Manager;
using ExamPaperParser.Number.Models.NumberTree;
using ExamPaperParser.Number.Parsers.DecoratedNumberParsers;
using ExamPaperParser.Number.Parsers.NumberParsers;
using FormattedFileParser.Parsers.Docx;
using Xunit;
using Xunit.Abstractions;

namespace ExamPaperParser.Test.Number
{
    public class NumberExtractorUnitTest
    {
        private readonly ITestOutputHelper _output;

        private static readonly UniversalNumberParser _numberParser = new UniversalNumberParser();
        private static readonly UniversalDecoratedNumberParser _decoratedNumberParser = new UniversalDecoratedNumberParser(_numberParser);
        private static readonly NumberExtractor _extractor = new NumberExtractor(_decoratedNumberParser);

        public NumberExtractorUnitTest(ITestOutputHelper output)
        {
            _output = output;
        }

        private void VisitNode(NumberNode node, int level)
        {
            var padding = string.Join("", Enumerable.Repeat("  ", level));
            _output.WriteLine($"{padding}-- {node.DecoratedNumber.RawRepresentation} <{node.Score} 分>: {node.Header}");
            if (!string.IsNullOrWhiteSpace(node.Body))
            {
                _output.WriteLine($"\n{node.Body}\n");
            }

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

        private void ParseDocx(string path)
        {
            using (var docxParser = new DocxParser(path))
            {
                var doc = docxParser.Parse();
                var results = _extractor.Extract(doc).ToList();

                foreach (var result in results)
                {
                    foreach (var e in result.Item3)
                    {
                        _output.WriteLine($"{e.Message}\n{e.InnerException.Message}\n");
                    }

                    _output.WriteLine($"******{result.Item1}******");
                    VisitRoot(result.Item2);
                }
            }
        }

        [Fact]
        public void Test1()
        {
            ParseDocx("test1.docx");
        }

        [Fact]
        public void Test2()
        {
            ParseDocx("test2.docx");
        }

        [Fact]
        public void Test3()
        {
            ParseDocx("test3.docx");
        }
    }
}
