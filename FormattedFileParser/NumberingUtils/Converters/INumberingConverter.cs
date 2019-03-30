using FormattedFileParser.Models.Parts.Paragraphs.Style;
using FormattedFileParser.NumberingUtils.Managers;

namespace FormattedFileParser.NumberingUtils.Converters
{
    public interface INumberingConverter
    {
        NumberingStyle Style { get; }

        string Convert(int number);
    }
}