using System;
using System.Collections.Generic;
using FormattedFileParser.Models;

namespace FormattedFileParser.Processors
{
    public interface IProcessor
    {
        List<Exception> Process(ParsedFile file);
    }
}