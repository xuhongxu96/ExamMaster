using FormattedFileParser.Models.Parts.Paragraphs;
using System.Collections.Generic;

namespace FormattedFileParser.Models.Parts.Tables
{
    public class TableCellPart : Part
    {
        public IList<IMainPart> MainParts { get; set; } = new List<IMainPart>();
    }
}