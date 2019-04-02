using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuestionClassifier.Classifier
{
    public class QuestionClassifierRegistry
    {
        public List<QuestionClassifier> Classifiers { get; } = new List<QuestionClassifier>();

        public string Classify(string query, out RuleContribution? ruleContribution)
        {
            var multiclass = new Dictionary<string, RuleContribution?>();
            foreach (var classifier in Classifiers)
            {
                if (classifier.IsInThisClassification(query, out var outRuleContribution))
                {
                    multiclass[classifier.Classification] = outRuleContribution;
                }
            }

            if (multiclass.Count == 1)
            {
                var result = multiclass.Single();
                ruleContribution = result.Value;
                return result.Key;
            }

            throw new MulticlassException(multiclass);
        }
    }
}
