namespace Irakur.Font.Formats.TTF.Tables
{
    public class FontTableHeaderEntry
    {
        public FontTableType Type { get; set; }
        public uint Offset { get; set; }
        public uint Length { get; set; }
        public uint Checksum { get; set; }
    }
}
