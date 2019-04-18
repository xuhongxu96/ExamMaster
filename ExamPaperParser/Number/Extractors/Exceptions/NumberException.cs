using System;
using System.Collections.Generic;
using System.Text;
using FormattedFileParser.Exceptions;

namespace ExamPaperParser.Number.Extractors.Exceptions
{
    public class NumberException : ParagraphFormatException
    {
        public string Position { get; }

        public NumberException(string message, string position, string content) : base(message, content)
        {
            Position = position;
        }
    }
}
