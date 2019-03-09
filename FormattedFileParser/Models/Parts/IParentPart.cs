using System;
using System.Collections.Generic;
using System.Text;

namespace FormattedFileParser.Models.Parts
{
    public interface IParentPart : IPart
    {
        IList<IPart> Parts { get; } 
    }
}
