﻿using Irakur.Font.Formats;
using Irakur.Font.Formats.TTF;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Irakur.Font
{
    public static class FontLoader
    {

        public static IFont FromFile(string path)
        {
            var fontFile = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);

            IFont font = new TrueTypeFont(fontFile);

            fontFile.Dispose();

            return font;
        }

    }
}
