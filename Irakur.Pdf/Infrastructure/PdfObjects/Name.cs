using System;
using System.Collections.Generic;
using System.Text;
using Irakur.Pdf.Infrastructure.Text;

namespace Irakur.Pdf.Infrastructure.PdfObjects
{
    public class Name : PdfObject
    {
        private string name { get; set; }

        public override PdfObjectType Type => PdfObjectType.Name;

        public Name(string name)
        {

        }

        public Name(Enum enumValue)
        {
            this.name = enumValue.ToString();
        }

        public override string ToString()
        {
            return this.name;
        }
    }
}
