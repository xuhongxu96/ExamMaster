using System;
using System.Collections.Generic;
using System.Text;

namespace QuestionClassifier.Classifier
{
    public class QuestionDimensionRegistry
    {
        public List<QuestionClassifierRegistry> QuestionClassifierRegistries { get; } 
            = new List<QuestionClassifierRegistry>();
    }
}
