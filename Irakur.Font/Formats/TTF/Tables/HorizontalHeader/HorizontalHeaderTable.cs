using System;
using System.Collections.Generic;
using System.Text;

namespace Irakur.Font.Formats.TTF.Tables.HorizontalHeader
{
    public class HorizontalHeaderTable : FontTableBase
    {
        public byte[] Version { get; set; }
        public short Ascender { get; set; }
        public short Descender { get; set; }
        public short LineGap { get; set; }
        public ushort AdvanceWidthMax { get; set; }
        public short MinLeftSideBearing { get; set; }
        public short MinRightSideBearing { get; set; }
        public short XMaxExtent { get; set; }
        public short CaretSlopeRise { get; set; }
        public short CaretSlopeRun { get; set; }
        public short Reserved0 { get; set; }
        public short Reserved1 { get; set; }
        public short Reserved2 { get; set; }
        public short Reserved3 { get; set; }
        public short Reserved4 { get; set; }
        public short MetricDataFormat { get; set; }
        public ushort NumberOfHorizontalMetrics { get; set; }

        public override void Process(TrueTypeFont font)
        {
            var reader = new TrueTypeReader(Data);

            Version = reader.ReadFixed();
            Ascender = reader.ReadFWord();
            Descender = reader.ReadFWord();
            LineGap = reader.ReadFWord();
            AdvanceWidthMax = reader.ReadUFWord();
            MinLeftSideBearing = reader.ReadFWord();
            MinRightSideBearing = reader.ReadFWord();
            XMaxExtent = reader.ReadFWord();
            CaretSlopeRise = reader.ReadShort();
            CaretSlopeRun = reader.ReadShort();
            Reserved0 = reader.ReadShort();
            Reserved1 = reader.ReadShort();
            Reserved2 = reader.ReadShort();
            Reserved3 = reader.ReadShort();
            Reserved4 = reader.ReadShort();
            MetricDataFormat = reader.ReadShort();
            NumberOfHorizontalMetrics = reader.ReadUShort();

            reader.Dispose();
        }
    }
}
