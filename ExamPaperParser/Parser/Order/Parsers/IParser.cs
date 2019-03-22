using System;
using System.Collections.Generic;
using System.Text;
using ExamPaperParser.Parser.DataView;

namespace ExamPaperParser.Parser.Order.Parsers
{
    public interface IParser<T>
    {
        T? Consume(IDataView data);
    }
}
