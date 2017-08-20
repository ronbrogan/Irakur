using System;
using System.Collections.Generic;
using System.Text;

namespace Irakur.Font.Formats.TTF.Tables.CharacterToGlyph
{
    public class MicrosoftSubtable : CharacterToGlyphSubtableBase
    {
        public ushort DoubleSegCount { get; set; }
        public ushort SearchRange { get; set; }
        public ushort EntrySelector { get; set; }
        public ushort RangeShift { get; set; }
        public ushort[] EndCounts { get; set; }
        public ushort ReservedPad { get; set; }
        public ushort[] StartCounts { get; set; }
        public ushort[] IdDeltas { get; set; }
        public ushort[] IdRangeOffsets { get; set; }

        public ushort[] GlyphIds { get; set; }


        public override void Process(TrueTypeReader reader, uint offset)
        {
            base.Process(reader, offset);



        }
    }
}
