using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace QuestionClassifier.WildcardRule
{
    public class Rule
    {
        internal List<Regex> AndRegexList { get; } = new List<Regex>();
    }
}
