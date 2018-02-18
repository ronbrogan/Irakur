using Irakur.Pdf.Infrastructure.Collections;
using Irakur.Pdf.Infrastructure.Core;
using Irakur.Pdf.Infrastructure.IO;
using Irakur.Pdf.Infrastructure.PdfObjects;
using Irakur.Pdf.Infrastructure.Serialization;
using Irakur.Pdf.Infrastructure.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Irakur.Pdf.Infrastructure
{

    /// <summary>
    /// The UnderlyingPdf object is the class that knows about the document structure. 
    /// </summary>
    internal class UnderlyingPdf
    {
        public Catalog Root { get; set; }

        public UnderlyingPdf(PdfDocument input)
        {
            this.Root = new Catalog();
            this.ProcessPages(input.Pages, true);
        }

        public bool ContainsBinary { get; set; }

        public void Flush(Stream stream)
        {
            ContainsBinary = true;
            
            var writer = new PdfWriter(stream);

            writer.WriteHeader();

            if (ContainsBinary)
                writer.WriteBinaryIndicator();

            // Write Body
            var bodyResult = writer.WriteBody(this.Root);

            // Write Xrefs table - record position for startxref
            writer.WriteLine();
            var xrefLocation = writer.WriteXref(bodyResult);

            // Write trailer
            writer.WriteLine();
            writer.WriteTrailer(this, bodyResult);

            // Write startxref
            writer.WriteLine();
            writer.WriteStartXref(xrefLocation);

            // Write EOF
            writer.WriteEndOfFile();
            writer.Dispose();
        }        

        public void ProcessPages(List<PdfPage> pages, bool enableLinearization = false)
        {
            if (enableLinearization)
                Root.Pages = this.BuildLinearizedPageTree(pages);
            else
                Root.Pages = this.BuildPageTree(pages);

            Root.Pages.ParentCatalog = Root;
        }

        /// <summary>
        /// We need to build the page tree based on inherited resources etc.
        /// </summary>
        /// <param name="pages"></param>
        /// <returns></returns>
        private PageNode BuildPageTree(List<PdfPage> pages)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Linearization requires that each page has it's own resource dictionary
        /// </summary>
        /// <param name="pages"></param>
        /// <returns></returns>
        private PageNode BuildLinearizedPageTree(List<PdfPage> pages)
        {
            var root = new PageNode();

            foreach(var page in pages)
            {
                var pageToAdd = this.BuildPage(page);
                pageToAdd.Resources = page.GetResources();
                pageToAdd.Parent = root;
                root.Kids.Add(pageToAdd);
            }

            return root;
        }

        /// <summary>
        /// Used during page tree building to construct individual pages. Resources should be handled by the caller.
        /// </summary>
        /// <param name="pdfPage"></param>
        /// <returns></returns>
        private Page BuildPage(PdfPage pdfPage)
        {
            var page = new Page();

            page.Contents = this.CompileContent(pdfPage);

            // Fixup relationship
            page.Contents.Parent = page;

            return page;
        }

        private ContentStream CompileContent(PdfPage pdfPage)
        {
            var contents = new ContentStream();

            contents.TextContent.AddRange(pdfPage.TextContent);

            return contents;
        }
    }
}
