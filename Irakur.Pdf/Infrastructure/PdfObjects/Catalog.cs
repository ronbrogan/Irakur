using Irakur.Pdf.Infrastructure.Serialization;
using System;
using System.Collections.Generic;
using System.Text;

namespace Irakur.Pdf.Infrastructure.PdfObjects
{
    public class Catalog : PdfObject
    {
        public override PdfObjectType Type => PdfObjectType.Catalog;
        public PageNode Pages { get; set; }

        public Catalog() : base(true)
        {
        }
        
        public override IEnumerable<IPdfObject> GetChildren()
        {
            yield return Pages;
        }
    }
}
