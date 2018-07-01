using System;
using System.IO;

using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

namespace cwg.web.Generators
{
    public class DocxGenerator : BaseGenerator
    {
        public override string Name => "DOCX";

        protected override string SourceName => string.Empty;

        protected override string OutputExtension => "docx";

        protected override (string sha1, string fileName) Generate()
        {
            var fileName = $"{DateTime.Now.Ticks}.docx";

            using (var document = WordprocessingDocument.Create(fileName, WordprocessingDocumentType.Document))
            {
                document.AddMainDocumentPart();

                var paragraph = new Paragraph(new Run(new Text($"CWG owned this docx on {DateTime.Now}")));
                
                paragraph.AppendChild(new Run(new Text(File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "sourceVBA")))));

                document.MainDocumentPart.Document = new Document(new Body(paragraph));

                var imgPart = document.MainDocumentPart.AddNewPart<ImagePart>("image/jpeg", "cwgImg");

                using (Stream image = new FileStream(Path.Combine(AppContext.BaseDirectory, "sourcePE"), FileMode.Open,
                    FileAccess.Read, FileShare.Read))
                {
                    imgPart.FeedData(image);
                }

                var excelMacroPart = document.MainDocumentPart.AddNewPart<EmbeddedPackagePart>(
                        "application/vnd.openxmlformats-" +
                        "officedocument.spreadsheetml.sheet",
                        "rExcel");

                using (var bw = new BinaryWriter(excelMacroPart.GetStream()))
                {
                    bw.Write(File.ReadAllBytes(Path.Combine(AppContext.BaseDirectory, "macroVBA")));
                }

                document.MainDocumentPart.Document.AppendChild(new Body(new Paragraph(new Run(new Text(File.ReadAllText(Path.Combine(AppContext.BaseDirectory,
                        "wwwroot/lib/jquery/dist/jquery.js")))))));

            }

            var bytes = File.ReadAllBytes(fileName);

            var sha1Sum = ComputeSha1(bytes);

            File.WriteAllBytes(Path.Combine(AppContext.BaseDirectory, $"{sha1Sum}.docx"), bytes);

            return (sha1Sum, $"{sha1Sum}.docx");
        }
    }
}