using System;
using System.Collections.Generic;
using ExamPaperParser.Number.Manager;
using ExamPaperParser.Number.Models.NumberTree;

namespace ExamPaperParser.Number.Postprocessors
{
    public interface IPostprocessor
    {
        List<Exception> Process(NumberRoot root);
    }
}