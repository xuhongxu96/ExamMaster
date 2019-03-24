using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using ExamPaperParser.Number.Models.DecoratedNumbers;

namespace ExamPaperParser.Number.Manager.Exceptions
{
    public class StartFromNonFirstNumberException : InvalidNumberException
    {
        public StartFromNonFirstNumberException(BaseDecoratedNumber decoratedNumber) 
            : base(decoratedNumber, $"Current number is {decoratedNumber.Number.IntNumber}" +
                  $" (parsed from \"{decoratedNumber.RawRepresentation}\")," +
                  $" but 1 is expected for the first number")
        {
        }
    }
}
