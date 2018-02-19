using System;
using System.Collections.Generic;
using System.Text;

namespace Irakur.Core.Extensions
{
    public static class MathExtensions
    {
        public static float DegreesToRadians(this float degrees)
        {
            return degrees * ((float)Math.PI / 180);
        }

    }
}
