using System;
using System.Collections.Generic;
using System.Text;

namespace Irakur.Pdf.Infrastructure.Core
{
    public struct IndirectReference
    {
        public IndirectReference(uint ident, uint gen)
        {
            this.Identifier = ident;
            this.Generation = gen;
        }

        public uint Identifier;
        public uint Generation;
    }
}
