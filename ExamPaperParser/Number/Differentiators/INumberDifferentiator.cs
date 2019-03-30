using System.Collections.Generic;
using ExamPaperParser.Number.Models.DecoratedNumbers;
using ExamPaperParser.Number.Models.Numbers;

namespace ExamPaperParser.Number.Differentiators
{
    public interface INumberDifferentiator
    {
        string GetDecoratedNumberDifferentiator(BaseDecoratedNumber number);

        string GetNumberDifferentiator(BaseNumber number);

        IReadOnlyCollection<string> AllowedDifferentiatorToSpanParents { get; }
    }
}