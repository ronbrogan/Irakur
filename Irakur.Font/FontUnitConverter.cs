using System;
using System.Collections.Generic;
using System.Text;

namespace Irakur.Font
{
    public static class FontUnitConverter
    {
        private const float PointsPerEm = 11.955168f;
        private const int PointsPerInch = 72;

        public static float EmToPoints(float em)
        {
            return em * PointsPerEm;
        }

        public static float PointsToEm(float points)
        {
            return points / PointsPerEm;
        }

        public static int PointsToPixels(float points, int ppi = 96)
        {
            return (int)Math.Ceiling(points * (ppi / (float)PointsPerInch));
        }

        public static int EmToPixels(float em, int ppi = 96)
        {
            return PointsToPixels(EmToPoints(em), ppi);
        }

    }
}
