using System;
using System.Collections.Generic;
using System.Text;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;

namespace FormattedFileParser.Parsers.Docx.InternalParsers
{
    public static class OpenXmlTypeHelper
    {
        public static bool OnOffToBool(OnOffType? type)
        {
            // doesn't exist this property
            if (type == null)
                return false;

            // this property exists with true value or without specified value, true by default
            if (type.Val == null || !type.Val.HasValue || type.Val.Value == true)
                return true;

            return false;
        }

        public static bool EnumToBool<T>(EnumValue<T> value, T noneValue)
            where T : struct
        {
            // doesn't exist this property
            if (value == null)
                return false;

            // no specified value, true by default
            if (!value.HasValue)
                return true;

            if (value.Value.Equals(noneValue))
            {
                return false;
            }

            return true;
        }
    }
}
