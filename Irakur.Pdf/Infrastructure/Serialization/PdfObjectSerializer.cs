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
    internal class PdfObjectSerializer
    {
        public PdfObjectSerializer()
        {
        }

        internal string SerializeObject(IPdfObject obj, IndirectObjectDictionary references)
        {
            var sb = new StringBuilder();

            var reference = references.Contains(obj) ? references.GetReference(obj) : (IndirectReference?)null;

            if (reference.HasValue)
            {
                sb.AppendLine($"{reference.Value.Identifier} {reference.Value.Generation} {PdfTokens.StartObject}");
            }

            sb.Append(SerializeObject((dynamic)obj, references));

            if (reference.HasValue)
            {
                sb.AppendLine(PdfTokens.EndObject);
            }

            return sb.ToString();
        }

        private string SerializeObject(Font font, IndirectObjectDictionary references)
        {
            var sb = new StringBuilder();
            var dicSerializer = new PdfDictionarySerializer(sb);

            dicSerializer.Start(PdfObjectType.Font);

            dicSerializer.WriteName("Subtype", new Name(font.Subtype));
            dicSerializer.WriteName("BaseFont", font.BaseFont);

            dicSerializer.End();

            return sb.ToString();
        }

        private string SerializeObject(Catalog obj, IndirectObjectDictionary references)
        {
            var sb = new StringBuilder();
            var dicSerializer = new PdfDictionarySerializer(sb);

            dicSerializer.Start(PdfObjectType.Catalog);

            dicSerializer.WriteReference(nameof(obj.Pages), references.GetReference(obj.Pages));

            dicSerializer.End();
            return sb.ToString();
        }

        private string SerializeObject(ContentStream obj, IndirectObjectDictionary references)
        {
            var streamData = new StringBuilder();
            foreach(var text in obj.TextContent)
            {
                streamData.AppendLine(PdfTokens.Text.Begin);
                streamData.AppendLine($"{text.FillColor.Red} {text.FillColor.Green} {text.FillColor.Blue} {PdfTokens.Graphics.Color.Operators.SetFillRGB}");
                streamData.AppendLine($"/{ResourceReference(text.Font)} {text.FontSize} {PdfTokens.Text.StateOperators.FontSize}");
                streamData.AppendLine($"{text.Rectangle.X} {text.Rectangle.Y} {PdfTokens.Text.PositioningOperators.MoveFromCurrent}");
                streamData.AppendLine($"( {text.Text} ) {PdfTokens.Text.ShowingOperators.Show}");
                streamData.AppendLine(PdfTokens.Text.End);
            }
            var streamContents = streamData.ToString();

            var sb = new StringBuilder();
            var dicSerializer = new PdfDictionarySerializer(sb);
            dicSerializer.Start();
            dicSerializer.WriteRaw("Length", streamContents.Length);
            dicSerializer.End();
            sb.AppendLine(PdfTokens.Stream.Start);
            sb.Append(streamContents);
            sb.AppendLine(PdfTokens.Stream.End);

            return sb.ToString();
        }

        private string SerializeObject(Page obj, IndirectObjectDictionary references)
        {
            var sb = new StringBuilder();
            var dicSerializer = new PdfDictionarySerializer(sb);

            dicSerializer.Start(PdfObjectType.Page);

            dicSerializer.WriteReference(nameof(obj.Parent), references.GetReference(obj.Parent));

            dicSerializer.WriteRaw(nameof(obj.Resources), SerializeObject(obj.Resources, references));

            dicSerializer.WriteRaw(nameof(obj.MediaBox), SerializeObject(obj.MediaBox, references));

            dicSerializer.WriteReference(nameof(obj.Contents), references.GetReference(obj.Contents));

            dicSerializer.End();
            return sb.ToString();
        }

        private string SerializeObject(PageNode node, IndirectObjectDictionary references)
        {
            var sb = new StringBuilder();
            var dicSerializer = new PdfDictionarySerializer(sb);

            dicSerializer.Start(PdfObjectType.Pages);

            dicSerializer.WriteRaw(nameof(node.Count), node.Count);
            dicSerializer.WriteReferences(nameof(node.Kids), node.Kids.Select(k => references.GetReference(k)));

            dicSerializer.End();
            return sb.ToString();
        }

        private string SerializeObject(Rectangle rect, IndirectObjectDictionary references)
        {
            return $"[{rect.X} {rect.Y} {rect.Width} {rect.Height}]";
        }

        private string SerializeObject(Name name, IndirectObjectDictionary references)
        {
            return $"/{name.ToString()}";
        }

        internal string SerializeTrailer(PdfTrailer trailer)
        {
            var sb = new StringBuilder();

            sb.AppendLine("trailer");

            var dicSerializer = new PdfDictionarySerializer(sb);

            dicSerializer.Start();

            dicSerializer.WriteRaw("Size", trailer.Size);
            dicSerializer.WriteReference("Root", trailer.Root);

            dicSerializer.End();

            return sb.ToString();
        }

        private string SerializeObject(Resources resources, IndirectObjectDictionary references)
        {
            var sb = new StringBuilder();
            var dicSerializer = new PdfDictionarySerializer(sb);
            dicSerializer.Start();

            if (resources.Fonts?.Count > 0)
            {
                sb.AppendLine("/Font <<");
                for (var i = 0; i < resources.Fonts.Count; i++)
                {
                    var fontRef = references.GetReference(resources.Fonts[i]);
                    dicSerializer.WriteReference($"{ResourceReference(resources.Fonts[i])}", fontRef);
                }
                sb.AppendLine(">>");
            }

            dicSerializer.End();

            return sb.ToString();
        }

        private static string ResourceReference(IPdfObject obj)
        {
            return obj.Id.ToString().Replace("-", string.Empty);
        }

        internal string SerializeXrefTable(XrefTable xrefTable)
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
