using Irakur.Pdf.Infrastructure.Core;
using Irakur.Pdf.Infrastructure.PdfObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Irakur.Pdf.Infrastructure.Collections
{
    internal class XrefTable
    {
        internal List<XrefEntry> Entries { get; set; }

        internal XrefTable()
        {
            Entries = new List<XrefEntry>()
            {
                // Creates free zero entry
                new XrefEntry(0, new IndirectReference(0, UInt16.MaxValue), true)
            };
        }

        internal void Add(XrefEntry entry)
        {
            this.Entries.Add(entry);
        }

        internal void Add(long offset, IndirectReference reference, bool free = false)
        {
            this.Add(new XrefEntry(offset, reference, free));
        }
    }
}
