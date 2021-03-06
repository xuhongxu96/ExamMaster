﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using FormattedFileParser.Exceptions;
using FormattedFileParser.Models;
using FormattedFileParser.Models.Parts.Paragraphs;
using FormattedFileParser.Models.Parts.Texts;
using FormattedFileParser.NumberingUtils.Allocators;
using FormattedFileParser.NumberingUtils.Converters;
using FormattedFileParser.NumberingUtils.Managers;

namespace FormattedFileParser.Processors
{
    public class PrependNumberingToContentProcessor : IProcessor
    {
        private readonly static Regex _placeholderRegex = new Regex(@"%\d+", RegexOptions.Compiled);

        public NumberingConverterRegistry NumberingConverterRegistry { get; }

        public PrependNumberingToContentProcessor()
            : this(new NumberingConverterRegistry())
        { }

        public PrependNumberingToContentProcessor(
            NumberingConverterRegistry numberingConverterRegistry)
        {
            NumberingConverterRegistry = numberingConverterRegistry;
        }

        public List<ParagraphFormatException> Process(ParsedFile file)
        {
            var exceptions = new List<ParagraphFormatException>();
            var numberingAllocator = new NumberingAllocator(file.NumberingManager);

            foreach (var part in file.Parts)
            {
                if (part is ParagraphPart paragraphPart)
                {
                    if (paragraphPart.Style.NumberingIndex.HasValue)
                    {
                        var numIndex = paragraphPart.Style.NumberingIndex.Value;
                        var def = file.NumberingManager.GetNumbering(numIndex.GroupId, numIndex.Level);

                        var orders = numberingAllocator.Allocate(numIndex.GroupId, numIndex.Level);
                        var finalRepr = def.Template;

                        var m = _placeholderRegex.Match(def.Template);
                        var hasError = !m.Success;
                        while (m.Success)
                        {
                            var level = int.Parse(m.Value.Substring(1)) - 1;
                            var levelDef = file.NumberingManager.GetNumbering(numIndex.GroupId, level);

                            try
                            {
                                var repr = NumberingConverterRegistry.Convert(levelDef, orders[level]);
                                finalRepr = finalRepr.Replace(m.Value, repr);
                            }
                            catch (NotImplementedException e)
                            {
                                exceptions.Add(new ParagraphFormatException(e.Message, paragraphPart.Content));
                                hasError = true;
                            }

                            if (hasError)
                            {
                                break;
                            }

                            m = m.NextMatch();
                        }

                        if (hasError)
                        {
                            continue;
                        }

                        TextStyle style;
                        if (paragraphPart.Parts.Any()
                            && paragraphPart.Parts[0] is TextPart textPart)
                        {
                            style = textPart.Style;
                        }
                        else
                        {
                            style = new TextStyle
                            {
                                IsBold = false,
                                IsEmphasized = false,
                                IsItalic = false,
                                IsUnderlined = false,
                                Size = 21,
                                TextColor = "000000",
                            };
                        }

                        paragraphPart.Parts.Insert(0, new TextPart
                        {
                            Content = finalRepr,
                            Style = style,
                        });
                    }
                }
            }

            return exceptions;
        }
    }
}