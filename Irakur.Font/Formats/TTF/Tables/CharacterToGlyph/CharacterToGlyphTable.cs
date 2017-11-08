using Irakur.Core.Attributes;
using Irakur.Font.Formats.TTF.Tables.CharacterToGlyph.Subtables;
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
        private Dictionary<ushort, ushort> GlyphIds { get; set; }

        public List<ICharacterToGlyphSubtable> Subtables { get; set; }

        public CharacterToGlyphTable()
        {
            GlyphIds = new Dictionary<ushort, ushort>();
            Subtables = new List<ICharacterToGlyphSubtable>();
        }

        public ushort GetGlyphId(ushort charCode)
        {
            if (GlyphIds.ContainsKey(charCode))
                return GlyphIds[charCode];

            ushort glyphId = 0;

            foreach(var subtable in Subtables)
            {
                var retrievedValue = subtable.GetGlyphId(charCode);

                if(retrievedValue != 0)
                {
                    glyphId = retrievedValue;
                    break;
                }
            }

            GlyphIds[charCode] = glyphId;

            return glyphId;
        }

        public override void Process()
        {
            reader = new TrueTypeReader(Data);

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

                var subtableProcessor = Activator.CreateInstance(subtableProcessorType) as ICharacterToGlyphSubtable;

                Subtables.Add(subtableProcessor);

                subtableProcessor.Process(reader, encodingTable.Offset);
            }
        }

        protected override void Dispose(bool disposing)
        {
            reader?.Dispose();

            foreach(var table in Subtables)
            {
                table.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
