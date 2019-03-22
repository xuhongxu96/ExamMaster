using System;
using System.Collections.Generic;
using System.Text;

namespace ExamPaperParser.Parser.DataView
{
    public class StringDataView : IDataView
    {
        public StringDataView(string content)
        {
            Content = content;
        }

        public string Content { get; }

        public int Position { get; set; } = 0;

        public bool EndOfStream => Position >= Content.Length;

        public ReadOnlySpan<char> CurrentView => Content.AsSpan(Position);

    }
}
