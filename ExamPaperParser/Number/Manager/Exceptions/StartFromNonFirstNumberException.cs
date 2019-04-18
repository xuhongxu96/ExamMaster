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
            : base(decoratedNumber, $"遇到题号 \"{decoratedNumber.RawRepresentation}\" " +
                  $"(={decoratedNumber.Number.IntNumber}), " +
                  $"但该类型({decoratedNumber.GetType().Name}, {decoratedNumber.Number.GetType().Name})第一次出现，应为1")
        {
        }
    }
}
