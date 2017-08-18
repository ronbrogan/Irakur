using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Irakur.Font.Formats.TTF
{
    public class TrueTypeReader : IDisposable
    {
        private Stream BaseStream { get; }

        /* 
         * TTF Layout
         */
        private const int ByteBytes = 1;
        private const int CharBytes = 1;
        private const int UShortBytes = 2;
        private const int ShortBytes = 2;
        private const int ULongBytes = 4;
        private const int LongBytes = 4;
        private const int FixedBytes = 4;
        private const int FWordBytes = ShortBytes;
        private const int UFWordBytes = UShortBytes;


        public TrueTypeReader(Stream input)
        {
            this.BaseStream = input;
        }

        public int Read(byte[] buffer, int offset, int count)
        {
            var bytesRead = BaseStream.Read(buffer, offset, count);

            Array.Reverse(buffer);

            return bytesRead;
        }

        public long Seek(long offset)
        {
            return BaseStream.Seek(offset, SeekOrigin.Begin);
        }

        public byte[] ReadFixed()
        {
            var rawData = new byte[FixedBytes];

            Read(rawData, 0, FixedBytes);

            return rawData;
        }

        public ushort ReadUShort()
        {
            var rawData = new byte[UShortBytes];

            Read(rawData, 0, UShortBytes);

            return BitConverter.ToUInt16(rawData, 0);
        }

        public short ReadShort()
        {
            var rawData = new byte[ShortBytes];

            Read(rawData, 0, ShortBytes);

            return BitConverter.ToInt16(rawData, 0);
        }

        public uint ReadULong()
        {
            var rawData = new byte[ULongBytes];

            Read(rawData, 0, ULongBytes);

            return BitConverter.ToUInt32(rawData, 0);
        }

        public int ReadLong()
        {
            var rawData = new byte[LongBytes];

            Read(rawData, 0, LongBytes);

            return BitConverter.ToInt32(rawData, 0);
        }


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if(disposing)
            {

                BaseStream.Dispose();
            }
        }
    }
}
