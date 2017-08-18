using System;
using System.Collections.Generic;
using System.Text;

namespace Irakur.Core.Attributes
{
    public class ImplementationTypeAttribute : Attribute
    {
        public Type type { get; set; }

        public ImplementationTypeAttribute(Type type)
        {
            this.type = type;
        }
    }
}
