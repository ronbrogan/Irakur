using Irakur.Pdf.Infrastructure.Serialization;
using System;
using System.Collections.Generic;
using System.Text;

namespace Irakur.Pdf.Infrastructure.PdfObjects
{
    public class PageNode : PdfObject
    {
        public override PdfObjectType Type => PdfObjectType.Pages;
        public Catalog ParentCatalog { get; set; }
        public PageNode ParentNode { get; set; }
        public List<Page> Kids { get; set; }
        public int Count => Kids.Count;

        public PageNode() : base(true)
        {
            Kids = new List<Page>();
        }

        public override IEnumerable<IPdfObject> GetChildren()
        {
            return Kids;
        }
    }
}
