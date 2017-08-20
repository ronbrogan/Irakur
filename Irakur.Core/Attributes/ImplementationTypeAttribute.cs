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

        public static Type GetTypeFromEnumValue(Type enumType, object value)
        {
            if (!Enum.IsDefined(enumType, value))
                return null;

            var typeAttrs = enumType
                .GetMember(value.ToString())[0]
                .GetCustomAttributes(typeof(ImplementationTypeAttribute), false);

            if (typeAttrs.Length == 0)
                return null;

            return ((ImplementationTypeAttribute)typeAttrs[0]).type;
        }
    }
}
