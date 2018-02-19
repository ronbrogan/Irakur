using System;
using System.Collections.Generic;
using System.Text;

namespace Irakur.Pdf.Infrastructure.PdfObjects
{
    public class Resources : PdfObject
    {
        public override PdfObjectType Type => PdfObjectType.Resources;
        public List<Font> Fonts { get; set; }
        public List<ImageExternalObject> Images { get; set; }

        public Resources() : base(false)
        {

        }

        public override IEnumerable<IPdfObject> GetChildren()
        {
            foreach(var font in Fonts)
            {
                yield return font;
            }

            foreach(var image in Images)
            {
                yield return image;
            }
        }
    }
}
