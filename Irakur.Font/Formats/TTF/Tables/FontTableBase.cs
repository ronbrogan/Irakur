using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Irakur.Font.Formats.TTF.Tables
{
    public abstract class TableBase : IFontTable, IDisposable
    {
        public FontTableType Type { get; set; }

        public uint Checksum { get; set; }

        public uint Offset { get; set; }

        public uint Length { get; set; }

        public virtual MemoryStream Data { get; set; }

        public virtual void Process(Font font)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Populates the <see cref="Data"/> field from the provided reader
        /// </summary>
        public virtual void ReadData(TrueTypeReader reader)
        {
            var buf = new byte[Length];

            reader.Seek(Offset);

            reader.Read(buf, 0, (int)Length);

            Array.Reverse(buf);

            Data = new MemoryStream(buf);
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
                    Data?.Dispose();
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
