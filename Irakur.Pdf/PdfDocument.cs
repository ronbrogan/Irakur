using Irakur.Core.CoordinateSystem;
using Irakur.Pdf.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;

namespace Irakur.Pdf
{
    public class PdfDocument
    {
        public List<PdfPage> Pages { get; set; }

        public PdfDocument(Size size)
        {
            Pages = new List<PdfPage>();
        }

        public void Save(Stream stream)
        {
            new UnderlyingPdf(this).Flush(stream);
        }
    }
}
