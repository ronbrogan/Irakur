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
        internal PdfTrailer(IndirectReference root, int size)
        {
            Root = root;
            Size = size;
        }

        internal IndirectReference Root { get; set; }

        internal long Size { get; set; }
    }
}
