using ExamPaperParser.Base;
using ExamPaperParser.DataView;
using ExamPaperParser.Helpers;
using ExamPaperParser.Number.Differentiators;
using ExamPaperParser.Number.Extractors.Exceptions;
using ExamPaperParser.Number.Manager;
using ExamPaperParser.Number.Manager.Exceptions;
using ExamPaperParser.Number.Models;
using ExamPaperParser.Number.Models.DecoratedNumbers;
using ExamPaperParser.Number.Models.NumberTree;
using ExamPaperParser.Number.Parsers.DecoratedNumberParsers;
using ExamPaperParser.Number.Postprocessors;
using FormattedFileParser.Exceptions;
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
        private Regex _spaceRegex = new Regex(@"(.*?)([^(（]\s+(?![\d\.]+\s*分)*|$)", RegexOptions.Compiled);

        private Regex _blackParagraphRegex
            = new Regex(@"(^【.*?(答案|解析|点评|试题).*?】|中小学教育网|转载.*?注明出处)"
                + @"|(准考证号|答题卡|答题纸|橡皮擦|本试题|本试卷|本卷)", RegexOptions.Compiled);

        private Regex _titleBlackRegex = new Regex(@"答案", RegexOptions.Compiled);

        private Regex _poemQuestionRegex = new Regex(@"读[^字章（）()]*?诗|诗[^字章()（）]*?读", RegexOptions.Compiled);

        private Regex _simpleScoreRegex = new Regex(@"^\s*[\d.]+\s*分", RegexOptions.Compiled);

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
                new RemoveCommentNumberPostprocessor(),
                new SelectiveQuestionPostprocessor(),
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
                catch (ReadingParagraphNumberException)
                {
                    _numberManager.Load(backup);
                }
                catch (InvalidNumberException e)
                {
                    exception = e;
                    _numberManager.Load(backup);
                }
            }

            if (exception != null)
            {
                var p = _numberManager.Current;
                if (p == null)
                {
                    throw new NumberException(exception.Message, $"Paragraph {paragraphOrder}", data.CurrentView.ToString());
                }

                var chain = NumberNodeHelper.GetNumberChain(p);
                throw new NumberException(exception.Message, string.Join(" ", chain), data.CurrentView.ToString());
            }

            return null;
        }

        private NumberNode? ExtractFromDataView(IDataView data, int paragraphOrder)
        {
            var content = new StringBuilder();

            var lastNode = ConsumeNumberFromDataView(ref data, paragraphOrder);
            if (lastNode == null)
            {
                return null;
            }

            while (!data.EndOfStream)
            {
                // Skip Score
                var scoreMatch = _simpleScoreRegex.Match(data.CurrentView.ToString());
                if (scoreMatch.Success)
                {
                    content.Append(scoreMatch.Value);
                    data = data.CloneByDelta(scoreMatch.Length);
                }

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

        private bool IsTitle(string lastQuestionHeader, ParagraphPart paragraph, double maxTextSize, double avgTextSize)
        {
            if (!paragraph.Parts.Any())
            {
                return false;
            }

            var isUnderlined = paragraph.Parts[0] is TextPart textPart2 && textPart2.Style.IsUnderlined == true;
            var isRealCenter = paragraph.Style.Justification == Justification.Center;
            var isCenter = isRealCenter
                || (!isUnderlined && paragraph.Content.Length - paragraph.Content.TrimStart().Length > 2);
            var isBold = paragraph.Parts[0] is TextPart textPart && textPart.Style.IsBold == true;
            var isTextSizeLargeEnough = maxTextSize - paragraph.GetAverageTextSize() <= 4;
            var isTextLargerThanAvg = paragraph.GetAverageTextSize() > avgTextSize;

            var sentences = paragraph.Content.Trim().Split(new char[] { ',', '，', '。', '.' }, StringSplitOptions.RemoveEmptyEntries);
            var isPoem = (sentences.Length == 2 && sentences[0].Length == sentences[1].Length)
                || _poemQuestionRegex.IsMatch(lastQuestionHeader);
            if (isPoem) return false;

            return ((isRealCenter || isBold) && isTextSizeLargeEnough && isTextLargerThanAvg)
                || (isCenter && isBold && isTextSizeLargeEnough)
                || (paragraph.Content.Contains("答案") && (isCenter || isBold))
                || (isCenter
                    && paragraph.Content.Trim().StartsWith("第")
                    && paragraph.Content.Trim().Replace(" ", "").Length < 5);
        }

        private List<ParagraphFormatException> Postprocess(NumberRoot root)
        {
            var exceptions = new List<ParagraphFormatException>();
            foreach (var processor in _postprocessors)
            {
                exceptions.AddRange(processor.Process(root));
            }

            return exceptions;
        }

        private IEnumerable<Tuple<string, NumberRoot, List<ParagraphFormatException>>> ExtractInternal(ParsedFile file)
        {
            NumberNode? lastNode = null;
            string lastTitle = string.Empty;
            Dictionary<string, int>? lastAllowFirstNumbers = null;
            var exceptions = new List<ParagraphFormatException>();
            _numberManager.Reset();

            var maxTextSize = file.GetMaxTextSize();
            var avgTextSize = file.GetAvgTextSize();

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

                        var isTitle = IsTitle(_numberManager.Current?.Header ?? "", paragraph, maxTextSize, avgTextSize);
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
                        catch (ParagraphFormatException e)
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
                            if (isTitle)
                            {
                                skipAppendingToBody = false;

                                if (_titleBlackRegex.IsMatch(paragraph.Content.Trim()))
                                {
                                    skipToNextTitle = true;
                                    continue;
                                }

                                if (_numberManager.Root.ChildDifferentiator != null)
                                {
                                    exceptions.AddRange(Postprocess(_numberManager.Root));
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
                exceptions.AddRange(Postprocess(_numberManager.Root));
                yield return Tuple.Create(lastTitle, _numberManager.Root, exceptions.ToList());
            }
        }

        public IEnumerable<Tuple<string, NumberRoot, List<ParagraphFormatException>>> Extract(ParsedFile file)
        {
            var result = ExtractInternal(file).ToArray();
            if (result.Length == 0)
            {
                yield return new Tuple<string, NumberRoot, List<ParagraphFormatException>>("错误", new NumberRoot(), new List<ParagraphFormatException>
                {
                    new SevereException("什么都没解析出来"),
                });
            }

            for (var i = 0; i < result.Length; ++i)
            {
                var item = result[i];

                if (i == result.Length - 1)
                {
                    var maxN = NumberNodeHelper.GetMaxNumber(item.Item2);
                    if (maxN < 15 || maxN > 30)
                    {
                        item.Item3.Add(
                            new SevereException($"最后的最大题号是{maxN}, 不正常（应在15-30间）"));
                    }
                }

                yield return item;
            }
        }
    }
}
