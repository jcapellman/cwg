using System;
using System.IO;
using System.Linq;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using DW = DocumentFormat.OpenXml.Drawing.Wordprocessing;
using A = DocumentFormat.OpenXml.Drawing;
using PIC = DocumentFormat.OpenXml.Drawing.Pictures;

namespace cwg.web.Generators
{
    public class DocxGenerator : BaseGenerator
    {
        public override string Name => "DOCX";

        protected override string SourceName => string.Empty;

        protected override string OutputExtension => "docx";

        private static void AddImageToBody(WordprocessingDocument wordDoc, string relationshipId)
        {
            // Define the reference of the image.
            var element =
                 new Drawing(
                     new DW.Inline(
                         new DW.Extent() { Cx = 990000L, Cy = 792000L },
                         new DW.EffectExtent()
                         {
                             LeftEdge = 0L,
                             TopEdge = 0L,
                             RightEdge = 0L,
                             BottomEdge = 0L
                         },
                         new DW.DocProperties()
                         {
                             Id = (UInt32Value)1U,
                             Name = "Picture 1"
                         },
                         new DW.NonVisualGraphicFrameDrawingProperties(
                             new A.GraphicFrameLocks() { NoChangeAspect = true }),
                         new A.Graphic(
                             new A.GraphicData(
                                 new PIC.Picture(
                                     new PIC.NonVisualPictureProperties(
                                         new PIC.NonVisualDrawingProperties()
                                         {
                                             Id = (UInt32Value)0U,
                                             Name = "New Bitmap Image.jpg"
                                         },
                                         new PIC.NonVisualPictureDrawingProperties()),
                                     new PIC.BlipFill(
                                         new A.Blip(
                                             new A.BlipExtensionList(
                                                 new A.BlipExtension()
                                                 {
                                                     Uri =
                                                        "{28A0092B-C50C-407E-A947-70E740481C1C}"
                                                 })
                                         )
                                         {
                                             Embed = relationshipId,
                                             CompressionState =
                                             A.BlipCompressionValues.Print
                                         },
                                         new A.Stretch(
                                             new A.FillRectangle())),
                                     new PIC.ShapeProperties(
                                         new A.Transform2D(
                                             new A.Offset() { X = 0L, Y = 0L },
                                             new A.Extents() { Cx = 990000L, Cy = 792000L }),
                                         new A.PresetGeometry(
                                             new A.AdjustValueList()
                                         )
                                         { Preset = A.ShapeTypeValues.Rectangle }))
                             )
                             { Uri = "http://schemas.openxmlformats.org/drawingml/2006/picture" })
                     )
                     {
                         DistanceFromTop = (UInt32Value)0U,
                         DistanceFromBottom = (UInt32Value)0U,
                         DistanceFromLeft = (UInt32Value)0U,
                         DistanceFromRight = (UInt32Value)0U,
                         EditId = "50D07946"
                     });

            // Append the reference to body, the element should be in a Run.
            wordDoc.MainDocumentPart.Document.Body.AppendChild(new Paragraph(new Run(element)));
        }

        protected override (string sha1, string fileName) Generate(bool bosartige, string injection)
        {
            var fileName = Path.Combine(AppContext.BaseDirectory, $"{DateTime.Now.Ticks}.docx");

            if (bosartige)
            {
                File.Copy(Path.Combine(AppContext.BaseDirectory, "sourceDOCM"), fileName);

                var originalBytes = System.IO.File.ReadAllBytes(fileName);

                var newBytes = new byte[GetRandomInt()];

                FillArray(newBytes);

                for (var y = 0; y < newBytes.Length; y++)
                {
                    originalBytes[originalBytes.Length - 1 - y] = newBytes[y];
                }

                var bSha1Sum = ComputeSha1(originalBytes);

                File.WriteAllBytes(Path.Combine(AppContext.BaseDirectory, $"{bSha1Sum}.docx"), originalBytes);

                return (bSha1Sum, $"{bSha1Sum}.docx");
            }

            using (var document = WordprocessingDocument.Create(fileName, WordprocessingDocumentType.Document, true))
            {
                var jqueryText = File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "jquery.js"));


                var mainPart = document.AddMainDocumentPart();

                new Document(new Body()).Save(mainPart);

                Body body = mainPart.Document.Body;
                body.Append(new Paragraph(
                    new Run(
                        new Text($"cwg owned this document on {DateTime.Now} {System.Environment.NewLine}\r\n{jqueryText}"))));

                for (var x = 0; x < 10; x++)
                {
                    body.Append(new Paragraph(new Run(new Text("https://wwww.jarredcapellman.com/"))));

                    body.Append(new Paragraph(new Run(new Text($"http://btyl.io/{x}/"))));
                }

                for (var x = 0; x < 10; x++)
                {
                    ImagePart imagePart = mainPart.AddImagePart(ImagePartType.Jpeg);

                    using (FileStream stream = new FileStream(Path.Combine(AppContext.BaseDirectory, "embed.jpg"),
                        FileMode.Open))
                    {
                        imagePart.FeedData(stream);

                        AddImageToBody(document, mainPart.GetIdOfPart(imagePart));
                    }
                }

                mainPart.Document.Save();
            }
        
            var bytes = File.ReadAllBytes(fileName);

            var sha1Sum = ComputeSha1(bytes);

            File.WriteAllBytes(Path.Combine(AppContext.BaseDirectory, $"{sha1Sum}.docx"), bytes);

            return (sha1Sum, $"{sha1Sum}.docx");
        }
    }
}