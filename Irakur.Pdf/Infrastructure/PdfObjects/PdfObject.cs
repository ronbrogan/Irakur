using Irakur.Pdf.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Irakur.Pdf.Infrastructure.PdfObjects
{
    public abstract class PdfObject : IPdfObject
    {
        public Guid Id { get; }

        private IndirectReference reference;

        /// <summary>
        /// Constructor used to add the object as an indirect object for the document
        /// </summary>
        /// <param name="doc"></param>
        public PdfObject(UnderlyingPdf doc)
        {
            this.Id = Guid.NewGuid();

            this.reference = doc.indirectObjects.Add(this);
        }

        /// <summary>
        /// Constructor to be used when a direct object is necessary
        /// </summary>
        public PdfObject()
        {
            this.Id = Guid.NewGuid();
        }

        public IndirectReference GetReference()
        {
            return reference;
        }

        public abstract PdfObjectType Type { get; }
    }
}
