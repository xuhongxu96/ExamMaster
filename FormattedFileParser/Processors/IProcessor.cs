using System;
using System.Collections.Generic;
using FormattedFileParser.Exceptions;
using FormattedFileParser.Models;

namespace FormattedFileParser.Processors
{
    public interface IProcessor
    {
        List<ParagraphFormatException> Process(ParsedFile file);
    }
}