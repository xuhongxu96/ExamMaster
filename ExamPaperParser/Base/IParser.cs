using System;
using System.Collections.Generic;
using System.Text;
using ExamPaperParser.DataView;

namespace ExamPaperParser.Base
{
    public interface IParser<T>
    {
        IEnumerable<ParsedResult<T>> Consume(IDataView data);
    }
}
