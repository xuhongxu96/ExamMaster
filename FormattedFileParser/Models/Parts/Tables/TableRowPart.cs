using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FormattedFileParser.Models.Parts.Tables
{
    public struct TableRowPart : IParentPart
    {
        public string Content => string.Join("", Parts.Select(o => o.Content));

        public IList<TableCellPart> Cells { get; set; }

        public IList<IPart> Parts
        {
            get
            {
                return Cells.Select(o => o as IPart).ToList();
            }
        }
    }
}
