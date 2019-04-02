using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace QuestionClassifier.WildcardRule
{
    public class WildcardToRegexConverter
    {
        private string ReplaceWildcardWithRegex(string rule)
        {
            return rule
                .Replace("*", ".*?")
                .Replace("?", ".");
        }

        public Rule Convert(string rule)
        {
            rule = ReplaceWildcardWithRegex(rule);
            var andRules = rule.Split('&');

            var result = new Rule();
            foreach (var andRule in andRules)
            {
                result.AndRegexList.Add(new Regex(andRule, RegexOptions.Compiled));
            }

            return result;
        }
    }
}
