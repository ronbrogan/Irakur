using Irakur.Core.Attributes;
using System;
using System.Collections.Generic;

namespace Irakur.Font.Formats.TTF.Tables.CharacterToGlyph
{
    public class CharacterToGlyphTable : FontTableBase
    {
        public ushort Version { get; set; }
        public ushort EncodingTableCount { get; set; }
        private List<EncodingTableEntry> EncodingTables { get; set; }
        private Dictionary<ushort, ushort> GlyphIds { get; set; }

        public List<CharacterToGlyphSubtableBase> Subtables { get; set; }

        public CharacterToGlyphTable()
        {
            GlyphIds = new Dictionary<ushort, ushort>();
            Subtables = new List<CharacterToGlyphSubtableBase>();
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

        public override void Process(TrueTypeFont font)
        {
            var reader = new TrueTypeReader(Data);

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

                var format = (CmapSubtableType)reader.ReadUShort();
                var length = reader.ReadUShort();

                var subtableProcessorType = ImplementationTypeAttribute.GetTypeFromEnumValue(typeof(CmapSubtableType), format);

                if (subtableProcessorType == null)
                    continue;

                var subtableProcessor = Activator.CreateInstance(subtableProcessorType) as CharacterToGlyphSubtableBase;
                subtableProcessor.Type = format;
                subtableProcessor.Length = length;
                subtableProcessor.Offset = encodingTable.Offset;
                subtableProcessor.EncodingTableEntry = encodingTable;
                subtableProcessor.ReadData(reader);
                Subtables.Add(subtableProcessor);

                subtableProcessor.ReadData(reader);
            }

            ProcessSubtables(font);

            reader.Dispose();
        }

        private void ProcessSubtables(TrueTypeFont font)
        {
            foreach (var subtable in Subtables)
                subtable.Process(font);
        }

        protected override void Dispose(bool disposing)
        {
            GlyphIds = null;

            foreach(var table in Subtables)
            {
                table.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
