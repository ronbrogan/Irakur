using System;
using System.Collections.Generic;
using System.Text;
using Irakur.Pdf.Infrastructure.Collections;
using Irakur.Pdf.Infrastructure.IO;
using Irakur.Pdf.Infrastructure.PdfObjects;

namespace Irakur.Pdf.Infrastructure.Serialization.Serdes
{
    internal class ResourcesSerdes : IPdfSerdes
    {
        public object Deserialize(string content)
        {
            throw new NotImplementedException();
        }

        public void Serialize(PdfWriter writer, object item, IndirectObjectDictionary references)
        {
            var resources = (Resources)item;

            writer.WriteDictionaryStart();

            if (resources.Fonts?.Count > 0)
            {
                writer.WriteRaw("Font", PdfTokens.StartDictionary);
                for (var i = 0; i < resources.Fonts.Count; i++)
                {
                    var fontRef = references[resources.Fonts[i]];
                    writer.WriteReference($"{resources.Fonts[i].Id.ToString().Replace("-", "")}", fontRef);
                }
                writer.WriteLine(PdfTokens.EndDictionary);
            }

            if (resources.Images?.Count > 0)
            {
                writer.WriteRaw("XObject", PdfTokens.StartDictionary);
                for (var i = 0; i < resources.Images.Count; i++)
                {
                    var imgRef = references[resources.Images[i]];
                    writer.WriteReference($"{resources.Images[i].Id.ToString().Replace("-", "")}", imgRef);
                }
                writer.WriteLine(PdfTokens.EndDictionary);
            }

            writer.WriteDictionaryEnd();
        }
    }
}
