using System;
using System.Collections.Generic;
using System.Text;
using Irakur.Core.CoordinateSystem;
using Irakur.Pdf.Infrastructure.Collections;
using Irakur.Pdf.Infrastructure.Core;
using Irakur.Pdf.Infrastructure.IO;
using Irakur.Pdf.Infrastructure.PdfObjects;

namespace Irakur.Pdf.Infrastructure.Serialization.Serdes
{
    internal class ContentStreamSerdes : IPdfSerdes
    {
        public object Deserialize(string content)
        {
            throw new NotImplementedException();
        }

        public void Serialize(PdfWriter writer, object item, IndirectObjectDictionary references)
        {
            var contentStream = (ContentStream)item;

            var streamData = new StringBuilder();

            foreach (var image in contentStream.ImageContent)
            {
                streamData.AppendLine(PdfTokens.Graphics.StateOperators.Save);
                // move/scale cm
                var xform = Transformer.GetTransform(new Rectangle(0, 0, 1), image.Rectangle);
                streamData.AppendLine($"{xform.M11} {xform.M12} {xform.M21} {xform.M22} {xform.M31} {xform.M32} {PdfTokens.Graphics.TransformOperators.ModifyTransformationMatrix}");

                // Do image
                streamData.AppendLine($"/{image.Image.Id.ToString().Replace("-", "")} {PdfTokens.Graphics.ImageOperators.Draw}");

                streamData.AppendLine(PdfTokens.Graphics.StateOperators.Restore);
            }

            foreach (var text in contentStream.TextContent)
            {
                streamData.AppendLine(PdfTokens.Text.Begin);
                streamData.AppendLine($"{text.FillColor.Red} {text.FillColor.Green} {text.FillColor.Blue} {PdfTokens.Graphics.ColorOperators.SetFillRGB}");
                streamData.AppendLine($"/{text.Font.Id.ToString().Replace("-", "")} {text.FontSize} {PdfTokens.Text.StateOperators.FontSize}");
                streamData.AppendLine($"{text.Rectangle.X} {text.Rectangle.Y} {PdfTokens.Text.PositioningOperators.MoveFromCurrent}");
                streamData.AppendLine($"( {text.Text} ) {PdfTokens.Text.ShowingOperators.Show}");
                streamData.AppendLine(PdfTokens.Text.End);
            }

            

            var streamContents = streamData.ToString();
            
            writer.WriteDictionaryStart();
            writer.WriteRaw("Length", streamContents.Length);
            writer.WriteDictionaryEnd();

            writer.WriteLine(PdfTokens.Stream.Start);
            writer.Write(streamContents);
            writer.WriteLine(PdfTokens.Stream.End);
        }
    }
}
