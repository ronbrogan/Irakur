using System.Collections.Generic;
using Irakur.Core.CoordinateSystem;
using Irakur.Pdf.Infrastructure.Serialization;

namespace Irakur.Pdf.Infrastructure.PdfObjects
{
    public class Page : PdfObject
    {
        public override PdfObjectType Type => PdfObjectType.Page;
        public PageNode Parent { get; set; }
        public Resources Resources { get; set; }
        public Rectangle MediaBox { get; set; }
        public ContentStream Contents { get; set; }

        public Page() : base(true)
        {
            MediaBox = new Rectangle(0, 0, 612, 792);
        }

        public override IEnumerable<IPdfObject> GetChildren()
        {
            yield return Resources;
            yield return Contents;
        }
    }
}
