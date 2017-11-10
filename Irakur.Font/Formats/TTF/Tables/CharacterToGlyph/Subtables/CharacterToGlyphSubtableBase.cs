namespace Irakur.Font.Formats.TTF.Tables.CharacterToGlyph
{
    public abstract class CharacterToGlyphSubtableBase : FontTableBase<CmapSubtableType>
    {
        public abstract ushort GetGlyphId(ushort charCode);

        public ushort Version { get; set; }

        internal EncodingTableEntry EncodingTableEntry { get; set; }
    }
}
