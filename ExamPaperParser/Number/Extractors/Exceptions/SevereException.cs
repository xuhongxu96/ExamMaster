using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace ExamPaperParser.Number.Extractors.Exceptions
{
    public class SevereException : FormatException
    {
        public SevereException()
        {
        }

        public SevereException(string message) : base(message)
        {
        }

        public SevereException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected SevereException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
