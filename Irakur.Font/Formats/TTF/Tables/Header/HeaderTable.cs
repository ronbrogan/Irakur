using System;

namespace Irakur.Font.Formats.TTF.Tables.Header
{
    public class HeaderTable : FontTableBase
    {
        public override FontTableType Type => FontTableType.FontHeader;

        public byte[] Version { get; set; }
        public byte[] Revision { get; set; }
        public ulong ChecksumAdjustment { get; set; }
        public ulong Magic { get; set; }
        public ushort Flags { get; set; }
        public ushort UnitsPerEm { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public short XMin { get; set; }
        public short YMin { get; set; }
        public short XMax { get; set; }
        public short YMax { get; set; }
        public ushort MacStyle { get; set; }
        public ushort LowestRecommendedPPEM { get; set; }
        public short FontDirectionHint { get; set; }
        public short IndexToLocFormat { get; set; }
        public short GlyphDataFormat { get; set; }

        public override void Process(TrueTypeFont font)
        {
            var reader = new TrueTypeReader(Data);

            Version = reader.ReadFixed();
            Revision = reader.ReadFixed();
            ChecksumAdjustment = reader.ReadULong();
            Magic = reader.ReadULong();
            Flags = reader.ReadUShort();
            UnitsPerEm = reader.ReadUShort();
            DateCreated = reader.ReadLongDateTime();
            DateModified = reader.ReadLongDateTime();
            XMin = reader.ReadFWord();
            YMin = reader.ReadFWord();
            XMax = reader.ReadFWord();
            YMax = reader.ReadFWord();
            MacStyle = reader.ReadUShort();
            LowestRecommendedPPEM = reader.ReadUShort();
            FontDirectionHint = reader.ReadShort();
            IndexToLocFormat = reader.ReadShort();
            GlyphDataFormat = reader.ReadShort();

            reader.Dispose();
        }
    }
}
