using Irakur.Core.CoordinateSystem;
using Irakur.Pdf.Infrastructure.Collections;
using Irakur.Pdf.Infrastructure.Core;
using Irakur.Pdf.Infrastructure.IO;
using Irakur.Pdf.Infrastructure.PdfObjects;
using Irakur.Pdf.Infrastructure.Serialization.Serializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Irakur.Pdf.Infrastructure.Serialization
{
    /// <summary>
    /// The PdfObjectSerializer knows how a given object should be represented in the resulting document.
    /// </summary>
    internal static class PdfObjectSerializer
    {
        internal static string SerializeObject(IndirectReference reference, IPdfObject obj)
        {
            var sb = new StringBuilder();

            sb.AppendLine($"{reference.Identifier} {reference.Generation} {PdfTokens.StartObject}");

            sb.Append(SerializeObject((dynamic)obj));

            sb.AppendLine(PdfTokens.EndObject);

            return sb.ToString();
        }

        private static string SerializeObject(Font font)
        {
            var sb = new StringBuilder();
            var dicSerializer = new PdfDictionarySerializer(sb);

            dicSerializer.Start(PdfObjectType.Font);

            dicSerializer.WriteRaw("Subtype", "/Type1");
            dicSerializer.WriteRaw("Name", "/F1");
            dicSerializer.WriteRaw("BaseFont", "/Helvetica");
            dicSerializer.WriteRaw("Encoding", "/WinAnsiEncoding");

            dicSerializer.End();

            return sb.ToString();
        }

        private static string SerializeObject(Catalog obj)
        {
            var sb = new StringBuilder();
            var dicSerializer = new PdfDictionarySerializer(sb);

            dicSerializer.Start(PdfObjectType.Catalog);

            dicSerializer.WriteReference(nameof(obj.Pages), obj.Pages.GetReference());

            dicSerializer.End();
            return sb.ToString();
        }

        private static string SerializeObject(ContentStream obj)
        {
            var sb = new StringBuilder();
            sb.AppendLine(
                $@"<< /Length 81 >>
stream
2 J
BT
0 0 0 rg
/F1 0027 Tf
57.3750 722.2800 Td
( A Test PDF File ) Tj
ET
endstream");

            return sb.ToString();
        }

        private static string SerializeObject(Page obj)
        {
            var sb = new StringBuilder();
            var dicSerializer = new PdfDictionarySerializer(sb);

            dicSerializer.Start(PdfObjectType.Page);

            dicSerializer.WriteReference(nameof(obj.Parent), obj.Parent.GetReference());

            dicSerializer.WriteRaw(nameof(obj.Resources), SerializeResourceDictionary(obj.Resources));

            dicSerializer.WriteRaw(nameof(obj.MediaBox), SerializeObject(obj.MediaBox));

            dicSerializer.WriteReference(nameof(obj.Contents), obj.Contents.GetReference());

            dicSerializer.End();
            return sb.ToString();
        }

        private static string SerializeObject(PageNode node)
        {
            var sb = new StringBuilder();
            var dicSerializer = new PdfDictionarySerializer(sb);

            dicSerializer.Start(PdfObjectType.Pages);

            dicSerializer.WriteRaw(nameof(node.Count), node.Count);
            dicSerializer.WriteReferences(nameof(node.Kids), node.Kids.Select(k => k.GetReference()));

            dicSerializer.End();
            return sb.ToString();
        }

        private static string SerializeObject(Rectangle rect)
        {
            return $"[{rect.X} {rect.Y} {rect.Width} {rect.Height}]";
        }

        internal static string SerializeTrailer(PdfTrailer trailer)
        {
            var sb = new StringBuilder();

            sb.AppendLine("trailer");

            var dicSerializer = new PdfDictionarySerializer(sb);

            dicSerializer.Start();

            dicSerializer.WriteRaw("Size", trailer.Size + 1);
            dicSerializer.WriteReference("Root", trailer.Root.GetReference());

            dicSerializer.End();

            return sb.ToString();
        }

        private static string SerializeResourceDictionary(Resources resources)
        {
            var sb = new StringBuilder();
            var dicSerializer = new PdfDictionarySerializer(sb);
            dicSerializer.Start();

            if (resources.Font != null)
            {
                sb.AppendLine("/Font <<");
                var fontRef = resources.Font.GetReference();
                dicSerializer.WriteReference("F1", fontRef);
                sb.AppendLine(">>");
            }

            dicSerializer.End();

            return sb.ToString();
        }

        internal static string SerializeXrefTable(XrefTable xrefTable)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"0 {xrefTable.Entries.Count}");
            foreach (var xref in xrefTable.Entries)
            {
                var offset = xref.Offset.ToString().PadLeft(10, '0');
                var generation = xref.Reference.Generation.ToString().PadLeft(5, '0');
                var free = xref.Free ? "f" : "n";
                sb.AppendLine($"{offset} {generation} {free}");
            }
            return sb.ToString();
        }
    }
}
