using FormattedFileParser.Models.Parts;
using System;
using System.Collections.Generic;
using System.Text;

namespace FormattedFileParser.Models
{
    public class ParsedFile
    {
        public IList<IMainPart> Parts { get; }

        public ParsedFile(IEnumerable<IMainPart> parts)
        {
            Parts = new List<IMainPart>(parts);
        }
    }
}
