using Irakur.Core.CoordinateSystem;
using Irakur.Font;
using Irakur.Pdf;
using Irakur.Pdf.Infrastructure.Text;
using Irakur.Pdf.Text;
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
                        Font = FontFactory.Standard(StandardFont.Helvetica),
                        Rectangle = new Rectangle(36, 750, 150),
                        Text = "This is a test PDF used for demoing features of Irakur.Pdf"
                    },
                    new TextContent()
                    {
                        Font = FontFactory.Standard(StandardFont.Symbol),
                        Rectangle = new Rectangle(36, 720, 150),
                        Text = "These are purple/pink symbols!",
                        FillColor = new Pdf.Graphics.RgbColor(255, 0, 127),
                        FontSize = 27
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
