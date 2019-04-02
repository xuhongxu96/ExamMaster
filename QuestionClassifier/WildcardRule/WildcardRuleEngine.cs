using System;
using System.Collections.Generic;
using System.Text;

namespace QuestionClassifier.WildcardRule
{
    public class WildcardRuleEngine
    {
        private readonly WildcardToRegexConverter _converter = new WildcardToRegexConverter();

        public bool IsMatch(string rule, string query)
        {
            foreach (var r in _converter.Convert(rule).AndRegexList)
            {
                if (!r.IsMatch(query))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
