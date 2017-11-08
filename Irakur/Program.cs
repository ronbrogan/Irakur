using Irakur.Font;
using System;

namespace Irakur
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var fontPath = @"C:\Windows\Fonts\calibri.ttf";

            var font = FontLoader.FromFile(fontPath);

            Console.WriteLine(font.Version);

            Console.WriteLine(font.GetPointWidthOfChar('Z'));

            Console.ReadLine();
        }
    }
}
