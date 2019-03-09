using System.Diagnostics;

namespace FormattedFileParser.Models.Parts
{
    [DebuggerDisplay("Content = {Content}")]
    public class Part
    {
        public string Content { get; set; } = "";
    }
}