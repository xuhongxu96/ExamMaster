using System;
using System.Collections.Generic;
using System.Text;
using QuestionClassifier.WildcardRule;

namespace QuestionClassifier.Classifier
{
    public class QuestionClassifier
    {
        private readonly WildcardRuleEngine _ruleEngine = new WildcardRuleEngine();

        public QuestionClassifier(string dimension, string classification)
        {
            Dimension = dimension;
            Classification = classification;
        }

        public string Dimension { get; set; }

        public string Classification { get; set; }

        public List<string> WhitelistRules { get; } = new List<string>();

        public List<string> BlacklistRules { get; } = new List<string>();

        public bool IsInThisClassification(string query, out string? matchedRule)
        {
            foreach (var rule in BlacklistRules)
            {
                if (_ruleEngine.IsMatch(rule, query))
                {
                    matchedRule = rule;
                    return false;
                }
            }

            foreach (var rule in WhitelistRules)
            {
                if (_ruleEngine.IsMatch(rule, query))
                {
                    matchedRule = rule;
                    return true;
                }
            }

            matchedRule = null;
            return false;
        }
    }
}
