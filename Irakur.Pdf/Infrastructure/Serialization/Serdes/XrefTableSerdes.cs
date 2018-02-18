using System;
using System.Collections.Generic;
using System.Text;
using Irakur.Pdf.Infrastructure.Collections;
using Irakur.Pdf.Infrastructure.IO;
using Irakur.Pdf.Infrastructure.PdfObjects;

namespace Irakur.Pdf.Infrastructure.Serialization.Serdes
{
    internal class XrefTableSerdes : IPdfSerdes
    {
        public object Deserialize(string content)
        {
            throw new NotImplementedException();
        }

        public void Serialize(PdfWriter writer, object item, IndirectObjectDictionary references)
        {
            var xrefTable = (XrefTable)item;

            writer.WriteLine($"0 {xrefTable.Entries.Count}");
            foreach (var xref in xrefTable.Entries)
            {
                var offset = xref.Offset.ToString().PadLeft(10, '0');
                var generation = xref.Reference.Generation.ToString().PadLeft(5, '0');
                var free = xref.Free ? "f" : "n";
                writer.WriteLine($"{offset} {generation} {free}");
            }
        }
    }
}
