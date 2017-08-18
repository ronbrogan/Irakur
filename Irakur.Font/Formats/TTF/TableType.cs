using System;
using System.Collections.Generic;
using System.Text;

namespace Irakur.Font.Formats.TTF
{
    public enum TableType: uint
    {
        // Required Entries
        CharacterToGlyphMap = 0x636D6170,
        GlyphData = 0x676C7966,
        FontHeader = 0x68656164,
        HorizontalHeader = 0x68686561,
        HorizontalMetrics = 0x686D7478,
        IndexToLocation = 0x6C6F6361,
        MaximumProfile = 0x6D617870,
        NamingTable = 0x6E616D65,
        PostScripInfo = 0x706F7374,
        OS2Metrics = 0x4F532F32,

        ControlValue = 0x63767420,
        EmbeddedBitmap = 0x45424454,
        EmbeddedBitmapLocation = 0x45424C43,
        EmbeddedBitmapScale = 0x45425343,
        FontProgram = 0x6670676D,
        GridFitScanConversionProc = 0x67617370,
        HorizontalDeviceMetrics = 0x68646D78,
        Kerning = 0x6B65726E,
        LinearThreshold = 0x4C545348,
        CVTProgram = 0x70726570,
        PCL5 = 0x50434C54,
        VerticalDeviceMetrics = 0x56444D58,
        VerticalMetricsHeader = 0x76686561,
        VerticalMetrics = 0x766D7478

    }
}
