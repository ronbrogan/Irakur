using System;

namespace Irakur.Font
{
    public interface IFont : IDisposable
    {
        FontFormat Format { get; }

        string Name { get; set; }

        string Version { get; set; }

        ushort GetUnitWidthOfChar(char c);
    }
}
