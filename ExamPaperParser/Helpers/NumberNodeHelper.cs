using System;
using System.Collections.Generic;
using System.Text;
using ExamPaperParser.Number.Models.NumberTree;

namespace ExamPaperParser.Helpers
{
    public static class NumberNodeHelper
    {
        public static List<string> GetNumberChain(NumberNode current)
        {
            var chain = new List<string>();
            var p = current;
            while (true)
            {
                chain.Add(p.DecoratedNumber.RawRepresentation);

                var parent = p.Parent;
                if (parent is NumberNode node)
                {
                    p = node;
                }
                else
                {
                    break;
                }
            }

            chain.Reverse();
            return chain;
        }

        public static int GetMaxNumber(NumberRoot root)
        {
            var maxN = 0;

            foreach (var child in root.Children)
            {
                maxN = Math.Max(maxN, GetMaxNumber(child));
            }

            return maxN;
        }

        public static int GetMaxNumber(NumberNode node)
        {
            var maxN = node.DecoratedNumber.Number.IntNumber;

            foreach (var child in node.Children)
            {
                maxN = Math.Max(maxN, GetMaxNumber(child));
            }

            return maxN;
        }
    }
}
