using FormattedFileParser.Models.Parts.Texts;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Text;
using FormattedFileParser.Models.Parts;
using DocumentFormat.OpenXml;

namespace FormattedFileParser.Parsers.Docx.Helpers
{
    public static class RunParseHelper
    {
        public static string? ParseRunTextElement(OpenXmlElement element)
        {
            var result = "";
            switch (element)
            {
                case Text text:
                    if (text.Space == null ||
                        !text.Space.HasValue ||
                        text.Space.Value == SpaceProcessingModeValues.Default)
                    {
                        result = text.Text.Trim();
                    }
                    else
                    {
                        result = text.Text;
                    }
                    return result;
                case TabChar _:
                    return "\t";
                case Break _:
                    return "\n";
                case Drawing _:
                case Picture _:
                default:
                    return " ";
            }
        }

        public static TextStyle ParseRunProperties(RunProperties runProp)
        {
            return new TextStyle
            {
                Size = int.Parse(runProp.FontSize?.Val?.Value ?? "21"),
                IsBold = OpenXmlTypeHelper.OnOffToBool(runProp.Bold),
                IsItalic = OpenXmlTypeHelper.OnOffToBool(runProp.Italic),
                IsUnderlined = OpenXmlTypeHelper.EnumToBool(runProp.Underline?.Val, UnderlineValues.None),
                IsEmphasized = OpenXmlTypeHelper.EnumToBool(runProp.Emphasis?.Val, EmphasisMarkValues.None),
                TextColor = runProp.Color?.Val?.Value ?? "000000",
            };
        }

        public static IEnumerable<IPart> ParseRun(Run run)
        {
            var style = ParseRunProperties(run.RunProperties);

            foreach (var element in run.ChildElements)
            {
                if (element is RunProperties)
                {
                    continue;
                }

                var textResult = ParseRunTextElement(element);
                if (textResult == null)
                {
                    // Skip Picture / Drawing for now
                }
                else
                {
                    yield return new TextPart
                    {
                        Content = textResult,
                        Style = style,
                    };
                }
            }
        }
    }
}
