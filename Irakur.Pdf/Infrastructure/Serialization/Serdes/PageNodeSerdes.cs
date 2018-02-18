using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Irakur.Pdf.Infrastructure.Collections;
using Irakur.Pdf.Infrastructure.IO;
using Irakur.Pdf.Infrastructure.PdfObjects;

namespace Irakur.Pdf.Infrastructure.Serialization.Serdes
{
    internal class PageNodeSerdes : IPdfSerdes
    {
        public object Deserialize(string content)
        {
            throw new NotImplementedException();
        }

        public void Serialize(PdfWriter writer, object item, IndirectObjectDictionary references)
        {
            var pageNode = (PageNode)item;

            writer.WriteDictionaryStart();

            writer.WriteType(pageNode.Type);
            writer.WriteRaw(nameof(pageNode.Count), pageNode.Count);
            writer.WriteReferences(nameof(pageNode.Kids), pageNode.Kids.Select(k => references[k]));

            writer.WriteDictionaryEnd();
        }
    }
}
