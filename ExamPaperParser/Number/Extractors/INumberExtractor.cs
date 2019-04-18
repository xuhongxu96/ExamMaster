using System;
using System.Collections.Generic;
using ExamPaperParser.Number.Models.NumberTree;
using FormattedFileParser.Exceptions;
using FormattedFileParser.Models;

namespace ExamPaperParser.Number.Extractors
{
    public interface INumberExtractor
    {
        IEnumerable<Tuple<string, NumberRoot, List<ParagraphFormatException>>> Extract(ParsedFile file);
    }
}