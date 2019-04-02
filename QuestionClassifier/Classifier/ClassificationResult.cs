using System;
using System.Collections.Generic;
using System.Text;

namespace QuestionClassifier.Classifier
{
    public struct ClassificationResult
    {
        public string Classification { get; set; }

        public bool IsMatched { get; set; }

        public string? Rule { get; set; }
    }
}
