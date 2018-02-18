using Irakur.Pdf.Infrastructure.Text;
using System;
using System.Collections.Generic;
using System.Text;

namespace Irakur.Pdf.Infrastructure.PdfObjects
{
    public class Font : PdfObject
    {
        public override PdfObjectType Type => PdfObjectType.Font;

        public PdfFontType Subtype { get; set; }

        public Name BaseFont { get; set; }

        public Font() : base(true)
        {

        }

        public override IEnumerable<IPdfObject> GetChildren()
        {
            yield break;
        }
    }
}
