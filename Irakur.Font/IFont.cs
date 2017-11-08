using System;

namespace Irakur.Font
{
    public interface IFont : IDisposable
    {
        FontFormat Format { get; }

        string Name { get; set; }

        string Version { get; set; }

        float GetPointWidthOfChar(char c);
    }
}
