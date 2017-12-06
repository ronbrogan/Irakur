using Irakur.Core.CoordinateSystem;
using Irakur.Font;
using Irakur.Pdf;
using System;
using System.Collections.Generic;
using System.IO;

namespace Irakur
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var fontPath = @"C:\Windows\Fonts\Calibri.ttf";

            var font = FontLoader.FromFile(fontPath);

            Console.WriteLine(font.Version);

            Console.WriteLine(font.GetPixelWidthOfString("Abcdefg", 12));

            var doc = new PdfDocument(new Size(612, 792));

            doc.Pages.Add(new PdfPage()
            {
                TextContent = new List<TextContent>()
                {
                    new TextContent()
                    {
                        FontName = "Helvetica",
                        Rectangle = new Rectangle(300, 300, 150),
                        Text = "This is a test"
                    }
                }
            });

            var docStream = new FileStream("D:\\irakur.pdf", FileMode.Create);

            doc.Save(docStream);

            docStream.Dispose();

            Console.ReadLine();
        }
    }
}
