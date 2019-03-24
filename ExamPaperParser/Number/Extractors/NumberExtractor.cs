using ExamPaperParser.Base;
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
using FormattedFileParser.Models.Parts.Paragraphs.Style;
using FormattedFileParser.Models.Parts.Tables;
using FormattedFileParser.Models.Parts.Texts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ExamPaperParser.Number.Extractors
{
    public class NumberExtractor
    {
        private IDecoratedNumberParser _decoratedNumberParser;
        private NumberManager _numberManager = new NumberManager(new SimpleNumberDifferentiator());
        private Regex _spaceRegex = new Regex(@"(.*?)(\s+|$)", RegexOptions.Compiled);
        private Regex _titleBlackRegex;

        public NumberExtractor(
            IDecoratedNumberParser decoratedNumberParser,
            Regex titleBlackRegex)
        {
            _decoratedNumberParser = decoratedNumberParser;
            _titleBlackRegex = titleBlackRegex;
        }

        private string? ConsumeContent(ref IDataView data)
        {
            var m = _spaceRegex.Match(data.CurrentView.ToString());
            if (m.Success)
            {
                var res = data.CurrentView.Slice(0, m.Index + m.Length).ToString();
                data = data.CloneByDelta(m.Index + m.Length);
                return res;
            }

            return null;
        }

        private NumberNode? ConsumeNumberFromDataView(ref IDataView data, int paragraphOrder)
        {
            var backup = _numberManager.Save();

            foreach (var result in _decoratedNumberParser.Consume(data))
            {
                try
                {
                    var node = _numberManager.AddNumber(result.Result, paragraphOrder);
                    data = result.DataView;
                    return node;
                }
                catch (InvalidNumberException)
                {
                    _numberManager.Load(backup);
                }
            }

            return null;
        }

        private bool ExtractFromDataView(IDataView data, int paragraphOrder)
        {
            var content = new StringBuilder();

            var lastNode = ConsumeNumberFromDataView(ref data, paragraphOrder);
            if (lastNode == null)
            {
                return false;
            }

            while (!data.EndOfStream)
            {
                var currentContent = content.ToString();

                NumberNode node;
                if ((node = ConsumeNumberFromDataView(ref data, paragraphOrder)) != null)
                {
                    lastNode.Content = currentContent;
                    lastNode = node;
                    content.Clear();
                }
                else
                {
                    content.Append(ConsumeContent(ref data));
                }
            }

            lastNode.Content = content.ToString();
            return true;
        }

        private bool ExtractFromParagraph(ParagraphPart paragraph)
        {
            paragraph = paragraph.TrimStart();
            var content = paragraph.Content;
            var data = new StringDataView(content);

            return ExtractFromDataView(data, paragraph.Order);
        }

        private bool IsTitle(ParagraphPart paragraph, double maxTextSize)
        {
            if (!paragraph.Parts.Any())
            {
                return false;
            }

            // var isCenter = paragraph.Style.Justification == Justification.Center;
            var isBold = paragraph.Parts[0] is TextPart textPart && textPart.Style.IsBold == true;
            var isTextSizeLargeEnough = maxTextSize - paragraph.GetAverageTextSize() <= 4;

            return isBold && isTextSizeLargeEnough;
        }

        public IEnumerable<Tuple<string, NumberRoot>> Extract(ParsedFile file)
        {
            string lastTitle = string.Empty;
            _numberManager.Reset();

            var maxTextSize = file.GetMaxTextSize();

            foreach (var part in file.Parts)
            {
                switch (part)
                {
                    case ParagraphPart paragraph:
                        if (!ExtractFromParagraph(paragraph))
                        {
                            // If no number was parsed in paragraph, judge if it is a title
                            if (IsTitle(paragraph, maxTextSize))
                            {
                                if (_numberManager.Root.ChildDifferentiator != null
                                    && !_titleBlackRegex.IsMatch(lastTitle))
                                {
                                    yield return Tuple.Create(lastTitle, _numberManager.Root);
                                }

                                lastTitle = paragraph.Content.Trim();
                                var allowFirstNumbers = _numberManager.GetAllowFirstNumberForDifferentiators();
                                _numberManager.Reset();
                                _numberManager.SetAllowFirstNumberForDifferentatiators(allowFirstNumbers);
                            }
                        }
                        break;
                    case TablePart table:
                        break;
                }
            }

            if (_numberManager.Root.ChildDifferentiator != null
                && !_titleBlackRegex.IsMatch(lastTitle))
            {
                yield return Tuple.Create(lastTitle, _numberManager.Root);
            }
        }
    }
}
