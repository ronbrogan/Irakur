using Irakur.Pdf.Infrastructure.Collections;
using System;
using System.Collections.Generic;
using System.Text;

namespace Irakur.Pdf.Infrastructure.Serialization
{
    internal class SerializationResult
    {
        public string Output { get; set; }

        public List<int> Offsets { get; set; }

        public IndirectObjectDictionary References { get; set; }
    }
}
