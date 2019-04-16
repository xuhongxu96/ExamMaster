using System;
using System.Collections.Generic;
using System.Text;
using FormattedFileParser.NumberingUtils.Managers;

namespace FormattedFileParser.NumberingUtils.Converters
{
    public class UpperLetterNumberingConverter : INumberingConverter
    {
        public NumberingStyle Style => NumberingStyle.UpperLetter;

        public string Convert(int number)
        {
            return ((char)('A' + number - 1)).ToString();
        }
    }
}
