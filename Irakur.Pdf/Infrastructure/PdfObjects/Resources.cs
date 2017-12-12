using System;
using System.Collections.Generic;
using System.Text;

namespace Irakur.Pdf.Infrastructure.PdfObjects
{
    public class Resources : PdfObject
    {
        public override PdfObjectType Type => PdfObjectType.Resources;

        public List<Font> Fonts { get; set; }

        public Resources() : base(false)
        {

        }
    }
}
