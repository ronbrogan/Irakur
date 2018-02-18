using Irakur.Pdf.Infrastructure.Collections;
using Irakur.Pdf.Infrastructure.IO;
using System;
using System.Collections.Generic;
using System.Text;

namespace Irakur.Pdf.Infrastructure.Serialization.Serdes
{
    internal interface IPdfSerdes
    {
        void Serialize(PdfWriter writer, object item, IndirectObjectDictionary references);
        object Deserialize(string content);
    }
}
