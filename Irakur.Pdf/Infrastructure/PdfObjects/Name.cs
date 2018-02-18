using System;
using System.Collections.Generic;
using System.Text;
using Irakur.Pdf.Infrastructure.Text;

namespace Irakur.Pdf.Infrastructure.PdfObjects
{
    public class Name : IPdfLiteral
    {
        private string name { get; set; }

        public Name(string name)
        {
            this.name = name;
        }

        public override string ToString()
        {
            return this.name;
        }
    }
}
