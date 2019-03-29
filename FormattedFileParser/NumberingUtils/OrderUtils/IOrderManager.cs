using System;
using System.Collections.Generic;
using System.Text;

namespace FormattedFileParser.NumberingUtils.OrderUtils
{
    public interface IOrderManager
    {
        int GetNumberForLevel(int groupId, int level, int order);
    }
}
