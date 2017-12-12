using Irakur.Pdf.Infrastructure.PdfObjects;
using Irakur.Pdf.Text;
using System;
using System.Collections.Generic;
using System.Text;

namespace Irakur.Pdf.Infrastructure.Text
{
    public class FontFactory
    {
        public static Font Standard(StandardFont font)
        {
            return new Font()
            {
                Subtype = PdfFontType.Type1,
                BaseFont = new Name(font)
            };
        }


    }
}
