using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using FormattedFileParser.Models;
using FormattedFileParser.Models.Parts;
using FormattedFileParser.Models.Parts.Paragraphs;
using FormattedFileParser.Models.Parts.Paragraphs.Style;
using FormattedFileParser.Models.Parts.Tables;
using FormattedFileParser.Parsers.Docx.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace FormattedFileParser.Parsers.Docx
{
    public class DocxParser : IDisposable
    {
        private readonly WordprocessingDocument _wordDocument;
        private readonly Document _document;

        public DocxParser(string path, bool isEditable = false)
        {
            _wordDocument = WordprocessingDocument.Open(path, isEditable);
            _document = _wordDocument.MainDocumentPart.Document;
        }

        private static TablePart ParseTable(int i, Table table)
        {
            return new TablePart();
        }

        private static IEnumerable<IMainPart> ParseDocument(Document document)
        {
            var i = 0;
            foreach (var element in document.Body.ChildElements)
            {
                if (element is Paragraph paragraph)
                {
                    yield return ParagraphParseHelper.ParseParagraph(i++, paragraph);
                }
                else if (element is Table table)
                {
                    yield return ParseTable(i++, table);
                }
            }
        }

        public ParsedFile Parse()
        {
            return new ParsedFile(ParseDocument(_document));
        }

        public void Dispose()
        {
            _wordDocument.Dispose();
        }
    }
}
