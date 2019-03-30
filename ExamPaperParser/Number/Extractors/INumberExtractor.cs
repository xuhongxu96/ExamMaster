using System;
using System.Collections.Generic;
using ExamPaperParser.Number.Manager;
using ExamPaperParser.Number.Models.NumberTree;
using FormattedFileParser.Models;

namespace ExamPaperParser.Number.Extractors
{
    public interface INumberExtractor
    {
        IEnumerable<Tuple<string, NumberRoot, List<Exception>>> Extract(ParsedFile file);
    }
}