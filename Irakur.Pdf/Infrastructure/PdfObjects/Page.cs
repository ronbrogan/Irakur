using System.Collections.Generic;
using Irakur.Core.CoordinateSystem;

namespace Irakur.Pdf.Infrastructure.PdfObjects
{
    public class Page : PdfObject
    {
        public Page(UnderlyingPdf doc) : base(doc)
        {
            MediaBox = new Rectangle(0, 0, 612, 792);
        }

        public override PdfObjectType Type => PdfObjectType.Page;

        public PageNode Parent { get; set; }

        public Resources Resources { get; set; }

        public Rectangle MediaBox { get; set; }

        public ContentStream Contents { get; set; }
    }
}
