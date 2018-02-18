using Irakur.Pdf.Infrastructure.PdfObjects;
using Irakur.Pdf.Text;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Irakur.Pdf.Infrastructure.Text
{
    public class FontFactory
    {
        private static ConcurrentDictionary<StandardFont, Font> StandardFontCache = new ConcurrentDictionary<StandardFont, Font>();

        public static Font Standard(StandardFont font)
        {
            if (StandardFontCache.ContainsKey(font))
                return StandardFontCache[font];

            var fontInstance = new Font()
            {
                Subtype = PdfFontType.Type1,
                BaseFont = new Name(font.ToString())
            };

            StandardFontCache.TryAdd(font, fontInstance);

            return fontInstance;
        }
    }
}
