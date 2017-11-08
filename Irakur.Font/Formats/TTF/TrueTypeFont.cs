using Irakur.Core.Attributes;
using Irakur.Core.Extensions;
using Irakur.Font;
using Irakur.Font.Formats.TTF.Tables;
using Irakur.Font.Formats.TTF.Tables.CharacterToGlyph;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Irakur.Font.Formats.TTF
{
    internal class TrueTypeFont : IFont
    {
        public FontFormat Format => FontFormat.TTF;
        public string Name { get; set; }
        public string Version { get; set; }

        /*
         * TTF Specific Properties 
         * */
        private ushort NumberOfTables { get; set; }
        private ushort SearchRange { get; set; }
        private ushort EntrySelector { get; set; }
        private ushort RangeShift { get; set; }
        private List<IFontTable> Tables { get; set; }

        /*
         * Tables
         * */
        internal CharacterToGlyphTable cmap { get; set; }
        

        public TrueTypeFont(Stream fontStream)
        {
            var reader = new TrueTypeReader(fontStream);

            Version = GetVersion(reader);
            this.NumberOfTables = reader.ReadUShort();
            this.SearchRange = reader.ReadUShort();
            this.EntrySelector = reader.ReadUShort();
            this.RangeShift = reader.ReadUShort();

            Tables = new List<IFontTable>(this.NumberOfTables);

            for (var i = 0; i < this.NumberOfTables; i++)
            {
                // Need to read each every time to ensure that the stream
                // is always seeked to the next table entry, fragile
                var type = (FontTableType)reader.ReadULong();
                var checksum = reader.ReadULong();
                var offset = reader.ReadULong();
                var length = reader.ReadULong();

                var tableType = ImplementationTypeAttribute.GetTypeFromEnumValue(typeof(FontTableType), type);

                if (tableType == null)
                    continue;

                var table = Activator.CreateInstance(tableType) as IFontTable;

                table.Type = type;
                table.Checksum = checksum;
                table.Offset = offset;
                table.Length = length;

                Tables.Add(table);
            }

            foreach (var table in Tables)
            {
                // Read data from stream into Table's internal buffer
                table.ReadData(reader);
            }

            reader.Dispose();

            ProcessTables();
        }

        private void ProcessTables()
        {
            foreach (var table in Tables)
            {
                // Call each table's process method to parse data
                table.Process();

                switch(table.Type)
                {
                    case FontTableType.CharacterToGlyphMap:
                        cmap = (CharacterToGlyphTable)table;
                        break;
                }
            }
        }

        private string GetVersion(TrueTypeReader reader)
        {
            reader.Seek(0);

            var versionBuf = reader.ReadFixed();

            return BitConverter.ToString(versionBuf).Replace("-", "");
        }

        public float GetPointWidthOfChar(char c)
        {
            var glyphId = cmap.GetGlyphId((ushort)c);

            return glyphId;
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    foreach(var table in Tables)
                    {
                        table.Dispose();
                    }
                }

                // TODO: set large fields to null.

                disposedValue = true;
            }
        }


        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
        }
        #endregion
    }
}
