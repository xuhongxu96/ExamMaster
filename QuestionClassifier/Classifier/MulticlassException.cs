using System;
using System.Collections.Generic;
using System.Text;

namespace QuestionClassifier.Classifier
{
    public class MulticlassException : Exception
    {
        public MulticlassException(Dictionary<string, RuleContribution?> triggeredClassification)
        {
            TriggeredClassification = triggeredClassification;
        }

        public Dictionary<string, RuleContribution> TriggeredClassification { get; } 
            = new Dictionary<string, RuleContribution>();
    }
}
