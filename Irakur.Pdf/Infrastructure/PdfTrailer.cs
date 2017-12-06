using Irakur.Pdf.Infrastructure.PdfObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Irakur.Pdf.Infrastructure
{
    public class PdfTrailer
    {
        public PdfTrailer(UnderlyingPdf doc)
        {
            Root = doc.Catalog;
            Size = doc.indirectObjects.Count;
        }

        public Catalog Root { get; set; }

        public long Size { get; set; }
    }
}
