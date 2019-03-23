using System;
using System.Collections.Generic;
using System.Text;
using ExamPaperParser.Base;
using ExamPaperParser.DataView;
using ExamPaperParser.Order.Models.DecoratedNumbers;
using ExamPaperParser.Order.Models.Numbers;
using ExamPaperParser.Order.Parsers.NumberParsers;

namespace ExamPaperParser.Order.Parsers.DecoratedNumberParsers
{
    public abstract class BaseDecoratedNumberParser : IDecoratedNumberParser
    {
        protected INumberParser _numberParser;

        public BaseDecoratedNumberParser(INumberParser numberParser)
        {
            _numberParser = numberParser;
        }

        protected IEnumerable<ParsedResult<BaseNumber>> ConsumeNumber(IDataView data) => _numberParser.Consume(data);

        public abstract IEnumerable<ParsedResult<BaseDecoratedNumber>> Consume(IDataView data);
    }
}
