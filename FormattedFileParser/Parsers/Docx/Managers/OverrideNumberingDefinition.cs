using System;
using System.Collections.Generic;
using System.Text;
using FormattedFileParser.NumberingUtils.Managers;

namespace FormattedFileParser.Parsers.Docx.Managers
{
    public struct OverrideNumberingDefinition
    {
        public NumberingStyle? Style { get; set; }

        public int? StartFrom { get; set; }

        public string? Template { get; set; }

        public string? Suffix { get; set; }

        public NumberingDefinition Override(NumberingDefinition def)
        {
            if (Style.HasValue)
            {
                def.Style = Style.Value;
            }

            if (StartFrom.HasValue)
            {
                def.StartFrom = StartFrom.Value;
            }

            if (Template != null)
            {
                def.Template = Template;
            }

            if (Suffix != null)
            {
                def.Suffix = Suffix;
            }

            return def;
        }
    }
}
