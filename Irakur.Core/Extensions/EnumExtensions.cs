using Irakur.Core.Attributes;
using System;

namespace Irakur.Core.Extensions
{
    public static class EnumExtensions
    {
        public static Type GetImplementationTypeForEnumValue(Type enumType, object value)
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
