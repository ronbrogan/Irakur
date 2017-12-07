using Irakur.Core.CoordinateSystem;
using Irakur.Pdf.Graphics;
using Irakur.Pdf.Infrastructure.PdfObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Irakur.Pdf
{
    public class TextContent
    {
        public string Text { get; set; }

        public Rectangle Rectangle { get; set; }

        public Font Font { get; set; }

        public int FontSize = 12;

        public RgbColor FillColor = new RgbColor(0);
    }
}
