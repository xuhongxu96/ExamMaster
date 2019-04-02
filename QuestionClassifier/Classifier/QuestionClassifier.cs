using System;
using System.Collections.Generic;
using System.Text;
using QuestionClassifier.WildcardRule;

namespace QuestionClassifier.Classifier
{
    public class QuestionClassifier
    {
        private readonly WildcardRuleEngine _ruleEngine = new WildcardRuleEngine();

        public QuestionClassifier(string classification)
        {
            Classification = classification;
        }

        public string Classification { get; set; }

        public List<string> WhitelistRules { get; } = new List<string>();

        public List<string> BlacklistRules { get; } = new List<string>();

        public bool IsInThisClassification(string query, out RuleContribution? ruleContribution)
        {
            foreach (var rule in BlacklistRules)
            {
                if (_ruleEngine.IsMatch(rule, query))
                {
                    ruleContribution = new RuleContribution
                    {
                        IsBlacklist = true,
                        Rule = rule,
                    };
                    return false;
                }
            }

            foreach (var rule in WhitelistRules)
            {
                if (_ruleEngine.IsMatch(rule, query))
                {
                    ruleContribution = new RuleContribution
                    {
                        IsBlacklist = false,
                        Rule = rule,
                    };
                    return true;
                }
            }

            ruleContribution = null;
            return false;
        }
    }
}
