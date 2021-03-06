﻿using System;
using System.IO;

namespace Irakur.Font.Formats.TTF
{
    public class TrueTypeReader : IDisposable
    {
        private Stream BaseStream { get; }
        private bool LeaveBaseStreamOpen = false;

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
        private const int LongDateTimeBytes = 8;


        public TrueTypeReader(Stream input)
        {
            this.BaseStream = input;
        }

        public TrueTypeReader(Stream input, bool leaveOpen)
        {
            this.BaseStream = input;
            LeaveBaseStreamOpen = leaveOpen;
        }

        public TrueTypeReader(byte[] data)
        {
            BaseStream = new MemoryStream(data);
        }

        public int Read(byte[] buffer, int offset, int count)
        {
            var bytesRead = BaseStream.Read(buffer, offset, count);

            Array.Reverse(buffer);

            return bytesRead;
        }

        /// <summary>
        /// This method calls the underlying stream's Read method without correcting endianness before returning. 
        /// Use this method when you need to create a new TrueTypeReader with the raw data returned.
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public int ReadRaw(byte[] buffer, int offset, int count)
        {
            return BaseStream.Read(buffer, offset, count);
        }

        public long Seek(long offset)
        {
            return BaseStream.Seek(offset, SeekOrigin.Begin);
        }

        public long Position => BaseStream.Position;

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

        /// <summary>
        /// Gets the quantity of FUnits
        /// </summary>
        /// <returns>16-bit signed int (short) indicating quantity of FUnits</returns>
        public short ReadFWord()
        {
            var rawData = new byte[FWordBytes];

            Read(rawData, 0, FWordBytes);

            return BitConverter.ToInt16(rawData, 0);
        }

        /// <summary>
        /// Gets the quantity of FUnits
        /// </summary>
        /// <returns>16-bit unsigned int (ushort) indicating quantity of FUnits</returns>
        public ushort ReadUFWord()
        {
            var rawData = new byte[UFWordBytes];

            Read(rawData, 0, UFWordBytes);

            return BitConverter.ToUInt16(rawData, 0);
        }

        public DateTime ReadLongDateTime()
        {
            var rawData = new byte[LongDateTimeBytes];

            Read(rawData, 0, LongDateTimeBytes);

            var maxEpoch = new DateTime(1904, 1, 1);

            var span = TimeSpan.FromSeconds(BitConverter.ToInt64(rawData, 0));

            return maxEpoch.Add(span);
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
                if(!LeaveBaseStreamOpen)
                    BaseStream?.Dispose();
            }
        }
    }
}
