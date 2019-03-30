using System;
using System.Collections.Generic;
using System.Text;
using FormattedFileParser.Models.Parts.Paragraphs.Style;
using FormattedFileParser.NumberingUtils.Managers;

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

        public string Convert(NumberingDefinition numberingDefinition, int number)
        {
            if (_converters.TryGetValue(numberingDefinition.Style, out var converter))
            {
                return converter.Convert(number);
            }

            throw new NotImplementedException($"{numberingDefinition.Style.ToString()} has not been supported");
        }
    }
}
