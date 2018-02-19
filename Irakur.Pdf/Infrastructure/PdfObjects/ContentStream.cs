using System;
using System.Collections.Generic;
using System.Text;

namespace Irakur.Pdf.Infrastructure.PdfObjects
{
    public class ContentStream : PdfObject
    {
        public override PdfObjectType Type => PdfObjectType.ContentStream;
        public Page Parent { get; set; }
        public List<TextContent> TextContent { get; set; } = new List<TextContent>();
        public List<ImageContent> ImageContent { get; set; } = new List<ImageContent>();

        public ContentStream() : base(true)
        {
        }

        public override IEnumerable<IPdfObject> GetChildren()
        {
            return null;
        }
    }
}
