using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using ExamPaperParser.Number.Models.DecoratedNumbers;

namespace ExamPaperParser.Number.Manager.Exceptions
{
    public class ReadingParagraphNumberException : Exception
    {
        public BaseDecoratedNumber DecoratedNumber { get; }

        public ReadingParagraphNumberException(BaseDecoratedNumber decoratedNumber)
        {
            DecoratedNumber = decoratedNumber;
        }

        public ReadingParagraphNumberException(BaseDecoratedNumber decoratedNumber, string message) 
            : base(message)
        {
            DecoratedNumber = decoratedNumber;
        }

        public ReadingParagraphNumberException(BaseDecoratedNumber decoratedNumber, string message, Exception innerException) 
            : base(message, innerException)
        {
            DecoratedNumber = decoratedNumber;
        }

        protected ReadingParagraphNumberException(BaseDecoratedNumber decoratedNumber, SerializationInfo info, StreamingContext context) 
            : base(info, context)
        {
            DecoratedNumber = decoratedNumber;
        }
    }
}
