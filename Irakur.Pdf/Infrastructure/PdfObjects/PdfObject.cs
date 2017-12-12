using Irakur.Pdf.Infrastructure.Core;
using Irakur.Pdf.Infrastructure.Serialization;
using System;
using System.Collections.Generic;
using System.Text;

namespace Irakur.Pdf.Infrastructure.PdfObjects
{
    public abstract class PdfObject : IPdfObject
    {
        public Guid Id { get; }

        public bool Indirect { get; set; }

        /// <summary>
        /// Constructor used to add the object as an indirect object for the document
        /// </summary>
        /// <param name="doc"></param>
        public PdfObject(bool indirect = true)
        {
            this.Id = Guid.NewGuid();

            this.Indirect = indirect;
        }

        public abstract PdfObjectType Type { get; }
    }
}
