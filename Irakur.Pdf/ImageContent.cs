using Irakur.Core.CoordinateSystem;
using Irakur.Pdf.Infrastructure.PdfObjects;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.Collections.Generic;
using System.Text;

namespace Irakur.Pdf
{
    public class ImageContent
    {
        public ImageExternalObject Image { get; set; }

        public Rectangle Rectangle { get; set; }

        public ImageContent() { }

        public ImageContent(Image<Rgb24> image, int x, int y)
        {
            this.Image = new ImageExternalObject(image);
            this.Rectangle = new Rectangle(x, y, image.Width, image.Height);

        }
    }
}
