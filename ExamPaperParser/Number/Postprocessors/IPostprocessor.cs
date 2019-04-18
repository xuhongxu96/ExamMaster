using System;
using System.Collections.Generic;
using ExamPaperParser.Number.Extractors.Exceptions;
using ExamPaperParser.Number.Manager;
using ExamPaperParser.Number.Models.NumberTree;
using FormattedFileParser.Exceptions;

namespace ExamPaperParser.Number.Postprocessors
{
    public interface IPostprocessor
    {
        List<ParagraphFormatException> Process(NumberRoot root);
    }
}