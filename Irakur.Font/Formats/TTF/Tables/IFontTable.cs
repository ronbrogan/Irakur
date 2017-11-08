using System;

namespace Irakur.Font.Formats.TTF.Tables
{
    public interface IFontTable : IDisposable
    {
        FontTableType Type { get; set; }

        uint Checksum { get; set; }

        uint Offset { get; set; }

        uint Length { get; set; }

        void ReadData(TrueTypeReader reader);

        void Process();
    }
}
