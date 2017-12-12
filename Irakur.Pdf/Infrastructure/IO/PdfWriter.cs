using Irakur.Pdf.Infrastructure.Collections;
using Irakur.Pdf.Infrastructure.Core;
using Irakur.Pdf.Infrastructure.PdfObjects;
using Irakur.Pdf.Infrastructure.Serialization;
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
        private readonly PdfObjectSerializer serializer;
        private IndirectObjectDictionary indirectObjects;

        private bool leaveStreamOpen;
        public string LineEnding = "\r\n";


        internal PdfWriter(Stream stream, bool leaveOpen = false)
        {
            this.baseStream = stream;
            this.serializer = new PdfObjectSerializer();
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

        internal XrefTable WriteBody(Catalog catalog)
        {
            // Write IndirectObjects - record position for xref table
            var xrefTable = new XrefTable();

            indirectObjects = this.AccumulateReferences(catalog);

            foreach(var obj in indirectObjects.Values)
            {
                this.WriteIndirectObject(obj, indirectObjects, xrefTable);
            }

            return xrefTable;
        }

        private IndirectObjectDictionary AccumulateReferences(IPdfObject root)
        {
            var indirectObjDict = new IndirectObjectDictionary(); 

            var nodesToVisit = new Stack<IPdfObject>();
            nodesToVisit.Push(root);

            while (nodesToVisit.Count > 0)
            {
                var node = nodesToVisit.Pop();

                if (node == null || indirectObjDict.Contains(node))
                    continue;

                if (node.Indirect)
                    indirectObjDict.Add(node);

                var pdfObjectProperties = node.GetType().GetProperties().Where(p =>
                    typeof(IPdfObject).IsAssignableFrom(p.PropertyType) ||
                    typeof(IEnumerable<IPdfObject>).IsAssignableFrom(p.PropertyType));

                foreach (var objProp in pdfObjectProperties)
                {
                    if (objProp.PropertyType == typeof(IPdfObject) || typeof(IPdfObject).IsAssignableFrom(objProp.PropertyType))
                    {
                        nodesToVisit.Push((IPdfObject)objProp.GetValue(node));
                    }
                    else if (typeof(IEnumerable<IPdfObject>).IsAssignableFrom(objProp.PropertyType))
                    {
                        var collection = (IEnumerable<IPdfObject>)objProp.GetValue(node);

                        foreach (var pdfObj in collection)
                        {
                            nodesToVisit.Push(pdfObj);
                        }
                    }
                }
            }

            return indirectObjDict;
        }
    
        internal long WriteIndirectObject(IPdfObject obj, IndirectObjectDictionary references, XrefTable xrefTable)
        {
            var position = this.Position;
            this.WriteLine();
            this.WriteLine(this.serializer.SerializeObject(obj, references));
            xrefTable.Add(position, references.GetReference(obj));
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

            this.Write(this.serializer.SerializeXrefTable(xrefTable));

            return position;
        }

        internal void WriteTrailer(UnderlyingPdf document, XrefTable xrefs)
        {
            var trailer = new PdfTrailer(indirectObjects.GetReference(document.Catalog), xrefs);

            this.WriteLine(this.serializer.SerializeTrailer(trailer));
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
