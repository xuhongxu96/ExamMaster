using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using FormattedFileParser.Exceptions;

namespace ExamPaperParser.Number.Extractors.Exceptions
{
    public class SevereException : ParagraphFormatException
    {
        public SevereException(string message) : base(message, "")
        {
        }
    }
}
