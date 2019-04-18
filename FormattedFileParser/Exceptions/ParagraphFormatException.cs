using System;
using System.Collections.Generic;
using System.Text;

namespace FormattedFileParser.Exceptions
{
    public class ParagraphFormatException : FormatException
    {
        public string Content { get; }

        public ParagraphFormatException(string message, string content) : base(message)
        {
            Content = content;
        }
    }
}
