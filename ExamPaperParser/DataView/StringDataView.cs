using System;
using System.Collections.Generic;
using System.Text;

namespace ExamPaperParser.DataView
{
    public class StringDataView : BaseDataView
    {
        public StringDataView(string content)
        {
            Content = content;
        }

        public string Content { get; }

        public override bool EndOfStream => Position >= Content.Length || Position < 0;

        public override ReadOnlySpan<char> CurrentView => Content.AsSpan(Position);

        public override IDataView Clone(int newPosition)
        {
            return new StringDataView(Content)
            {
                Position = newPosition,
            };
        }
    }
}
