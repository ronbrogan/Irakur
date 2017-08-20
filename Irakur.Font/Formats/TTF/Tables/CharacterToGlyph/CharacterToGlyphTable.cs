using Irakur.Core.Attributes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Irakur.Font.Formats.TTF.Tables.CharacterToGlyph
{
    public class CharacterToGlyphTable : TableBase
    {
        private TrueTypeReader reader { get; set; }

        public ushort Version { get; set; }
        public ushort EncodingTableCount { get; set; }
        private List<EncodingTableEntry> EncodingTables { get; set; }


        public override void Process(Font font)
        {
            reader = new TrueTypeReader(Data, true);

            Version = reader.ReadUShort();
            EncodingTableCount = reader.ReadUShort();

            EncodingTables = new List<EncodingTableEntry>(EncodingTableCount);
            for (var i = 0; i < EncodingTableCount; i++)
            {
                EncodingTables.Add(new EncodingTableEntry()
                {
                    Platform = (Platform)reader.ReadUShort(),
                    Encoding = (Encoding)reader.ReadUShort(),
                    Offset = reader.ReadULong()
                });
            }

            foreach(var encodingTable in EncodingTables)
            {
                reader.Seek(encodingTable.Offset);

                var subType = (SubtableFormat)reader.ReadUShort();

                var subtableProcessorType = ImplementationTypeAttribute.GetTypeFromEnumValue(typeof(SubtableFormat), subType);

                if (subtableProcessorType == null)
                    continue;

                var subtableProcessor = Activator.CreateInstance(subtableProcessorType) as CharacterToGlyphSubtableBase;

                subtableProcessor.Process(reader, encodingTable.Offset);
            }

        }

        protected override void Dispose(bool disposing)
        {
            reader?.Dispose();

            base.Dispose(disposing);
        }
    }
}
