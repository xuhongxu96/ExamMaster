using System;
using System.Collections.Generic;
using System.Text;
using ExamPaperParser.Number.Models.DecoratedNumbers;

namespace ExamPaperParser.Number.Manager.Exceptions
{
    public class DiscontinuousNumberException : InvalidNumberException
    {
        public DiscontinuousNumberException(BaseDecoratedNumber decoratedNumber, int expectedNumber)
            : base(decoratedNumber, $"Current number is {decoratedNumber.Number.IntNumber}" +
                  $" (parsed from \"{decoratedNumber.RawRepresentation}\")," +
                  $" but {expectedNumber} is expected")
        {
            ExpectedNumber = expectedNumber;
        }


        public int ExpectedNumber { get; }
    }
}
