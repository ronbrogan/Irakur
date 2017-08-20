using System;
using System.Collections.Generic;
using System.Text;

namespace Irakur.Font.Formats.TTF.Tables.CharacterToGlyph
{
    public abstract class CharacterToGlyphSubtableBase
    {
        public ushort Format { get; set; }
        public ushort Length { get; set; }
        public ushort Version { get; set; }

        /// <summary>
        /// Populates the Format, Length, and Version fields, when overriding, make sure to 
        /// call this base method first, or manually seek the reader yourself.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="offset"></param>
        public virtual void Process(TrueTypeReader reader, uint offset)
        {
            reader.Seek(offset);

            Format = reader.ReadUShort();
            Length = reader.ReadUShort();
            Version = reader.ReadUShort();
        }

    }
}
