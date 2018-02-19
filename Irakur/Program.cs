using Irakur.Core.CoordinateSystem;
using Irakur.Font;
using Irakur.Pdf;
using Irakur.Pdf.Infrastructure.PdfObjects;
using Irakur.Pdf.Infrastructure.Text;
using Irakur.Pdf.Text;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

            GenerateIrakurPdf();

            Console.ReadLine();
        }

        private static void GenerateIrakurPdf()
        {
            var timer = new Stopwatch();
            timer.Start();

            var doc = new PdfDocument(new Size(612, 792));

            var img = Image.Load<Rgb24>("D:\\test.png");

            for (var p = 0; p < 1000; p++)
            {
                var page = new PdfPage();

                page.ImageContent.Add(new ImageContent(img, 32, 32));

                for (var t = 0; t < 20; t++)
                {
                    page.TextContent.Add(new TextContent()
                    {
                        Font = FontFactory.Standard(StandardFont.Helvetica),
                        Rectangle = new Rectangle(36, 792 - t * 36, 150),
                        Text = "This is a test PDF used for demoing features of Irakur.Pdf"
                    });
                }

                doc.Pages.Add(page);
            }

            var docStream = new FileStream("D:\\irakur.pdf", FileMode.Create);

            doc.Save(docStream);

            timer.Stop();

            docStream.Dispose();
            img.Dispose();

            Console.WriteLine($"PDF Generation took: {timer.Elapsed.TotalMilliseconds}ms (Irakur)");
        }
    }
}
