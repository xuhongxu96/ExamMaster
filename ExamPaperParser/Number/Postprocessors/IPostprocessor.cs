using ExamPaperParser.Number.Manager;
using ExamPaperParser.Number.Models.NumberTree;

namespace ExamPaperParser.Number.Postprocessors
{
    public interface IPostprocessor
    {
        void Process(NumberRoot root);
    }
}