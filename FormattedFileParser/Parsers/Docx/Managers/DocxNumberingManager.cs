using System;
using System.Collections.Generic;
using System.Text;
using FormattedFileParser.NumberingUtils.Managers;

namespace FormattedFileParser.Parsers.Docx.Managers
{
    using NumberingDict = Dictionary<int, Dictionary<int, NumberingDefinition>>;

    public class DocxNumberingManager : INumberingManager
    {
        private readonly NumberingDict _abstractNumIdToStyle = new NumberingDict();
        private readonly NumberingDict _numberingStyleMapping = new NumberingDict();

        public void Reset()
        {
            _abstractNumIdToStyle.Clear();
            _numberingStyleMapping.Clear();
        }

        public void AddAbstractNumbering(int id, int level, NumberingDefinition numberingDefinition)
        {
            if (!_abstractNumIdToStyle.TryGetValue(id, out var levelDict))
            {
                _abstractNumIdToStyle[id] = levelDict = new Dictionary<int, NumberingDefinition>();
            }

            levelDict[level] = numberingDefinition;
        }

        public void AddNumbering(int id, int baseAbstractId)
        {
            if (!_abstractNumIdToStyle.TryGetValue(baseAbstractId, out var abstractLevelDict))
            {
                throw new ArgumentException($"Abstract Numbering ID \"{baseAbstractId}\" not found");
            }

            if (_numberingStyleMapping.TryGetValue(id, out var levelDict))
            {
                throw new ArgumentException($"Numbering ID \"{id}\" already exists");
            }

            _numberingStyleMapping[id] = new Dictionary<int, NumberingDefinition>(abstractLevelDict);
        }

        public void OverrideNumbering(int id, int level, OverrideNumberingDefinition overrideNumberingDefinition)
        {
            if (!_numberingStyleMapping.TryGetValue(id, out var levelDict))
            {
                throw new ArgumentException($"Numbering ID \"{id}\" not found");
            }

            if (levelDict.TryGetValue(level, out var def))
            {
                def = overrideNumberingDefinition.Override(def);
            }
            else
            {
                def = overrideNumberingDefinition.Override(new NumberingDefinition());
            }

            levelDict[level] = def;
        }

        public NumberingDefinition GetNumbering(int id, int level)
        {
            if (!_numberingStyleMapping.TryGetValue(id, out var levelDict)
                || !levelDict.TryGetValue(level, out var def))
            {
                throw new ArgumentException($"Level {level} of Numbering Id {id} not found");
            }

            return def;
        }
    }
}
