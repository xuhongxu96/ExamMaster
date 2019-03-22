using System;
using System.Collections.Generic;
using System.Text;

namespace ExamPaperParser.Parser.DataView
{
    public interface IDataView
    {
        int Position { get; set; }
        bool EndOfStream { get; }
        ReadOnlySpan<char> CurrentView { get; }
    }
}
