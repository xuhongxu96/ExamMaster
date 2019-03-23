using System;
using System.Collections.Generic;
using System.Text;

namespace ExamPaperParser.DataView
{
    public interface IDataView
    {
        int Position { get; }

        bool EndOfStream { get; }

        ReadOnlySpan<char> CurrentView { get; }

        IDataView Clone(int newPosition);

        IDataView CloneByDelta(int deltaPosition);
    }
}
