using Irakur.Pdf.Infrastructure.Collections;
using Irakur.Pdf.Infrastructure.Core;
using Irakur.Pdf.Infrastructure.PdfObjects;
using Irakur.Pdf.Infrastructure.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Irakur.Pdf.Infrastructure.IO
{
    /// <summary>
    /// The PdfWriter is the class that is responsible for writing the provided object to an output stream via the serializer(s).
    /// </summary>
    public class PdfWriter : IDisposable
    {
        private readonly Stream baseStream;
        private bool leaveStreamOpen;
        public string LineEnding = "\r\n";

        public PdfWriter(Stream stream, bool leaveOpen = false)
        {
            this.baseStream = stream;
            this.leaveStreamOpen = leaveOpen;

        }

        public long Position {
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

        public void WriteHeader()
        {
            this.WriteLine(PdfTokens.FileHeaderPrefix + PdfTokens.FileHeaderVersion17);
        }

        public void WriteBinaryIndicator()
        {
            this.WriteLine(PdfTokens.BinaryContentIndicator);
        }

        public long WriteObject(IndirectReference reference, IPdfObject obj)
        {
            var position = this.Position;

            this.WriteLine(PdfObjectSerializer.SerializeObject(reference, obj));

            return position;
        }

        public void WriteStartXref(long xrefLocation)
        {
            this.WriteLine(PdfTokens.StartXref);
            this.WriteLine(xrefLocation);
        }

        internal long WriteXref(XrefTable xrefTable)
        {
            var position = this.Position;
            this.WriteLine(PdfTokens.Xref);

            this.Write(PdfObjectSerializer.SerializeXrefTable(xrefTable));

            return position;
        }

        internal void WriteTrailer(UnderlyingPdf document)
        {
            var trailer = new PdfTrailer(document);

            this.WriteLine(PdfObjectSerializer.SerializeTrailer(trailer));
        }

        public void WriteEndOfFile()
        {
            this.WriteLine(PdfTokens.EndOfFile);
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
