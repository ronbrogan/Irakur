using System;
using System.Collections.Generic;
using System.Text;
using Irakur.Pdf.Infrastructure.Collections;
using Irakur.Pdf.Infrastructure.IO;
using Irakur.Pdf.Infrastructure.PdfObjects;

namespace Irakur.Pdf.Infrastructure.Serialization.Serdes
{
    internal class PdfTrailerSerdes : IPdfSerdes
    {
        public object Deserialize(string content)
        {
            throw new NotImplementedException();
        }

        public void Serialize(PdfWriter writer, object item, IndirectObjectDictionary references)
        {
            var trailer = (PdfTrailer)item;

            writer.WriteDictionaryStart();

            writer.WriteRaw("Size", trailer.Size);
            writer.WriteReference("Root", trailer.Root);

            writer.WriteDictionaryEnd();
        }
    }
}
