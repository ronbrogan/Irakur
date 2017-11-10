namespace Irakur.Font.Formats.TTF.Tables
{
    public abstract class FontTableBase : FontTableBase<FontTableType>
    {
        public FontTableHeaderEntry HeaderEntry { get; set; }
    }

    public abstract class FontTableBase<TTypeEnum> : IFontTable<TTypeEnum>
    {
        public abstract TTypeEnum Type { get; }

        public uint Checksum { get; set; }

        protected byte[] Data { get; set; }

        public abstract void Process(TrueTypeFont font);

        /// <summary>
        /// Populates the <see cref="Data"/> field from the provided reader.
        /// </summary>
        public virtual void ReadData(TrueTypeReader reader, uint offset, uint length)
        {
            Data = new byte[length];

            reader.Seek(offset);

            reader.ReadRaw(Data, 0, (int)length);
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
