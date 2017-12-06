using System;
using System.Collections.Generic;
using System.Text;

namespace Irakur.Pdf.Infrastructure.PdfObjects
{
    public class Font : PdfObject
    {
        public override PdfObjectType Type => PdfObjectType.Font;

        public Font(UnderlyingPdf doc) : base(doc)
        {

        }


    }
}
