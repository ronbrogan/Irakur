using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Irakur.Font.Formats.TTF.Tables
{
    public class CharacterToGlyphTable : TableBase
    {
        private TrueTypeReader reader { get; set; }

        public ushort Version { get; set; }
        public ushort EncodingTableCount { get; set; }


        public override void Process(Font font)
        {
            reader = new TrueTypeReader(Data, true);

            Version = reader.ReadUShort();
            EncodingTableCount = reader.ReadUShort();
        }

        protected override void Dispose(bool disposing)
        {
            reader?.Dispose();

            base.Dispose(disposing);
        }
    }
}
