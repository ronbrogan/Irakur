using Irakur.Core.Attributes;
using Irakur.Core.Extensions;
using Irakur.Font;
using Irakur.Font.Formats.TTF.Tables;
using Irakur.Font.Formats.TTF.Tables.CharacterToGlyph;
using Irakur.Font.Formats.TTF.Tables.Header;
using Irakur.Font.Formats.TTF.Tables.HorizontalHeader;
using Irakur.Font.Formats.TTF.Tables.HorizontalMetrics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Irakur.Font.Formats.TTF
{
    public class TrueTypeFont : IFont
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
        internal HeaderTable head { get; set; }
        internal HorizontalHeaderTable hhea { get; set; }
        internal HorizontalMetricsTable hmtx { get; set; }


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
            // TODO: These may need to be Processed() in a certain order, for example the horizontal metrics table
            // 'needs' to know the number of metrics from the horizontal header table.
            // However, it looks like the tables are listed in the proper order, so we get that for free. But that seems awfully fragile....
            foreach (var table in Tables)
            {
                // Call each table's process method to parse data
                table.Process(this);

                switch(table.Type)
                {
                    case FontTableType.CharacterToGlyphMap:
                        cmap = (CharacterToGlyphTable)table;
                        break;

                    case FontTableType.FontHeader:
                        head = (HeaderTable)table;
                        break;

                    case FontTableType.HorizontalHeader:
                        hhea = (HorizontalHeaderTable)table;
                        break;

                    case FontTableType.HorizontalMetrics:
                        hmtx = (HorizontalMetricsTable)table;
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

        public ushort GetUnitWidthOfChar(char c)
        {
            var glyphId = cmap.GetGlyphId((ushort)c);

            var metric = hmtx.Metrics[glyphId];

            return metric.AdvanceWidth;
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
