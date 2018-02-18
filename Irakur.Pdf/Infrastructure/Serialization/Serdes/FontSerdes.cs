using System;
using System.Collections.Generic;
using System.Text;
using Irakur.Pdf.Infrastructure.Collections;
using Irakur.Pdf.Infrastructure.IO;
using Irakur.Pdf.Infrastructure.PdfObjects;

namespace Irakur.Pdf.Infrastructure.Serialization.Serdes
{
    internal class FontSerdes : IPdfSerdes
    {
        public object Deserialize(string content)
        {
            throw new NotImplementedException();
        }

        public void Serialize(PdfWriter writer, object item, IndirectObjectDictionary references)
        {
            var font = (Font)item;

            writer.WriteDictionaryStart();

            writer.WriteType(font.Type);
            writer.WriteSubtype(font.Subtype.ToString());
            writer.WriteName("BaseFont", font.BaseFont);

            writer.WriteDictionaryEnd();
        }
    }
}
