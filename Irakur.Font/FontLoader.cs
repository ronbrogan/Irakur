using Irakur.Font.Formats;
using Irakur.Font.Formats.TTF;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Irakur.Font
{
    public static class FontLoader
    {

        public static Font FromFile(string path)
        {
            var font = new Font();

            var fontFile = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);

            using (var ttfLoader = new TrueTypeFont(fontFile))
            {
                font = ttfLoader.GenerateFont();
            }

            fontFile.Dispose();

            return font;
        }

    }
}
