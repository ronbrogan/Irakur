using Irakur.Font.Formats.TTF.Tables.CharacterToGlyph.Subtables;
using System;
using System.Collections.Generic;
using System.Text;

namespace Irakur.Font.Formats.TTF.Tables.CharacterToGlyph
{
    public abstract class CharacterToGlyphSubtableBase : ICharacterToGlyphSubtable, IDisposable
    {
        protected byte[] Data { get; set; }

        public ushort Format { get; set; }
        public ushort Length { get; set; }
        public ushort Version { get; set; }

        public abstract ushort GetGlyphId(ushort charCode);

        /// <summary>
        /// Populates the Format, Length, Data, and Version fields, when overriding, make sure to 
        /// call this base method first, or manually seek the reader yourself.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="offset"></param>
        public virtual void Process(TrueTypeReader reader, uint offset)
        {
            reader.Seek(offset);

            Format = reader.ReadUShort();
            Length = reader.ReadUShort();

            var resumeOffset = reader.Position;

            Data = new byte[Length];

            reader.Seek(offset);

            reader.ReadRaw(Data, 0, Length);

            reader.Seek(resumeOffset);

            Version = reader.ReadUShort();
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
