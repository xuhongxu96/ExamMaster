using System;
using System.Collections.Generic;
using System.Text;
using ExamPaperParser.Parser.DataView;

namespace ExamPaperParser.Parser.Order.Parsers
{
    public class OrderParser
    {
        public bool Accept(IDataView data)
        {
            if (data.EndOfStream) return false;
        }

        public INumber Consume(IDataView data)
        {
            var m = regex.Match(data.CurrentView.ToString());
            var rawNumber = m.Value;
            return ConstructNumber(rawNumber, ParseRawNumber(rawNumber));
        }
    }
}
