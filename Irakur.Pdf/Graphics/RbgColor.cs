using System;
using System.Collections.Generic;
using System.Text;

namespace Irakur.Pdf.Graphics
{
    public struct RgbColor
    {
        public byte Red;
        public byte Green;
        public byte Blue;

        public RgbColor(byte red, byte green, byte blue)
        {
            this.Red = red;
            this.Green = green;
            this.Blue = blue;
        }

        public RgbColor(byte grayShade)
        {
            this.Red = grayShade;
            this.Green = grayShade;
            this.Blue = grayShade;
        }
    }
}
