using System;
using System.Collections.Generic;
using System.Text;
using FormattedFileParser.Models.Parts.Paragraphs.Style;

namespace FormattedFileParser.NumberingUtils.Converters
{
    public class NumberingConverterRegistry
    {
        private readonly Dictionary<NumberingStyle, INumberingConverter> _converters
            = new Dictionary<NumberingStyle, INumberingConverter>();

        public NumberingConverterRegistry Register(INumberingConverter converter)
        {
            _converters[converter.Style] = converter;
            return this;
        }

        public string Convert(Numbering numbering, int number)
        {
            return _converters[numbering.Style].Convert(number);
        }
    }
}
