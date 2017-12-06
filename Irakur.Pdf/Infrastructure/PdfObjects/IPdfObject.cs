using System;
using System.Collections.Generic;
using System.Text;

namespace Irakur.Pdf.Infrastructure.PdfObjects
{
    public interface IPdfObject
    {
        PdfObjectType Type { get; }

        Guid Id { get; }
    }
}
