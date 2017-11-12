using System;

namespace Irakur.Font
{
    public interface IFont : IDisposable
    {
        FontFormat Format { get; }

        string Name { get; set; }

        string Version { get; set; }

        ushort GetUnitWidthOfChar(char c);

        ushort GetUnitWidthOfString(string s);

        float GetEmWidthOfString(string s, float em);

        float GetPointWidthOfString(string s, float point);

        float GetPixelWidthOfString(string s, float point);

        float UnitsToEm(uint units);
    }
}
