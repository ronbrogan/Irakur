using System;
using System.Collections.Generic;
using System.Text;
using Irakur.Pdf.Infrastructure.Collections;
using Irakur.Pdf.Infrastructure.IO;
using Irakur.Pdf.Infrastructure.PdfObjects;

namespace Irakur.Pdf.Infrastructure.Serialization.Serdes
{
    internal class ImageExternalObjectSerdes : IPdfSerdes
    {
        public object Deserialize(string content)
        {
            throw new NotImplementedException();
        }

        public void Serialize(PdfWriter writer, object item, IndirectObjectDictionary references)
        {
            var img = (ImageExternalObject)item;

            writer.WriteDictionaryStart();
            writer.WriteType(PdfObjectType.XObject);
            writer.WriteSubtype(img.Subtype.ToString());
            writer.WriteRaw(nameof(img.Width), img.Width);
            writer.WriteRaw(nameof(img.Height), img.Height);
            writer.WriteName(nameof(img.ColorSpace), new Name(img.ColorSpace.ToString()));
            writer.WriteRaw(nameof(img.BitsPerComponent), img.BitsPerComponent);
            writer.WriteRaw("Length", img.Data.Length);
            writer.WriteDictionaryEnd();

            writer.WriteLine(PdfTokens.Stream.Start);
            writer.Write(img.Data);
            writer.WriteLine();
            writer.WriteLine(PdfTokens.Stream.End);
        }
    }
}
