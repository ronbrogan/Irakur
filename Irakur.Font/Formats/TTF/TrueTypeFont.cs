using Irakur.Core.Attributes;
using Irakur.Core.Extensions;
using Irakur.Font;
using Irakur.Font.Formats.TTF.Tables;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Irakur.Font.Formats.TTF
{
    internal class TrueTypeFont : IFontLoader, IDisposable
    {
        private TrueTypeReader reader { get; }

        private byte[] Version { get; set; }

        /*
         * TTF Specific Properties 
         * */
        private ushort NumberOfTables { get; set; }
        private ushort SearchRange { get; set; }
        private ushort EntrySelector { get; set; }
        private ushort RangeShift { get; set; }
        private List<IFontTable> Tables { get; set; }


        public TrueTypeFont(Stream fontStream)
        {
            reader = new TrueTypeReader(fontStream);
        }

        public Font GenerateFont()
        {
            var font = new Font();

            font.Version = GetVersion();
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
                // Read data from file
                table.ReadData(reader);
            }

            foreach (var table in Tables)
            {
                // Call each table's process method
                table.Process(font);
            }

            return font;
        }

        private string GetVersion()
        {
            reader.Seek(0);

            var versionBuf = reader.ReadFixed();

            return BitConverter.ToString(versionBuf).Replace("-", "");
        }

        private void ProcessTable(TableBase table, Font font)
        {
            switch(table.Type)
            {


                default:
                    break;
            }


        }

        private void ProcessCharacterToGlyphMap(TableBase table, Font font)
        {

        }

        #region Disposable

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if(disposing)
            {
                reader.Dispose();
            }
        }

        #endregion
    }
}
