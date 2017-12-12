using System;
using System.Collections.Generic;
using System.Text;

namespace Irakur.Pdf.Infrastructure.PdfObjects
{
    public class PageNode : PdfObject
    {
        public PageNode() : base(true)
        {
            Kids = new List<Page>();
        }

        public override PdfObjectType Type => PdfObjectType.Pages;

        public IPdfObject Parent { get; set; }

        public List<Page> Kids { get; set; }

        public int Count => Kids.Count;
    }
}
