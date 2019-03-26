using ExamPaperParser.Number.Models.NumberTree;

namespace ExamPaperParser.Number.Visitors
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="node"></param>
    /// <param name="level"></param>
    /// <returns>True if continue visiting the children</returns>
    public delegate bool OnVisited(NumberNode node, int level);

    public interface INumberNodeVisitor
    {
        event OnVisited OnVisited;

        void Visit(NumberRoot root);
    }
}