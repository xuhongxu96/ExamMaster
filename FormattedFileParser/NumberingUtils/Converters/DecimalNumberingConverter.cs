using System;
using System.Collections.Generic;
using System.Text;
using FormattedFileParser.Models.Parts.Paragraphs.Style;
using FormattedFileParser.NumberingUtils.OrderUtils;

namespace FormattedFileParser.NumberingUtils.Converters
{
    public class DecimalNumberingConverter : INumberingConverter
    {
        public NumberingStyle Style => NumberingStyle.Decimal;

        public string Convert(int number)
        {
            return number.ToString();
        }
    }
}
