using System;
using System.Collections.Generic;
using System.Text;
using Irakur.Pdf.Infrastructure.Collections;
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
            foreach (var text in contentStream.TextContent)
            {
                streamData.AppendLine(PdfTokens.Text.Begin);
                streamData.AppendLine($"{text.FillColor.Red} {text.FillColor.Green} {text.FillColor.Blue} {PdfTokens.Graphics.Color.Operators.SetFillRGB}");
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
