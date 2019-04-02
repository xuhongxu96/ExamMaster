using System;
using System.Collections.Generic;
using System.Text;

namespace QuestionClassifier.Classifier
{
    public struct RuleContribution
    {
        public bool IsBlacklist { get; set; }

        public string Rule { get; set; }
    }
}
