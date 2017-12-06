using System;
using System.Collections.Generic;
using System.Text;

namespace Irakur.Pdf.Infrastructure.Text
{
    public enum TextRenderingMode
    {
        Fill = 0,
        Stroke = 1,
        FillAndStroke = 2,
        None = 3,
        FillAndClip = 4,
        StrokeAndClip = 5,
        FillAndStrokeAndClip = 6,
        ClipOnly = 7
    }
}
