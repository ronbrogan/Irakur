using Irakur.Font;
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

        private Table[] Tables { get; set; }



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

            Tables = new Table[this.NumberOfTables];

            for (var i = 0; i < this.NumberOfTables; i++)
            {
                Tables[i] = new Table()
                {
                    Type = (TableType)reader.ReadULong(),
                    Checksum = reader.ReadULong(),
                    Offset = reader.ReadULong(),
                    Length = reader.ReadULong()
                };
            }

            for (var i = 0; i < Tables.Length; i++)
            {
                Tables[i].ReadData(reader);
            }

            return font;
        }

        public string GetVersion()
        {
            reader.Seek(0);

            var versionBuf = reader.ReadFixed();

            return BitConverter.ToString(versionBuf).Replace("-", "");
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
