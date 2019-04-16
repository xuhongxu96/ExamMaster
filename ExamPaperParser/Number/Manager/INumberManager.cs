using System.Collections.Generic;
using ExamPaperParser.Number.Models.DecoratedNumbers;
using ExamPaperParser.Number.Models.NumberTree;

namespace ExamPaperParser.Number.Manager
{
    public struct LevelProps
    {
        public int Level { get; set; }

        public int MaxNumber { get; set; }
    }

    public struct Backup
    {
        public Dictionary<string, LevelProps> DifferentiatorLevelPropsMapping { get; set; }

        public HashSet<string> DifferentiatorSet { get; set; }

        public NumberNode? Current { get; set; }
    }

    public interface INumberManager
    {
        NumberRoot Root { get; }

        NumberNode? Current { get; }

        NumberNode AddNumber(BaseDecoratedNumber decoratedNumber, int paragraphOrder);

        void Reset();

        void Load(Backup backup);

        Backup Save();

        Dictionary<string, int> GetAllowFirstNumberForDifferentiators();

        void SetAllowFirstNumberForDifferentatiators(IDictionary<string, int> allowFirstNumbers);
    }
}