using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using ExamPaperParser.Number.Models.DecoratedNumbers;

namespace ExamPaperParser.Number.Manager.Exceptions
{
    public class InvalidNumberException : Exception
    {
        public BaseDecoratedNumber DecoratedNumber { get; }

        public InvalidNumberException(BaseDecoratedNumber decoratedNumber)
        {
            DecoratedNumber = decoratedNumber;
        }

        public InvalidNumberException(BaseDecoratedNumber decoratedNumber, string message) 
            : base(message)
        {
            DecoratedNumber = decoratedNumber;
        }

        public InvalidNumberException(BaseDecoratedNumber decoratedNumber, string message, Exception innerException) 
            : base(message, innerException)
        {
            DecoratedNumber = decoratedNumber;
        }

        protected InvalidNumberException(BaseDecoratedNumber decoratedNumber, SerializationInfo info, StreamingContext context) 
            : base(info, context)
        {
            DecoratedNumber = decoratedNumber;
        }
    }
}
