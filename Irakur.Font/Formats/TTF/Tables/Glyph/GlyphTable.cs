using System;
using System.Collections.Generic;
using System.Text;

namespace Irakur.Font.Formats.TTF.Tables.Glyph
{
    public class GlyphTable : FontTableBase
    {
        public override FontTableType Type => FontTableType.GlyphData;

        private TrueTypeFont font { get; set; }

        private Dictionary<ushort, Glyph> GlyphCache { get; set; }

        public GlyphTable()
        {
            GlyphCache = new Dictionary<ushort, Glyph>();
        }

        public override void Process(TrueTypeFont font)
        {
            this.font = font;

            // noop
            // This table is merely a data holder to access the raw data according to loca lookup
        }

        public Glyph GetGlyphData(ushort glyphId)
        {
            if (GlyphCache.ContainsKey(glyphId))
                return GlyphCache[glyphId];

            var dataOffset = font.loca.Offsets[glyphId];

            var reader = new TrueTypeReader(Data);
            reader.Seek(dataOffset);

            var glyph = new Glyph()
            {
                NumberOfContours = reader.ReadShort(),
                XMin = reader.ReadFWord(),
                YMin = reader.ReadFWord(),
                XMax = reader.ReadFWord(),
                YMax = reader.ReadFWord()
            };

            reader.Dispose();

            GlyphCache[glyphId] = glyph;

            return glyph;
        }
    }
}
