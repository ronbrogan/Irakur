using Irakur.Core.CoordinateSystem;
using Irakur.Pdf.Infrastructure.PdfObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Irakur.Pdf
{
    public class PdfPage
    {
        public Guid Id { get; }
        /// <summary>
        /// Use to override the page's size, otherwise inherits from the document it's added to
        /// </summary>
        public Size SizeOverride { get; set; }
        public List<TextContent> TextContent { get; set; } = new List<TextContent>();
        

        public PdfPage()
        {
            this.Id = Guid.NewGuid();
        }


        public IEnumerable<IPdfObject> GetRawResources()
        {
            foreach(var text in TextContent)
            {
                yield return text.Font;
            }
        }

        public Resources GetResources()
        {
            var fonts = this.TextContent.Select(t => t.Font).Distinct().ToList();

            return new Resources
            {
                Fonts = fonts
            };
        }
    }
}
