using System;
using System.Collections.Generic;
using System.Text;
using ExamPaperParser.Base;
using ExamPaperParser.DataView;
using ExamPaperParser.Number.Models.Numbers;

namespace ExamPaperParser.Number.Parsers.NumberParsers
{
    public class UniversalNumberParser : INumberParser
    {
        private IEnumerable<BaseNumberParser> _parsers;

        public UniversalNumberParser(bool consumeFromStart = true)
        {
            _parsers = new List<BaseNumberParser>
            {
                new HalfWidthArabicNumberParser(consumeFromStart),
                new FullWidthArabicNumberParser(consumeFromStart),
                new LowerRomanNumberParser(consumeFromStart),
                new UpperRomanNumberParser(consumeFromStart),
                new LowerChineseNumberParser(consumeFromStart),
                new UpperChineseNumberParser(consumeFromStart),
                new FullStopNumberParser(consumeFromStart),
                new CircledNumberParser(consumeFromStart),
                new ParenthesizedNumberParser(consumeFromStart),
                new ParenthesizedAlphabeticalNumberParser(consumeFromStart),
                new HalfWidthAlphabeticalNumberParser(consumeFromStart),
                new FullWidthAlphabeticalNumberParser(consumeFromStart),
                new ChineseIdeographNumberParser(consumeFromStart),
            };
        }

        public UniversalNumberParser(params BaseNumberParser[] numberParsers)
        {
            _parsers = numberParsers;
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
