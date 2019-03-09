using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FormattedFileParser.Models.Parts.Tables
{
    public struct TablePart : IMainPart
    {
        public string Content { get; set; }

        public int Order { get; set; }

        public IList<TableRowPart> Rows { get; set; }

        public IList<IPart> Parts
        {
            get
            {
                return Rows.Select(o => o as IPart).ToList();
            }
        }
    }
}
