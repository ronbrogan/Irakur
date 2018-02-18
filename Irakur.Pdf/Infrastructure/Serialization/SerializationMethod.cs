using System;
using System.Collections.Generic;
using System.Text;

namespace Irakur.Pdf.Infrastructure.Serialization
{
    internal enum SerializationMethod
    {
        Default = 0,
        Reference = 1,
        Literal = 2,
        Object = 3
    }
}
