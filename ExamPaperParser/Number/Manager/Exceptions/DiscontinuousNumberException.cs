using System;
using System.Collections.Generic;
using System.Text;
using ExamPaperParser.Number.Models.DecoratedNumbers;

namespace ExamPaperParser.Number.Manager.Exceptions
{
    public class DiscontinuousNumberException : InvalidNumberException
    {
        public DiscontinuousNumberException(BaseDecoratedNumber decoratedNumber, int expectedNumber)
            : base(decoratedNumber, $"遇到题号 \"{decoratedNumber.RawRepresentation}\" " +
                  $"(={decoratedNumber.Number.IntNumber}), " +
                  $"但此处应为{expectedNumber}")
        {
            ExpectedNumber = expectedNumber;
        }


        public int ExpectedNumber { get; }
    }
}
