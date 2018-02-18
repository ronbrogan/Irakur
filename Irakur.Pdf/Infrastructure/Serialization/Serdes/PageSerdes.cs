using System;
using System.Collections.Generic;
using System.Text;
using Irakur.Pdf.Infrastructure.Collections;
using Irakur.Pdf.Infrastructure.IO;
using Irakur.Pdf.Infrastructure.PdfObjects;

namespace Irakur.Pdf.Infrastructure.Serialization.Serdes
{
    internal class PageSerdes : IPdfSerdes
    {
        public object Deserialize(string content)
        {
            throw new NotImplementedException();
        }

        public void Serialize(PdfWriter writer, object item, IndirectObjectDictionary references)
        {
            var page = (Page)item;

            writer.WriteDictionaryStart();
            writer.WriteType(page.Type);
            writer.WriteReference(nameof(page.Parent), references[page.Parent]);

            writer.WriteRaw(nameof(page.Resources), string.Empty);

            SerdesFactory.GetFor(page.Resources).Serialize(writer, page.Resources, references);

            writer.WriteRaw(nameof(page.MediaBox), string.Empty);

            SerdesFactory.GetFor(page.MediaBox).Serialize(writer, page.MediaBox, references);

            writer.WriteReference(nameof(page.Contents), references[page.Contents]);

            writer.WriteDictionaryEnd();
        }
    }
}
