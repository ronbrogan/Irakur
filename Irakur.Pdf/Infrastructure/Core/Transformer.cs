using Irakur.Core.CoordinateSystem;
using Irakur.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Irakur.Pdf.Infrastructure.Core
{
    public static class Transformer
    {
        public static Matrix3x2 GetTransform(Rectangle source, Rectangle dest)
        {
            // translate
            var t = Matrix3x2.CreateTranslation((float)(dest.X - source.X), (float)(dest.Y - source.Y));

            //rotate 
            var r = Matrix3x2.CreateRotation(0f);

            //scale/skew
            var s = Matrix3x2.CreateScale((float)dest.Width, (float)dest.Height);

            return Matrix3x2.Identity * s * r * t;
        }
    }
}
