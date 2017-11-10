using System;

namespace Irakur.Font.Formats.TTF.Tables.HorizontalMetrics
{
    public class HorizontalMetricsTable : FontTableBase
    {
        public override FontTableType Type => FontTableType.HorizontalMetrics;

        public HorizontalMetric[] Metrics { get; set; }

        public override void Process(TrueTypeFont font)
        {
            if (font.hhea == null)
                throw new Exception("Table processing order error. Processing the hmtx table requires the hhea table.");

            Metrics = new HorizontalMetric[font.hhea.NumberOfHorizontalMetrics];

            var reader = new TrueTypeReader(Data);

            for(var m = 0; m < Metrics.Length; m++)
            {
                Metrics[m] = new HorizontalMetric()
                {
                    AdvanceWidth = reader.ReadUFWord(),
                    LeftSideBearing = reader.ReadFWord()
                };
            }

            reader.Dispose();
        }
    }
}
