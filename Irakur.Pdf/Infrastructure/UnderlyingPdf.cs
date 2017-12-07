using Irakur.Pdf.Infrastructure.Collections;
using Irakur.Pdf.Infrastructure.Core;
using Irakur.Pdf.Infrastructure.IO;
using Irakur.Pdf.Infrastructure.PdfObjects;
using Irakur.Pdf.Infrastructure.Serialization;
using Irakur.Pdf.Infrastructure.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Irakur.Pdf.Infrastructure
{

    /// <summary>
    /// The UnderlyingPdf object is the class that knows about the document structure. 
    /// </summary>
    public class UnderlyingPdf
    {
        public Catalog Catalog { get; set; }

        public IndirectObjectDictionary indirectObjects { get; set; }

        public UnderlyingPdf()
        {
            indirectObjects = new IndirectObjectDictionary();

            Catalog = new Catalog(this);
        }

        public bool ContainsBinary { get; set; }

        public void Flush(Stream stream)
        {
            ContainsBinary = true;

            var writer = new PdfWriter(stream);

            writer.WriteHeader();

            if (ContainsBinary)
                writer.WriteBinaryIndicator();

            // Write IndirectObjects - record position for xref table
            var xrefTable = new XrefTable();
            foreach(var reference in indirectObjects.Keys)
            {
                writer.WriteLine();
                var offset = writer.WriteObject(reference, indirectObjects[reference]);
                xrefTable.Add(offset, reference);
            }

            // Write Xrefs table - record position for startxref
            writer.WriteLine();
            var xrefLocation = writer.WriteXref(xrefTable);

            // Write trailer
            writer.WriteLine();
            writer.WriteTrailer(this);

            // Write startxref
            writer.WriteLine();
            writer.WriteStartXref(xrefLocation);

            // Write EOF
            writer.WriteEndOfFile();
            writer.Dispose();
        }

        

        public void ProcessPages(List<PdfPage> pages)
        {
            var pageNode = new PageNode(this);

            foreach(var page in pages)
            {
                var underlyingPage = new Page(this)
                {
                    Resources = new Resources()
                    {
                        Fonts = new List<Font>()
                    },
                    Parent = pageNode
                };

                underlyingPage.Contents = new ContentStream(this)
                {
                    Parent = underlyingPage
                };

                underlyingPage.Contents.TextContent.AddRange(page.TextContent);


                foreach(var text in underlyingPage.Contents.TextContent)
                {
                    // TODO: make sure these are unique
                    underlyingPage.Resources.Fonts.Add(text.Font);
                }

                pageNode.Kids.Add(underlyingPage);
            }

            Catalog.Pages = pageNode;
        }
    }
}
