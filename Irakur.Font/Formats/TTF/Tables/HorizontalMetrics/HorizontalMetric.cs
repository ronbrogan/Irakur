using System;
using System.Collections.Generic;
using System.Text;

namespace Irakur.Font.Formats.TTF.Tables.HorizontalMetrics
{
    public struct HorizontalMetric
    {
        public ushort AdvanceWidth { get; set; }
        public short LeftSideBearing { get; set; }
    }
}
