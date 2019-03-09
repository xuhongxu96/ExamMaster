using System;
using System.Collections.Generic;
using System.Text;

namespace FormattedFileParser.Models.Parts.Tables
{
    public class TableRowPart : Part
    {
        public IList<TableCellPart> Cells { get; set; } = new List<TableCellPart>();
    }
}
