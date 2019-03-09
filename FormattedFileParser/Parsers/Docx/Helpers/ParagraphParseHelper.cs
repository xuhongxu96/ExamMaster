using DocumentFormat.OpenXml.Wordprocessing;
using FormattedFileParser.Models.Parts;
using FormattedFileParser.Models.Parts.Paragraphs;
using FormattedFileParser.Models.Parts.Paragraphs.Style;
using FormattedFileParser.Models.Parts.Texts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FormattedFileParser.Parsers.Docx.Helpers
{
    public static class ParagraphParseHelper
    {
        public static Models.Parts.Paragraphs.Style.Indentation ParseIndentation(DocumentFormat.OpenXml.Wordprocessing.Indentation indent)
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

        public static Models.Parts.Paragraphs.Style.Justification ParseJustification(DocumentFormat.OpenXml.Wordprocessing.Justification justification)
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

        public static ParagraphStyle ParseParagraphProperties(ParagraphProperties paragraphProperties)
        {
            return new ParagraphStyle
            {
                Indent = ParseIndentation(paragraphProperties.Indentation),
                Justification = ParseJustification(paragraphProperties.Justification),
            };
        }

        internal static IEnumerable<Part> ConcatTextPartsWithSameStyle(IEnumerable<Part> parts)
        {
            var result = new List<Part>();
            TextPart? concatPart = null;

            foreach (var part in parts)
            {
                if (part is TextPart textPart)
                {
                    if (concatPart == null)
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

            if (concatPart != null)
            {
                result.Add(concatPart);
            }

            return result;
        }

        public static ParagraphPart ParseParagraph(int i, Paragraph paragraph)
        {
            var parts = new List<Part>();

            foreach (var run in paragraph.Elements<Run>())
            {
                parts.AddRange(RunParseHelper.ParseRun(run));
            }

            parts = ConcatTextPartsWithSameStyle(parts).ToList();

            return new ParagraphPart
            {
                Content = paragraph.InnerText,
                Order = i,
                Style = ParseParagraphProperties(paragraph.ParagraphProperties),
                Parts = parts,
            };
        }

    }
}
