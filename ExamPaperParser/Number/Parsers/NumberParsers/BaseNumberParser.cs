using ExamPaperParser.Base;
using ExamPaperParser.DataView;
using ExamPaperParser.Number.Models.Numbers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace ExamPaperParser.Number.Parsers.NumberParsers
{
    public abstract class BaseNumberParser : INumberParser
    {
        protected abstract string MatchRegex { get; }
        protected abstract int ParseRawNumber(string rawNumber);
        protected abstract BaseNumber ConstructNumber(string rawNumber, int number);

        protected Regex regex;

        public BaseNumberParser()
        {
            regex = new Regex(MatchRegex, RegexOptions.Compiled);
        }

        public IEnumerable<ParsedResult<BaseNumber>> Consume(IDataView data)
        {
            var m = regex.Match(data.CurrentView.ToString());
            if (!m.Success)
            {
                yield break;
            }

            var rawNumber = m.Value;
            yield return new ParsedResult<BaseNumber>(
                result: ConstructNumber(rawNumber, ParseRawNumber(rawNumber)),
                dataView: data.CloneByDelta(m.Index + m.Length));
        }
    }
}
