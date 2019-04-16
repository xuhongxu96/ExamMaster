using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using FormattedFileParser.Models.Parts;
using FormattedFileParser.Models.Parts.Paragraphs;
using FormattedFileParser.Models.Parts.Paragraphs.Style;
using FormattedFileParser.Models.Parts.Texts;
using FormattedFileParser.NumberingUtils.Allocators;
using FormattedFileParser.NumberingUtils.Managers;
using FormattedFileParser.Parsers.Docx.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FormattedFileParser.Parsers.Docx.InternalParsers
{
    public class ParagraphParser
    {
        private readonly RunParser _runParser = new RunParser();
        private readonly DocxNumberingManager _numberingManager;

        private readonly MainDocumentPart _mainDocumentPart;
        private readonly NumberingDefinitionsPart _numberingDefinitionsPart;

        public ParagraphParser(
            MainDocumentPart mainDocumentPart,
            DocxNumberingManager numberingManager)
        {
            _mainDocumentPart = mainDocumentPart;
            _numberingDefinitionsPart = _mainDocumentPart.NumberingDefinitionsPart;

            _numberingManager = numberingManager;

            PrepareNumbering();
        }

        public Models.Parts.Paragraphs.Style.Indentation ParseIndentation(DocumentFormat.OpenXml.Wordprocessing.Indentation indent)
        {
            var result = new Models.Parts.Paragraphs.Style.Indentation();

            if (indent == null)
            {
                return result;
            }

            if (indent.Left != null && indent.Left.HasValue)
            {
                result.Left = Convert.ToInt32(double.Parse(indent.Left.Value) / 210.0);
            }
            if (indent.LeftChars != null && indent.LeftChars.HasValue)
            {
                result.Left = indent.LeftChars.Value;
            }

            if (indent.Right != null && indent.Right.HasValue)
            {
                result.Right = Convert.ToInt32(double.Parse(indent.Right.Value) / 210.0);
            }
            if (indent.RightChars != null && indent.LeftChars.HasValue)
            {
                result.Right = indent.RightChars.Value;
            }

            if (indent.FirstLine != null && indent.FirstLine.HasValue)
            {
                result.FirstLine = Convert.ToInt32(double.Parse(indent.FirstLine.Value) / 210.0);
            }
            if (indent.FirstLineChars != null && indent.FirstLineChars.HasValue)
            {
                result.FirstLine = indent.FirstLineChars.Value;
            }

            return result;
        }

        public Models.Parts.Paragraphs.Style.Justification ParseJustification(DocumentFormat.OpenXml.Wordprocessing.Justification justification)
        {
            if (justification == null || justification.Val == null || !justification.Val.HasValue)
            {
                return Models.Parts.Paragraphs.Style.Justification.Justified;
            }

            switch (justification.Val.Value)
            {
                case JustificationValues.Both:
                    return Models.Parts.Paragraphs.Style.Justification.Justified;
                case JustificationValues.Center:
                    return Models.Parts.Paragraphs.Style.Justification.Center;
                case JustificationValues.Left:
                    return Models.Parts.Paragraphs.Style.Justification.Left;
                case JustificationValues.Right:
                    return Models.Parts.Paragraphs.Style.Justification.Right;
                default:
                    return Models.Parts.Paragraphs.Style.Justification.Other;
            }
        }

        public NumberingStyle ParseNumberingFormat(NumberingFormat numberingFormat)
        {
            if (numberingFormat?.Val?.HasValue == true)
            {
                switch (numberingFormat.Val.Value)
                {
                    case NumberFormatValues.ChineseCounting:
                    case NumberFormatValues.JapaneseCounting:
                        return NumberingStyle.ChineseCounting;
                    case NumberFormatValues.Decimal:
                    case NumberFormatValues.DecimalHalfWidth:
                        return NumberingStyle.Decimal;
                    case NumberFormatValues.DecimalEnclosedCircle:
                        return NumberingStyle.DecimalEnclosedCircle;
                    case NumberFormatValues.DecimalEnclosedFullstop:
                        return NumberingStyle.DecimalEnclosedFullstop;
                    case NumberFormatValues.DecimalEnclosedParen:
                        return NumberingStyle.DecimalEnclosedParen;
                    case NumberFormatValues.DecimalZero:
                        return NumberingStyle.DecimalZero;
                    case NumberFormatValues.DecimalFullWidth:
                    case NumberFormatValues.DecimalFullWidth2:
                        return NumberingStyle.DecimalFullWidth;
                    case NumberFormatValues.UpperLetter:
                        return NumberingStyle.UpperLetter;
                    case NumberFormatValues.UpperRoman:
                        return NumberingStyle.UpperRoman;
                    case NumberFormatValues.LowerLetter:
                        return NumberingStyle.LowerLetter;
                    case NumberFormatValues.LowerRoman:
                        return NumberingStyle.LowerRoman;
                    default:
                        return NumberingStyle.Other;
                }
            }

            return NumberingStyle.Decimal;
        }

        private string ParseLevelSuffix(LevelSuffix suffix)
        {
            if (suffix?.Val?.HasValue == true)
            {
                switch (suffix.Val.Value)
                {
                    case LevelSuffixValues.Space:
                        return " ";
                    case LevelSuffixValues.Tab:
                        return "\t";
                    case LevelSuffixValues.Nothing:
                        return "";
                }
            }

            return "";
        }

        private void PrepareNumbering()
        {
            if (_numberingDefinitionsPart == null || _numberingDefinitionsPart.Numbering == null)
            {
                return;
            }

            foreach (var child in _numberingDefinitionsPart.Numbering.ChildElements)
            {
                if (child is AbstractNum abstractNum
                    && abstractNum.AbstractNumberId?.HasValue == true)
                {
                    var abstractNumId = abstractNum.AbstractNumberId.Value;
                    foreach (var item in abstractNum.ChildElements)
                    {
                        if (item is Level level
                            && level.LevelIndex?.HasValue == true
                            && level.LevelText?.Val?.HasValue == true)
                        {
                            var def = new NumberingDefinition
                            {
                                StartFrom = level.StartNumberingValue?.Val?.HasValue == true ? level.StartNumberingValue.Val.Value : 1,
                                Template = level.LevelText.Val.Value,
                                Suffix = ParseLevelSuffix(level.LevelSuffix),
                                Style = ParseNumberingFormat(level.NumberingFormat),
                            };

                            _numberingManager.AddAbstractNumbering(
                                abstractNumId,
                                level.LevelIndex.Value,
                                def);
                        }
                    }
                }
            }

            foreach (var child in _numberingDefinitionsPart.Numbering.ChildElements)
            {
                if (child is NumberingInstance num
                    && num.NumberID?.HasValue == true
                    && num.AbstractNumId?.Val?.HasValue == true)
                {
                    _numberingManager.AddNumbering(num.NumberID.Value, num.AbstractNumId.Val.Value);

                    foreach (var item in num.ChildElements)
                    {
                        if (item is LevelOverride lvlOverride
                            && lvlOverride.LevelIndex?.HasValue == true)
                        {
                            var def = new OverrideNumberingDefinition();

                            var lvl = lvlOverride.Level;
                            if (lvl != null)
                            {
                                if (lvl.StartNumberingValue?.Val?.HasValue == true)
                                {
                                    def.StartFrom
                                        = lvl.StartNumberingValue.Val.Value;
                                }

                                if (lvl.LevelSuffix?.Val?.HasValue == true)
                                {
                                    def.Suffix = ParseLevelSuffix(lvl.LevelSuffix);
                                }

                                if (lvl.LevelText?.Val?.HasValue == true)
                                {
                                    def.Template = lvl.LevelText.Val.Value;
                                }

                                if (lvl.NumberingFormat?.Val?.HasValue == true)
                                {
                                    def.Style = ParseNumberingFormat(lvl.NumberingFormat);
                                }
                            }

                            if (lvlOverride.StartOverrideNumberingValue?.Val?.HasValue == true)
                            {
                                def.StartFrom = lvlOverride.StartOverrideNumberingValue.Val.Value;
                            }

                            _numberingManager.OverrideNumbering(
                                num.NumberID,
                                lvlOverride.LevelIndex.Value,
                                def);
                        }
                    }

                }
            }
        }

        public NumberingIndex? ParseNumberingProperties(NumberingProperties? numberingProperties)
        {
            var level = numberingProperties?.NumberingLevelReference?.Val?.Value;
            var numId = numberingProperties?.NumberingId?.Val?.Value;

            if (level != null && numId != null)
            {
                return new NumberingIndex
                {
                    GroupId = numId.Value,
                    Level = level.Value,
                };
            }

            return null;
        }

        public ParagraphStyle ParseParagraphProperties(ParagraphProperties? paragraphProperties)
        {
            if (paragraphProperties == null)
            {
                return new ParagraphStyle();
            }

            return new ParagraphStyle
            {
                Indent = ParseIndentation(paragraphProperties.Indentation),
                Justification = ParseJustification(paragraphProperties.Justification),
                NumberingIndex = ParseNumberingProperties(paragraphProperties.NumberingProperties),
            };
        }

        internal IEnumerable<IPart> ConcatTextPartsWithSameStyle(IEnumerable<IPart> parts)
        {
            var result = new List<IPart>();
            TextPart concatPart = new TextPart();

            foreach (var part in parts)
            {
                if (part is TextPart textPart)
                {
                    if (string.IsNullOrEmpty(concatPart.Content))
                    {
                        concatPart = textPart;
                    }
                    else if (concatPart.Style == textPart.Style)
                    {
                        concatPart.Content += textPart.Content;
                    }
                    else
                    {
                        result.Add(concatPart);
                        concatPart = textPart;
                    }
                }
                else
                {
                    result.Add(part);
                }
            }

            if (!string.IsNullOrEmpty(concatPart.Content))
            {
                result.Add(concatPart);
            }

            return result;
        }

        public ParagraphPart ParseParagraph(int i, Paragraph paragraph)
        {
            var parts = new List<IPart>();

            foreach (var element in paragraph.Elements())
            {
                switch (element)
                {
                    case Run run:
                        parts.AddRange(_runParser.ParseRun(run));
                        break;
                    case Break _:
                        parts.Add(new TextPart { Content = "\n", Style = new TextStyle() });
                        break;
                }
            }

            parts = ConcatTextPartsWithSameStyle(parts).ToList();

            return new ParagraphPart
            {
                Order = i,
                Style = ParseParagraphProperties(paragraph.ParagraphProperties),
                Parts = parts,
            };
        }

    }
}
