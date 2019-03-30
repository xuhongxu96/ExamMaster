using FormattedFileParser.Models.Parts;
using FormattedFileParser.NumberingUtils.Managers;
using System;
using System.Collections.Generic;
using System.Text;

namespace FormattedFileParser.Models
{
    public class ParsedFile
    {
        public IList<IMainPart> Parts { get; }

        public INumberingManager NumberingManager { get; }

        public ParsedFile(IEnumerable<IMainPart> parts, INumberingManager numberingManager)
        {
            Parts = new List<IMainPart>(parts);
            NumberingManager = numberingManager;
        }
    }
}
