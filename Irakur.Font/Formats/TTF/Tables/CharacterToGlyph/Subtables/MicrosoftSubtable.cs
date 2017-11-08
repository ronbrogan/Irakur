using System;
using System.Collections.Generic;
using System.Text;

namespace Irakur.Font.Formats.TTF.Tables.CharacterToGlyph
{
    public class MicrosoftSubtable : CharacterToGlyphSubtableBase
    {
        public ushort DoubleSegCount { get; set; }
        public ushort SegCount => (ushort)(DoubleSegCount / 2);
        public ushort SearchRange { get; set; }
        public ushort EntrySelector { get; set; }
        public ushort RangeShift { get; set; }
        public ushort[] EndCounts { get; set; }
        public ushort ReservedPad { get; set; }
        public ushort[] StartCounts { get; set; }
        public ushort[] IdDeltas { get; set; }
        public ushort[] IdRangeOffsets { get; set; }
        public long[] IdRangeOffsetPositions { get; set; }

        public override void Process(TrueTypeReader reader, uint offset)
        {
            base.Process(reader, offset);

            DoubleSegCount = reader.ReadUShort();
            SearchRange = reader.ReadUShort();
            EntrySelector = reader.ReadUShort();
            RangeShift = reader.ReadUShort();

            EndCounts = new ushort[SegCount];
            for(var i = 0; i < SegCount; i++)
            {
                EndCounts[i] = reader.ReadUShort();
            }

            ReservedPad = reader.ReadUShort();

            StartCounts = new ushort[SegCount];
            for (var i = 0; i < SegCount; i++)
            {
                StartCounts[i] = reader.ReadUShort();
            }

            IdDeltas = new ushort[SegCount];
            for (var i = 0; i < SegCount; i++)
            {
                IdDeltas[i] = reader.ReadUShort();
            }

            IdRangeOffsets = new ushort[SegCount];
            IdRangeOffsetPositions = new long[SegCount];
            for (var i = 0; i < SegCount; i++)
            {
                IdRangeOffsetPositions[i] = reader.Position - offset;
                IdRangeOffsets[i] = reader.ReadUShort();
            }
        }

        public override ushort GetGlyphId(ushort charCode)
        {
            ushort glyphId = 0;

            /// You search for the first endCode that is greater than or equal to the character code you want to map. 
            /// If the corresponding startCode is less than or equal to the character code, then you use the corresponding
            /// idDelta and idRangeOffset to map the character code to a glyph index (otherwise, the missingGlyph is returned).

            ushort endCode = 0;
            int segment = 0;

            while (true)
            {
                endCode = EndCounts[segment];

                if (endCode >= charCode)
                {
                    break;
                }
                else
                {
                    segment++;
                }
            }

            var startCode = StartCounts[segment];

            if(startCode <= charCode)
            {
                // use idDelta and idRangeOffset to map charcode to glyphindex

                var idDelta = IdDeltas[segment];
                var idRangeOffset = IdRangeOffsets[segment];

                if(idRangeOffset == 0)
                {
                    unchecked
                    {
                        glyphId = (ushort)(idDelta + charCode);
                    }
                }
                else
                {
                    // use glyphIds data
                    var idStreamPosition = idRangeOffset + 2 * (charCode - startCode) + IdRangeOffsetPositions[segment];
                    ushort readGlyphValue;
                    using (var reader = new TrueTypeReader(base.Data))
                    {
                        reader.Seek(idStreamPosition);
                        readGlyphValue = reader.ReadUShort();
                    }
                        
                    if (readGlyphValue != 0)
                    {
                        unchecked
                        {
                            glyphId = (ushort)(idDelta + readGlyphValue);
                        }
                    }
                }
            }

            return glyphId;
        }
    }
}
