using System;
using System.Collections.Generic;
using Irakur.Core.CoordinateSystem;
using Irakur.Pdf.Infrastructure.Collections;
using Irakur.Pdf.Infrastructure.IO;

namespace Irakur.Pdf.Infrastructure.Serialization.Serdes
{
    internal class RectangleSerdes : IPdfSerdes
    {
        public object Deserialize(string content)
        {
            throw new NotImplementedException();
        }

        public void Serialize(PdfWriter writer, object item, IndirectObjectDictionary references)
        {
            var rect = (Rectangle)item;

            writer.Write($"[{rect.X} {rect.Y} {rect.Width} {rect.Height}]");
        }
    }
}
