using System;
using System.Collections.Generic;
using System.Text;

namespace Irakur.Pdf.Infrastructure.PdfObjects
{
    public class ContentStream : PdfObject
    {
        public ContentStream(UnderlyingPdf doc) : base(doc)
        {
        }

        public Page Parent { get; set; }

        public object Data { get; set; }

        public override PdfObjectType Type => PdfObjectType.ContentStream;
    }
}
