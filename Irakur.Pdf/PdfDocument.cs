using Irakur.Core.CoordinateSystem;
using Irakur.Pdf.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;

namespace Irakur.Pdf
{
    public class PdfDocument
    {
        private UnderlyingPdf underlyingPdf;

        public List<PdfPage> Pages { get; set; }


        public PdfDocument(Size size)
        {
            underlyingPdf = new UnderlyingPdf();
            Pages = new List<PdfPage>();
        }

        public void Save(Stream stream)
        {
            ProcessPages();
            underlyingPdf.Flush(stream);
        }

        private void ProcessPages()
        {
            underlyingPdf.ProcessPages(Pages);
        }
    }
}
