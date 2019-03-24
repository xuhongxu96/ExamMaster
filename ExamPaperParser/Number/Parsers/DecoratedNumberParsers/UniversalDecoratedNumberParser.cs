using System;
using System.Collections.Generic;
using System.Text;
using ExamPaperParser.Base;
using ExamPaperParser.DataView;
using ExamPaperParser.Number.Models.DecoratedNumbers;
using ExamPaperParser.Number.Parsers.NumberParsers;

namespace ExamPaperParser.Number.Parsers.DecoratedNumberParsers
{
    public class UniversalDecoratedNumberParser : IDecoratedNumberParser
    {
        private IEnumerable<BaseDecoratedNumberParser> _parsers;

        public UniversalDecoratedNumberParser(IEnumerable<BaseDecoratedNumberParser> parsers)
        {
            _parsers = parsers;
        }

        public UniversalDecoratedNumberParser(INumberParser numberParser)
        {
            _parsers = new List<BaseDecoratedNumberParser>
            {
                new BracketDecoratedNumberParser(numberParser),
                new UndecoratedAndDelimiterDecoratedNumberParser(numberParser),
            };
        }

        public IEnumerable<ParsedResult<BaseDecoratedNumber>> Consume(IDataView data)
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
