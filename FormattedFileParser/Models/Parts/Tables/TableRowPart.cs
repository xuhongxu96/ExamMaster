using System;
using System.Collections.Generic;
using System.Text;

namespace FormattedFileParser.Models.Parts.Tables
{
    public struct TableRowPart : IPart
    {
        public string Content { get; set; }
        public IList<TableCellPart> Cells { get; set; } 
    }
}
