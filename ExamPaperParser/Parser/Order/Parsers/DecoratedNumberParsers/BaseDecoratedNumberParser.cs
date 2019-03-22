using System;
using System.Collections.Generic;
using System.Text;
using ExamPaperParser.Parser.DataView;
using ExamPaperParser.Parser.Order.Models.DecoratedNumbers;
using ExamPaperParser.Parser.Order.Models.Numbers;
using ExamPaperParser.Parser.Order.Parsers.NumberParsers;

namespace ExamPaperParser.Parser.Order.Parsers.DecoratedNumberParsers
{
    public abstract class BaseDecoratedNumberParser : IParser<BaseDecoratedNumber>
    {
        protected IEnumerable<BaseNumberParser> _numberParsers;

        public BaseDecoratedNumberParser(IEnumerable<BaseNumberParser> numberParsers)
        {
            _numberParsers = numberParsers;
        }

        protected BaseNumber? ConsumeNumber(IDataView data)
        {
            BaseNumber? number = null;

            foreach (var numberParser in _numberParsers)
            {
                if ((number = numberParser.Consume(data)) != null)
                {
                    break;
                }
            }

            return number;
        }

        public abstract BaseDecoratedNumber? Consume(IDataView data);
    }
}
