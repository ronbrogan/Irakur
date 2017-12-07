using Irakur.Pdf.Infrastructure.Core;
using Irakur.Pdf.Infrastructure.PdfObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Irakur.Pdf.Infrastructure.Serialization.Serializers
{
    internal class PdfDictionarySerializer
    {
        private readonly StringBuilder sb;

        public PdfDictionarySerializer(StringBuilder sb)
        {
            this.sb = sb;
        }

        public void Start(PdfObjectType? objectType = null)
        {
            sb.AppendLine("<<");

            if (objectType.HasValue)
                sb.AppendLine($"/Type /{objectType.ToString()}");
        }

        public void End()
        {
            sb.AppendLine(">>");
        }

        public void WriteRaw(string key, object value)
        {
            sb.AppendLine($"/{key} {value.ToString()}");
        }

        public void WriteName(string key, Name name)
        {
            sb.AppendLine($"/{key} /{name}");
        }

        public void WriteReference(string key, IndirectReference reference)
        {
            sb.AppendLine($"/{key} {reference.Identifier} {reference.Generation} R");
        }

        public void WriteReferences(string key, IEnumerable<IndirectReference> references)
        {
            var serializedValues = references.Select(r => $"{r.Identifier} {r.Generation} R");

            sb.AppendLine($"/{key} [{string.Join(", ", serializedValues)}]");
        }
    }
}
