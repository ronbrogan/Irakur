﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Irakur.Font.Formats.TTF.Tables.CharacterToGlyph
{
    internal class EncodingTableEntry
    {
        public Platform Platform { get; set; }

        public Encoding Encoding { get; set; }

        public uint Offset { get; set; }
    }
}
