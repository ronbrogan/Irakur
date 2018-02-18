using Irakur.Core.Extensions;
using Irakur.Pdf.Infrastructure.Collections;
using Irakur.Pdf.Infrastructure.IO;
using Irakur.Pdf.Infrastructure.PdfObjects;
using Irakur.Pdf.Infrastructure.Serialization.Serdes;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Irakur.Pdf.Infrastructure.Serialization
{
    internal class IterativePdfSerializer
    {

        public SerializationResult SerializeTree(PdfWriter writer, IPdfObject root)
        {
            var references = new List<Guid>();
            var indRefs = new IndirectObjectDictionary();
            // Add empty GUID to represent the "free" object
            references.Add(Guid.Empty);
            
            var offsets = new List<int>();

            // need to visit all nodes to reserve references for each node

            var nodesToSerialize = new ConcurrentQueue<IPdfObject>();
            var nodesToVisit = new Stack<IPdfObject>();
            nodesToVisit.Push(root);
            while (nodesToVisit.Count > 0)
            {
                var node = nodesToVisit.Pop();
                if (node == null || indRefs.Contains(node))
                {
                    continue;
                }

                nodesToVisit.PushRange(node.GetChildren());
                indRefs.Add(node);
                nodesToSerialize.Enqueue(node);
            }

            while (nodesToSerialize.TryDequeue(out IPdfObject node))
            {
                offsets.Add((int)writer.Position);
                writer.WriteLine($"{indRefs[node].Identifier} 0 {PdfTokens.StartObject}");
                SerdesFactory.GetFor(node).Serialize(writer, node, indRefs);
                writer.WriteLine(PdfTokens.EndObject);
            }

            return new SerializationResult()
            {
                Offsets = offsets,
                References = indRefs
            };
        }

        public void SerializeXrefTable(PdfWriter writer, XrefTable xrefTable)
        {
            writer.WriteLine(PdfTokens.Xref);
            SerdesFactory.GetFor(xrefTable).Serialize(writer, xrefTable, null);
        }

        public void SerializeTrailer(PdfWriter writer, PdfTrailer trailer)
        {
            writer.WriteLine(PdfTokens.Trailer);
            SerdesFactory.GetFor(trailer).Serialize(writer, trailer, null);
        }
    }
}
