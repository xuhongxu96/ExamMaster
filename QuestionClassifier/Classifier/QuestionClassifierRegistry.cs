using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuestionClassifier.Classifier
{
    public class QuestionClassifierRegistry
    {
        public List<QuestionClassifier> Classifiers { get; } = new List<QuestionClassifier>();

        public IEnumerable<ClassificationResult> Classify(string query)
        {
            foreach (var classifier in Classifiers)
            {
                if (classifier.IsInThisClassification(query, out var matchedRule))
                {
                    yield return new ClassificationResult
                    {
                        IsMatched = true,
                        Classification = classifier.Classification,
                        Rule = matchedRule,
                    };
                }
                else
                {
                    yield return new ClassificationResult
                    {
                        IsMatched = false,
                        Classification = classifier.Classification,
                        Rule = matchedRule,
                    };
                }
            }
        }
    }
}
