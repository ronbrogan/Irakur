using Irakur.Pdf.Infrastructure.Collections;
using Irakur.Pdf.Infrastructure.Core;
using Irakur.Pdf.Infrastructure.PdfObjects;
using Irakur.Pdf.Infrastructure.Serialization;
using Irakur.Pdf.Infrastructure.Serialization.Serdes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace Irakur.Pdf.Infrastructure.IO
{
    /// <summary>
    /// The PdfWriter is the class that is responsible for writing the provided object to an output stream via the serializer(s).
    /// </summary>
    public class PdfWriter : IDisposable
    {
        private readonly Stream baseStream;
        private readonly IterativePdfSerializer iSer;

        private bool leaveStreamOpen;
        public string LineEnding = "\r\n";

        internal PdfWriter(Stream stream, bool leaveOpen = false)
        {
            this.baseStream = stream;
            this.iSer = new IterativePdfSerializer();
            this.leaveStreamOpen = leaveOpen;
        }

        public void WriteHeader()
        {
            this.WriteLine(PdfTokens.FileHeaderPrefix + PdfTokens.FileHeaderVersion17);
        }

        public void WriteBinaryIndicator()
        {
            this.WriteLine(PdfTokens.BinaryContentIndicator);
        }

        internal SerializationResult WriteBody(Catalog root)
        {
            this.WriteLine();

            var serializeResult = iSer.SerializeTree(this, root);

            return serializeResult;
        }

        internal long WriteXref(SerializationResult serializeResult)
        {
            var position = this.Position;

            var xrefTable = new XrefTable();

            foreach (var offset in serializeResult.Offsets)
            {
                xrefTable.Add(offset, new IndirectReference(0, 0));
            }

            this.iSer.SerializeXrefTable(this, xrefTable);

            return position;
        }

        internal void WriteTrailer(UnderlyingPdf document, SerializationResult sRes)
        {
            var trailer = new PdfTrailer(sRes.References[document.Root], sRes.Offsets.Count);

            this.iSer.SerializeTrailer(this, trailer);
        }

        public void WriteStartXref(long xrefLocation)
        {
            this.WriteLine(PdfTokens.StartXref);
            this.WriteLine(xrefLocation);
        }

        public long Position
        {
            get => this.baseStream.Position;
        }

        public void Write(string text)
        {
            var textBytes = Encoding.UTF8.GetBytes(text);
            baseStream.Write(textBytes, 0, textBytes.Length);
        }

        public void WriteLine(string text = null)
        {
            this.Write((text ?? string.Empty) + this.LineEnding);
        }

        public void WriteLine(object value)
        {
            this.WriteLine(value.ToString());
        }

        #region Rollups 

        public void WriteEndOfFile()
        {
            this.WriteLine(PdfTokens.EndOfFile);
        }

        public void WriteDictionaryStart()
        {
            this.WriteLine("<<");
        }

        public void WriteDictionaryEnd()
        {
            this.WriteLine(">>");
        }

        public void WriteType(PdfObjectType type)
        {
            this.WriteName("Type", new Name(type.ToString()));
        }

        public void WriteSubtype(string subtype)
        {
            this.WriteName("Subtype", new Name(subtype));
        }

        public void WriteRaw(string key, object value)
        {
            this.WriteLine($"/{key} {value.ToString()}");
        }

        public void WriteName(string key, Name name)
        {
            this.WriteLine($"/{key} /{name}");
        }

        public void WriteReference(string key, IndirectReference reference)
        {
            this.WriteLine($"/{key} {reference.Identifier} {reference.Generation} R");
        }

        public void WriteReference(string key, int identifier)
        {
            this.WriteLine($"/{key} {identifier} 0 R");
        }

        public void WriteReferences(string key, IEnumerable<IndirectReference> references)
        {
            var serializedValues = references.Select(r => $"{r.Identifier} {r.Generation} R");

            this.WriteLine($"/{key} [{string.Join(" ", serializedValues)}]");
        }

        public void WriteReferences(string key, IEnumerable<int> identifiers)
        {
            var serializedValues = identifiers.Select(r => $"{r} 0 R");

            this.WriteLine($"/{key} [{string.Join(" ", serializedValues)}]");
        }

        #endregion


        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).

                    if (leaveStreamOpen == false)
                        this.baseStream.Dispose();

                }

                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
        }
        #endregion
    }
}
