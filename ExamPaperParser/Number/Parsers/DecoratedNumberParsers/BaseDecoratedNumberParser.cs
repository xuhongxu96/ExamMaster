using System;
using System.Collections.Generic;
using System.Text;
using ExamPaperParser.Base;
using ExamPaperParser.DataView;
using ExamPaperParser.Number.Models.DecoratedNumbers;
using ExamPaperParser.Number.Models.Numbers;
using ExamPaperParser.Number.Parsers.NumberParsers;

namespace ExamPaperParser.Number.Parsers.DecoratedNumberParsers
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
