using System;
using System.Collections.Generic;
using System.Text;

namespace Irakur.Font.Formats.TTF.Tables.IndexToLocation
{
    public class IndexToLocationTable : FontTableBase
    {
        public override FontTableType Type => FontTableType.IndexToLocation;

        public Dictionary<ushort, uint> Offsets { get; set; }

        public override void Process(TrueTypeFont font)
        {
            var offsetFormat = font.head.IndexToLocFormat;
            var numberOfGlyphs = Data.Length / ((offsetFormat + 1) * 2);
            
            Offsets = new Dictionary<ushort, uint>(numberOfGlyphs);

            var reader = new TrueTypeReader(Data);

            for(ushort i = 0; i < numberOfGlyphs; i++)
            {
                if((LocationOffsetFormat)offsetFormat == LocationOffsetFormat.Short)
                {
                    Offsets[i] = reader.ReadUShort();
                }
                else
                {
                    Offsets[i] = reader.ReadULong();
                }
            }

            reader.Dispose();
        }
    }
}
