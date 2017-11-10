using System;

namespace Irakur.Font.Formats.TTF.Tables
{
    public interface IFontTable<TTypeEnum> : IDisposable
    {
        TTypeEnum Type { get; }

        uint Checksum { get; set; }

        void ReadData(TrueTypeReader reader, uint offset, uint length);

        void Process(TrueTypeFont font);
    }
}
