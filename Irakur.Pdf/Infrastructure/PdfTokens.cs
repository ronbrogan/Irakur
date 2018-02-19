using System;
using System.Collections.Generic;
using System.Text;

namespace Irakur.Pdf.Infrastructure
{
    internal static class PdfTokens
    {
        internal const string FileHeaderPrefix = "%PDF";
        internal const string FileHeaderVersion17 = "-1.7";
        internal static string BinaryContentIndicator = new string(new char[] { '%', (char)207, (char)211, (char)226, (char)227 });
        internal const string StartDictionary = "<<";
        internal const string EndDictionary = ">>";
        internal const string Xref = "xref";
        internal const string Trailer = "trailer";
        internal const string StartXref = "startxref";
        internal const string EndOfFile = "%%EOF";
        internal const string StartObject = "obj";
        internal const string EndObject = "endobj";


        internal static class Text
        {
            internal const string Begin = "BT";
            internal const string End = "ET";

            internal static class ShowingOperators
            {
                internal const string Show = "Tj";
                internal const string NextLineAndShow = "'";
                internal const string NextLineAndShowWithSpacing = "\"";
                internal const string ShowWithGlyphSpacing = "TJ";
            }

            internal static class PositioningOperators
            {
                internal const string MoveFromCurrent = "Td";
                internal const string MoveFromCurrentWithLeading = "TD";
                internal const string SetTransformMatrix = "Tm";
                internal const string MoveToNextLine = "T*";
            }

            internal static class StateParameters
            {
                internal const string CharacterSpacing = "Tc";
                internal const string WordSpacing = "Tw";
                internal const string HorizontalScaling = "Th";
                internal const string Leading = "Tl";
                internal const string Font = "Tf";
                internal const string FontSize = "Tfs";
                internal const string RenderingMode = "Tmode";
                internal const string Rise = "Trise";
                internal const string Knockout = "Tk";
            }

            /// <summary>
            /// These operations may appear outside text objects and retain across text objects in a single content stream.
            /// </summary>
            internal static class StateOperators
            {
                internal const string CharacterSpace = "Tc";
                internal const string WordSpace = "Tw";
                internal const string Scale = "Tz";
                internal const string Leading = "TL";
                internal const string FontSize = "Tf";
                internal const string RenderingMode = "Tr";
                internal const string Rise = "Ts";
            }
        }

        internal static class Stream
        {
            internal const string Start = "stream";
            internal const string End = "endstream";
        }

        internal static class Graphics
        {
            internal static class ColorOperators
            {
                internal const string SetStrokeRGB = "RG";
                internal const string SetFillRGB = "rg";
            }

            internal static class StateOperators
            {
                internal static string Save = "q";
                internal static string Restore = "Q";
            }

            internal static class LineOperators
            {
                internal static string Width = "w";
                internal static string Cap = "J";
                internal static string Join = "j";
                internal static string miterLimit = "M";
            }

            internal static class ImageOperators
            {
                internal static string Draw = "Do";
            }

            internal static class TransformOperators
            {
                internal static string ModifyTransformationMatrix = "cm";
            }
        }
    }
}
