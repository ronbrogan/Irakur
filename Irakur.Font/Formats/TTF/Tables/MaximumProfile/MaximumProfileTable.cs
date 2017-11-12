using System;
using System.Collections.Generic;
using System.Text;

namespace Irakur.Font.Formats.TTF.Tables.MaximumProfile
{
    public class MaximumProfileTable : FontTableBase
    {
        public override FontTableType Type => FontTableType.MaximumProfile;

        public byte[] Version { get; set; }
        public ushort NumberOfGlyphs { get; set; }

        public override void Process(TrueTypeFont font)
        {
            var reader = new TrueTypeReader(Data);

            Version = reader.ReadFixed();
            NumberOfGlyphs = reader.ReadUShort();

            reader.Dispose();
        }
    }
}
