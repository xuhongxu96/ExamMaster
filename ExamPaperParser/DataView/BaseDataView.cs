﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace ExamPaperParser.DataView
{
    [DebuggerDisplay("CurrentView = {CurrentView}, Pos = {Position}")]
    public abstract class BaseDataView : IDataView
    {
        public int Position { get; set; }

        public abstract bool EndOfStream { get; }

        public abstract ReadOnlySpan<char> CurrentView { get; }

        public abstract IDataView Clone(int newPosition);

        public IDataView CloneByDelta(int deltaPosition) => Clone(Position + deltaPosition);
    }
}
