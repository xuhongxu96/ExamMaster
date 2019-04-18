using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using FormattedFileParser.Exceptions;
using FormattedFileParser.Models;
using FormattedFileParser.Models.Parts;
using FormattedFileParser.Models.Parts.Paragraphs;
using FormattedFileParser.Models.Parts.Paragraphs.Style;
using FormattedFileParser.Models.Parts.Tables;
using FormattedFileParser.NumberingUtils.Managers;
using FormattedFileParser.Parsers.Docx.InternalParsers;
using FormattedFileParser.Parsers.Docx.Managers;
using FormattedFileParser.Processors;
using System;
using System.Collections.Generic;
using System.Text;

namespace FormattedFileParser.Parsers.Docx
{
    public class DocxParser : IDisposable
    {
        private readonly WordprocessingDocument _wordDocument;
        private readonly Document _document;

        private readonly ParagraphParser _paragraphParser;
        private readonly RunParser _runParser;

        private DocxNumberingManager _numberingManager = new DocxNumberingManager();

        private readonly List<IProcessor> _processors = new List<IProcessor>();

        public DocxParser(string path, bool isEditable = false)
            : this(path, new List<IProcessor>(), isEditable)
        {
        }

        public DocxParser(string path, IEnumerable<IProcessor> processors, bool isEditable = false)
        {
            _wordDocument = WordprocessingDocument.Open(path, isEditable);
            _document = _wordDocument.MainDocumentPart.Document;

            _paragraphParser = new ParagraphParser(_wordDocument.MainDocumentPart, _numberingManager);
            _runParser = new RunParser();

            _processors.AddRange(processors);
        }

        private TablePart ParseTable(int i, Table table)
        {
            return new TablePart();
        }

        private IEnumerable<IMainPart> ParseDocument(Document document)
        {
            var i = 0;
            foreach (var element in document.Body.ChildElements)
            {
                if (element is Paragraph paragraph)
                {
                    yield return _paragraphParser.ParseParagraph(i++, paragraph);
                }
                else if (element is Table table)
                {
                    yield return ParseTable(i++, table);
                }
            }
        }

        public ParsedFile Parse(out IList<ParagraphFormatException> exceptions)
        {
            var result = new ParsedFile(ParseDocument(_document), _numberingManager);
            exceptions = new List<ParagraphFormatException>();
            foreach (var processor in _processors)
            {
                foreach (var e in processor.Process(result))
                {
                    exceptions.Add(e);
                }
            }

            return result;
        }

        public void Dispose()
        {
            _wordDocument.Dispose();
        }
    }
}
