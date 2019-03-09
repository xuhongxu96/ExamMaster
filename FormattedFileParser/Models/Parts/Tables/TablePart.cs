using System;
using System.Collections.Generic;
using System.Text;

namespace FormattedFileParser.Models.Parts.Tables
{
    public class TablePart : Part, IMainPart
    {
        public int Order { get; set; }

        public IList<TableRowPart> Rows { get; set; } = new List<TableRowPart>();
    }
}
