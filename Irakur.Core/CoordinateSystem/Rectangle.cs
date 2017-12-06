using System;
using System.Collections.Generic;
using System.Text;

namespace Irakur.Core.CoordinateSystem
{
    public struct Rectangle
    {
        public double X;
        public double Y;
        public double Width;
        public double Height;

        public Rectangle(double x, double y, double sideLength)
        {
            X = x;
            Y = y;
            Width = sideLength;
            Height = sideLength;
        }

        public Rectangle(double x, double y, double width, double height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }
    }
}
