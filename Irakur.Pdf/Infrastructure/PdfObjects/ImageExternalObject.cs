using Irakur.Pdf.Graphics;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Irakur.Pdf.Infrastructure.PdfObjects
{
    public class ImageExternalObject : ExternalObject
    {
        public ExternalObjectSubtype Subtype => ExternalObjectSubtype.Image;
        public int Width { get; set; }
        public int Height { get; set; }
        public ColorSpace ColorSpace { get; set; }
        public int BitsPerComponent { get; set; }
        public byte[] Data { get; set; }

        public ImageExternalObject(Image<Rgb24> image) : base(true)
        {
            this.Width = image.Width;
            this.Height = image.Height;
            this.ColorSpace = ColorSpace.DeviceRGB;
            this.BitsPerComponent = 8;


            this.Data = image.SavePixelData();
        }

        public override IEnumerable<IPdfObject> GetChildren()
        {
            yield break;
        }
    }
}
