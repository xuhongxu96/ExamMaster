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

        public BaseNumberParser(bool consumeFromStart = true)
        {
            if (consumeFromStart)
            {
                regex = new Regex($"^({MatchRegex})", RegexOptions.Compiled);
            }
            else
            {
                regex = new Regex(MatchRegex, RegexOptions.Compiled);
            }
        }

        public IEnumerable<ParsedResult<BaseNumber>> Consume(IDataView data)
        {
            if (data.EndOfStream)
            {
                yield break;
            }

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
