using Irakur.Pdf.Infrastructure.Collections;
using Irakur.Pdf.Infrastructure.IO;
using Irakur.Pdf.Infrastructure.PdfObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Irakur.Pdf.Infrastructure.Serialization.Serdes
{
    internal class CatalogSerdes : IPdfSerdes
    { 

        public object Deserialize(string content)
        {
            throw new NotImplementedException();
        }

        public void Serialize(PdfWriter writer, object item, IndirectObjectDictionary references)
        {
            var catalog = (Catalog)item;

            writer.WriteDictionaryStart();

            writer.WriteType(catalog.Type);
            writer.WriteReference(nameof(catalog.Pages), references[catalog.Pages]);

            writer.WriteDictionaryEnd();
        }
    }
}
