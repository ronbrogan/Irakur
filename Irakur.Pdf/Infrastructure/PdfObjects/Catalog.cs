using System;
using System.Collections.Generic;
using System.Text;

namespace Irakur.Pdf.Infrastructure.PdfObjects
{
    public class Catalog : PdfObject
    {
        public Catalog(UnderlyingPdf doc) : base(doc)
        {
        }

        public override PdfObjectType Type => PdfObjectType.Catalog;

        public PageNode Pages { get; set; }
    }
}
