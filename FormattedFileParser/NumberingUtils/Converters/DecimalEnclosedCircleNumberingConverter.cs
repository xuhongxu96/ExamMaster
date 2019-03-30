using System;
using System.Collections.Generic;
using System.Text;
using FormattedFileParser.NumberingUtils.Managers;

namespace FormattedFileParser.NumberingUtils.Converters
{
    public class DecimalEnclosedCircleNumberingConverter : INumberingConverter
    {
        public NumberingStyle Style => NumberingStyle.DecimalEnclosedCircle;

        public string Convert(int number)
        {
            return ((char)('\u2460' + number - 1)).ToString();
        }
    }
}
