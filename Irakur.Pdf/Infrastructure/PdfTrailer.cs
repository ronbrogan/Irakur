using Irakur.Pdf.Infrastructure.Collections;
using Irakur.Pdf.Infrastructure.Core;
using Irakur.Pdf.Infrastructure.PdfObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Irakur.Pdf.Infrastructure
{
    internal class PdfTrailer
    {
        internal PdfTrailer(IndirectReference root, XrefTable xrefs)
        {
            Root = root;
            Size = xrefs.Entries.Count;
        }

        internal IndirectReference Root { get; set; }

        internal long Size { get; set; }
    }
}
