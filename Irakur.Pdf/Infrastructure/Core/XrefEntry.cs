using System;
using System.Collections.Generic;
using System.Text;

namespace Irakur.Pdf.Infrastructure.Core
{
    internal class XrefEntry
    {
        internal XrefEntry(long offset, IndirectReference reference, bool free = false)
        {
            Offset = offset;
            Reference = reference;
            Free = free;
        }

        internal long Offset { get; set; }

        internal IndirectReference Reference { get; set; }

        internal bool Free { get; set; }
    }
}
