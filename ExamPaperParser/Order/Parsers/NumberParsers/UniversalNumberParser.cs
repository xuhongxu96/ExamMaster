using System;
using System.Collections.Generic;
using System.Text;
using ExamPaperParser.Base;
using ExamPaperParser.DataView;
using ExamPaperParser.Order.Models.Numbers;

namespace ExamPaperParser.Order.Parsers.NumberParsers
{
    public class UniversalNumberParser : INumberParser
    {
        private IEnumerable<BaseNumberParser> _parsers;

        public UniversalNumberParser()
        {
            _parsers = new List<BaseNumberParser>
            {
                new HalfWidthArabicNumberParser(),
                new FullWidthArabicNumberParser(),
                new LowerRomanNumberParser(),
                new UpperRomanNumberParser(),
                new LowerChineseNumberParser(),
                new UpperChineseNumberParser(),
                new FullStopNumberParser(),
                new CircledNumberParser(),
                new ParenthesizedNumberParser(),
                new ParenthesizedAlphabeticalNumberParser(),
                new HalfWidthAlphabeticalNumberParser(),
                new FullWidthAlphabeticalNumberParser(),
            };
        }

        public UniversalNumberParser(IEnumerable<BaseNumberParser> numberParsers)
        {
            _parsers = numberParsers;
        }

        public IEnumerable<ParsedResult<BaseNumber>> Consume(IDataView data)
        {
            foreach (var parser in _parsers)
            {
                foreach (var result in parser.Consume(data))
                {
                    yield return result;
                }
            }
        }
    }
}
