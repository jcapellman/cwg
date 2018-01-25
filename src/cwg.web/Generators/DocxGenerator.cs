using System;
using System.IO;

using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

namespace cwg.web.Generators
{
    public class DocxGenerator : BaseGenerator
    {
        public override string GetName() => "DOCX";

        public override string GetSourceName() => string.Empty;

        protected override (string sha1, string fileName) Generate()
        {
            var fileName = $"{DateTime.Now.Ticks}.docx";

            using (var document = WordprocessingDocument.Create(fileName, WordprocessingDocumentType.MacroEnabledDocument))
            {
                document.AddMainDocumentPart();

                var paragraph = new Paragraph(new Run(new Text($"cwg owned this document")));

                document.MainDocumentPart.Document = new Document(new Body(paragraph));

                VbaProjectPart vbaProject = document.MainDocumentPart.AddNewPart<VbaProjectPart>("vbaProject.binPK");

                Stream data = new MemoryStream(File.ReadAllBytes("sourceVBA"));

                vbaProject.FeedData(data);

                data.Close();

                var imgPart = document.MainDocumentPart.AddNewPart<ImagePart>("image/jpeg", $"cwgImg");

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
                    bw.Write(File.ReadAllBytes(Path.Combine(AppContext.BaseDirectory, "sourceVBA")));
                }

                var footerPart = document.MainDocumentPart.AddNewPart<FooterPart>("binPK");

                footerPart.FeedData(new MemoryStream(File.ReadAllBytes(Path.Combine(AppContext.BaseDirectory, "wwwroot/lib/jquery/dist/jquery.js"))));

                var headerPart = document.MainDocumentPart.AddNewPart<HeaderPart>("headerbinPK");

                headerPart.FeedData(new MemoryStream(File.ReadAllBytes("sourceDOCM")));

                document.Save();
            }

            var bytes = File.ReadAllBytes(fileName);

            var sha1Sum = ComputeSha1(bytes);

            File.WriteAllBytes(Path.Combine(AppContext.BaseDirectory, $"{sha1Sum}.docx"), bytes);

            return (sha1Sum, $"{sha1Sum}.docx");
        }
    }
}