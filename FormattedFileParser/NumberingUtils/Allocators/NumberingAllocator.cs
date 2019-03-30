using System;
using System.Collections.Generic;
using System.Text;
using FormattedFileParser.Models.Parts.Paragraphs.Style;
using FormattedFileParser.NumberingUtils.Managers;

namespace FormattedFileParser.NumberingUtils.Allocators
{
    public class NumberingAllocator
    {
        private INumberingManager _numberingManager;
        private readonly Dictionary<Tuple<int, int>, int> _numberingMaxOrder
            = new Dictionary<Tuple<int, int>, int>();

        public NumberingAllocator(INumberingManager numberingManager)
        {
            _numberingManager = numberingManager;
        }

        public void Reset()
        {
            _numberingMaxOrder.Clear();
        }

        public void Reset(INumberingManager numberingManager)
        {
            Reset();
            _numberingManager = numberingManager;
        }

        /// <summary>
        /// Return order in each level less than and equals current level
        /// </summary>
        /// <param name="id"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        public int[] Allocate(int id, int level)
        {
            var orders = new int[level + 1];

            for (var i = 0; i <= level; ++i)
            {
                var key = Tuple.Create(id, level);

                var def = _numberingManager.GetNumbering(id, i);
                if (_numberingMaxOrder.TryGetValue(key, out var order))
                {
                    if (i == level)
                    {
                        ++order;
                    }

                    orders[i] = order;
                }
                else
                {
                    orders[i] = order = def.StartFrom;
                }

                if (i == level)
                {
                    _numberingMaxOrder[key] = order;
                }
            }

            return orders;
        }
    }
}
