using System;
using System.Collections.Generic;
using System.Text;
using ExamPaperParser.DataView;

namespace ExamPaperParser.Base
{
    public class ParsedResult<T>
    {
        public ParsedResult(T result, IDataView dataView)
        {
            Result = result;
            DataView = dataView;
        }

        public T Result { get; set; }

        public IDataView DataView { get; set; }
    }
}
