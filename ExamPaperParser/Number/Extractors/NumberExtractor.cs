using ExamPaperParser.Base;
using ExamPaperParser.DataView;
using ExamPaperParser.Helpers;
using ExamPaperParser.Number.Differentiators;
using ExamPaperParser.Number.Manager;
using ExamPaperParser.Number.Manager.Exceptions;
using ExamPaperParser.Number.Models;
using ExamPaperParser.Number.Models.DecoratedNumbers;
using ExamPaperParser.Number.Models.NumberTree;
using ExamPaperParser.Number.Models.PaperNumbers;
using ExamPaperParser.Number.Parsers.DecoratedNumberParsers;
using ExamPaperParser.Number.Postprocessors;
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
    public class NumberExtractor : INumberExtractor
    {
        private Regex _spaceRegex = new Regex(@"(.*?)(\s+|$)", RegexOptions.Compiled);

        private Regex _blackParagraphRegex 
            = new Regex(@"(^【.*?(答案|解析|点评|试题).*?】|中小学教育网|转载.*?注明出处)"
                + @"|(准考证号|答题卡|橡皮擦|本试题|本试卷|本卷)", RegexOptions.Compiled);

        private Regex _titleBlackRegex = new Regex(@"答案", RegexOptions.Compiled);

        private IDecoratedNumberParser _decoratedNumberParser;
        private INumberManager _numberManager = new NumberManager(new SimpleNumberDifferentiator());
        private IEnumerable<IPostprocessor> _postprocessors;

        public NumberExtractor(IDecoratedNumberParser decoratedNumberParser)
        {
            _decoratedNumberParser = decoratedNumberParser;
            _postprocessors = new List<IPostprocessor>
            {
                new ChoiceQuestionPostprocessor(),
                new QuestionScorePostprocessor(),
                new RemoveArticleNumberPostprocessor(),
            };
        }

        public NumberExtractor(
            IDecoratedNumberParser decoratedNumberParser,
            Regex titleBlackRegex,
            Regex blackParagraphRegex,
            IEnumerable<IPostprocessor> postprocessors)
        {
            _decoratedNumberParser = decoratedNumberParser;
            _titleBlackRegex = titleBlackRegex;
            _blackParagraphRegex = blackParagraphRegex;
            _postprocessors = postprocessors;
        }

        private string? ConsumeContentToSpaceOrEnd(ref IDataView data)
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
            InvalidNumberException? exception = null;

            foreach (var result in _decoratedNumberParser.Consume(data))
            {
                if (result.Result.Number.IntNumber > 50)
                {
                    continue;
                }

                try
                {
                    var node = _numberManager.AddNumber(result.Result, paragraphOrder);
                    data = result.DataView;
                    exception = null;
                    return node;
                }
                catch (InvalidNumberException e)
                {
                    exception = e;
                    _numberManager.Load(backup);
                }
            }

            if (exception != null)
            {
                throw new FormatException($"{paragraphOrder}: {data.CurrentView.ToString()}", exception);
            }

            return null;
        }

        private NumberNode? ExtractFromDataView(IDataView data, int paragraphOrder)
        {
            var content = new StringBuilder();

            NumberNode lastNode;
            try
            {
                lastNode = ConsumeNumberFromDataView(ref data, paragraphOrder);
                if (lastNode == null)
                {
                    return null;
                }
            }
            catch (FormatException)
            {
                throw;
            }

            while (!data.EndOfStream)
            {
                var currentContent = content.ToString();

                NumberNode? node = null;
                try
                {
                    node = ConsumeNumberFromDataView(ref data, paragraphOrder);
                }
                catch (FormatException)
                { }

                if (node != null)
                {
                    lastNode.Header = currentContent.Trim();
                    lastNode = node;
                    content.Clear();
                }
                else
                {
                    content.Append(ConsumeContentToSpaceOrEnd(ref data));
                }
            }

            lastNode.Header = content.ToString().Trim();
            return lastNode;
        }

        private NumberNode? ExtractFromParagraph(ParagraphPart paragraph)
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

            var isCenter = paragraph.Style.Justification == Justification.Center
                || paragraph.Content.Length - paragraph.Content.TrimStart().Length > 2;
            var isBold = paragraph.Parts[0] is TextPart textPart && textPart.Style.IsBold == true;
            var isTextSizeLargeEnough = maxTextSize - paragraph.GetAverageTextSize() <= 4;

            return (isCenter && isBold && isTextSizeLargeEnough)
                || (paragraph.Content.Contains("答案") && (isCenter || isBold))
                || (isCenter
                    && paragraph.Content.Trim().StartsWith("第")
                    && paragraph.Content.Trim().Replace(" ", "").Length < 5);
        }

        private void Postprocess(NumberRoot root)
        {
            foreach (var processor in _postprocessors)
            {
                processor.Process(root);
            }
        }

        public IEnumerable<Tuple<string, NumberRoot, List<FormatException>>> Extract(ParsedFile file)
        {
            NumberNode? lastNode = null;
            string lastTitle = string.Empty;
            Dictionary<string, int>? lastAllowFirstNumbers = null;
            var exceptions = new List<FormatException>();
            _numberManager.Reset();

            var maxTextSize = file.GetMaxTextSize();

            var skipToNextTitle = false;
            var skipAppendingToBody = false;

            foreach (var part in file.Parts)
            {
                switch (part)
                {
                    case ParagraphPart paragraph:
                        if (_blackParagraphRegex.IsMatch(paragraph.Content.Trim()))
                        {
                            skipAppendingToBody = true;
                            continue;
                        }

                        var isTitle = IsTitle(paragraph, maxTextSize);
                        if (isTitle)
                        {
                            skipToNextTitle = false;
                        }

                        if (skipToNextTitle)
                        {
                            continue;
                        }

                        NumberNode? currentNode;
                        try
                        {
                            currentNode = ExtractFromParagraph(paragraph);
                        }
                        catch (FormatException e)
                        {
                            if (!skipAppendingToBody)
                            {
                                exceptions.Add(e);
                            }

                            currentNode = null;
                        }

                        if (currentNode != null)
                        {
                            skipAppendingToBody = false;
                            lastNode = currentNode;
                        }
                        else
                        {
                            // If no number was parsed in paragraph, judge if it is a title
                            if (IsTitle(paragraph, maxTextSize))
                            {
                                skipAppendingToBody = false;

                                if (_titleBlackRegex.IsMatch(paragraph.Content.Trim()))
                                {
                                    skipToNextTitle = true;
                                    continue;
                                }

                                if (_numberManager.Root.ChildDifferentiator != null)
                                {
                                    Postprocess(_numberManager.Root);
                                    lastAllowFirstNumbers = _numberManager.GetAllowFirstNumberForDifferentiators();
                                    yield return Tuple.Create(lastTitle, _numberManager.Root, exceptions.ToList());
                                }

                                lastTitle = paragraph.Content.Trim();

                                exceptions.Clear();
                                _numberManager.Reset();
                                if (lastAllowFirstNumbers != null)
                                {
                                    _numberManager.SetAllowFirstNumberForDifferentatiators(lastAllowFirstNumbers);
                                }
                            }
                            else if (lastNode != null)
                            {
                                // If it's not title either, append it to node content
                                if (!skipAppendingToBody)
                                {
                                    lastNode.Body = $"{lastNode.Body}\n{paragraph.Content}".Trim();
                                }
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
                Postprocess(_numberManager.Root);
                yield return Tuple.Create(lastTitle, _numberManager.Root, exceptions.ToList());
            }
        }
    }
}
