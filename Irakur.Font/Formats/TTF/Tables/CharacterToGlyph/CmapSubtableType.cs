using Irakur.Core.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Irakur.Font.Formats.TTF.Tables.CharacterToGlyph
{
    public enum CmapSubtableType : ushort
    {
        Apple = 0,
        Highbyte = 2,
        [ImplementationType(typeof(MicrosoftSubtable))]
        Microsoft = 4,
        Trimmed = 6
    }
}
