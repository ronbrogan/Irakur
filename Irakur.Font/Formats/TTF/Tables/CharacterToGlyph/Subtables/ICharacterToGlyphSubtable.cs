using System;
using System.Collections.Generic;
using System.Text;

namespace Irakur.Font.Formats.TTF.Tables.CharacterToGlyph.Subtables
{
    public interface ICharacterToGlyphSubtable : IDisposable
    {
        void Process(TrueTypeReader reader, uint offset);
        ushort GetGlyphId(ushort charCode);
    }
}
