using FormattedFileParser.Models.Parts.Paragraphs.Style;

namespace FormattedFileParser.NumberingUtils.Converters
{
    public interface INumberingConverter
    {
        NumberingStyle Style { get; }

        string Convert(int number);
    }
}