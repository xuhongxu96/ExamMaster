using FormattedFileParser.Models.Parts.Paragraphs;
using System.Collections.Generic;

namespace FormattedFileParser.Models.Parts.Tables
{
    public struct TableCellPart : IPart
    {
        public string Content { get; set; }
        public IList<IMainPart> MainParts { get; set; } 
    }
}