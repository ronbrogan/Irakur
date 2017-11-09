using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Irakur.Font.Formats.TTF.Tables
{
    public abstract class FontTableBase : FontTableBase<FontTableType>
    {

    }

    public abstract class FontTableBase<TTypeEnum> : IFontTable<TTypeEnum>
    {
        public TTypeEnum Type { get; set; }

        public uint Checksum { get; set; }

        public uint Offset { get; set; }

        public uint Length { get; set; }

        protected byte[] Data { get; set; }

        public abstract void Process(TrueTypeFont font);

        /// <summary>
        /// Populates the <see cref="Data"/> field from the provided reader. Make sure to preload <see cref="Offset"/> and <see cref="Length"/>.
        /// </summary>
        public virtual void ReadData(TrueTypeReader reader)
        {
            Data = new byte[Length];

            reader.Seek(Offset);

            reader.ReadRaw(Data, 0, (int)Length);
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
                }

                // TODO: set large fields to null.
                Data = null;

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
