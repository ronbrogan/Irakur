using System;
using System.Collections.Generic;
using System.Text;

namespace Irakur.Pdf.Infrastructure.PdfObjects
{
    public class ContentStream : PdfObject
    {
        public override PdfObjectType Type => PdfObjectType.ContentStream;

        public Page Parent { get; set; }

        public List<TextContent> TextContent { get; set; }

        public ContentStream(UnderlyingPdf doc) : base(doc)
        {
            this.TextContent = new List<TextContent>();
        }
    }
}
