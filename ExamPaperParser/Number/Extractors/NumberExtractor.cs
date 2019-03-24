using ExamPaperParser.DataView;
using ExamPaperParser.Helpers;
using ExamPaperParser.Number.Differentiators;
using ExamPaperParser.Number.Manager;
using ExamPaperParser.Number.Manager.Exceptions;
using ExamPaperParser.Number.Models;
using ExamPaperParser.Number.Models.DecoratedNumbers;
using ExamPaperParser.Number.Models.PaperNumbers;
using ExamPaperParser.Number.Parsers.DecoratedNumberParsers;
using FormattedFileParser.Models;
using FormattedFileParser.Models.Parts.Paragraphs;
using FormattedFileParser.Models.Parts.Tables;
using FormattedFileParser.Models.Parts.Texts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExamPaperParser.Number.Extractors
{
    public class NumberExtractor
    {
        private IDecoratedNumberParser _decoratedNumberParser;
        private NumberManager _numberManager = new NumberManager(new SimpleNumberDifferentiator());

        public NumberExtractor(IDecoratedNumberParser decoratedNumberParser)
        {
            _decoratedNumberParser = decoratedNumberParser;
        }

        private bool ExtractFromDataView(IDataView data, int paragraphOrder)
        {
            _numberManager.PushCurrent();

            foreach (var result in _decoratedNumberParser.Consume(data))
            {
                try
                {
                    var current = _numberManager.AddNumber(result.Result, paragraphOrder);
                    if (!ExtractFromDataView(result.DataView, paragraphOrder))
                    {
                        // No descendant
                        current.HasAdjacentNumber = true;
                    }
                    return true;
                }
                catch (InvalidNumberException)
                {
                    _numberManager.PopCurrent();
                }
            }
            return false;
        }

        private void ExtractFromParagraph(ParagraphPart paragraph)
        {
            paragraph = paragraph.TrimStart();
            var content = paragraph.Content;
            var data = new StringDataView(content);

            ExtractFromDataView(data, paragraph.Order);
        }

        public NumberRoot Extract(ParsedFile file)
        {
            foreach (var part in file.Parts)
            {
                switch (part)
                {
                    case ParagraphPart paragraph:
                        ExtractFromParagraph(paragraph);
                        break;
                    case TablePart table:
                        break;
                }
            }
            return _numberManager.Root;
        }
    }
}
