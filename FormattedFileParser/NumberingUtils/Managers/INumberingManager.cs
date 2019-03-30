using System;
using System.Collections.Generic;
using System.Text;
using FormattedFileParser.Parsers.Docx.Managers;

namespace FormattedFileParser.NumberingUtils.Managers
{
    public interface INumberingManager
    {
        NumberingDefinition GetNumbering(int id, int level);
    }
}
