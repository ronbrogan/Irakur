using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Irakur.Font.Formats.TTF
{
    public struct Table 
    {
        public TableType Type { get; set; }

        public uint Checksum { get; set; }

        public uint Offset { get; set; }

        public uint Length { get; set; }

        public byte[] Data { get; set; }

        /// <summary>
        /// Populates the <see cref="Data"/> field from the provided reader
        /// </summary>
        public void ReadData(TrueTypeReader reader)
        {
            Data = new byte[Length];

            reader.Seek(Offset);

            reader.Read(Data, 0, (int)Length);
        }
    }
}
