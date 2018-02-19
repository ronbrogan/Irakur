using System;
using System.Collections.Generic;
using System.Text;

namespace Irakur.Pdf.Infrastructure.PdfObjects
{
    public abstract class ExternalObject : PdfObject
    {
        public ExternalObject(bool indirect): base(indirect)
        {

        }

        public override PdfObjectType Type => PdfObjectType.XObject;
    }
}
