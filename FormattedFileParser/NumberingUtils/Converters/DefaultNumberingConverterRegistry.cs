using System;
using System.Collections.Generic;
using System.Text;

namespace FormattedFileParser.NumberingUtils.Converters
{
    public class DefaultNumberingConverterRegistry : NumberingConverterRegistry
    {
        public DefaultNumberingConverterRegistry()
        {
            Register(new DecimalNumberingConverter());
            Register(new ChineseCountingNumberingConverter());
        }
    }
}
