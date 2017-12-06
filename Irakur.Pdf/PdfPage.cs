using Irakur.Core.CoordinateSystem;
using System.Collections.Generic;

namespace Irakur.Pdf
{
    public class PdfPage
    {
        /// <summary>
        /// Use to override the page's size, otherwise inherits from the document it's added to
        /// </summary>
        public Size SizeOverride { get; set; }

        public List<TextContent> TextContent { get; set; }

    }
}
