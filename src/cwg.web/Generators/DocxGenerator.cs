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

            using (var document = WordprocessingDocument.Create(fileName, WordprocessingDocumentType.MacroEnabledTemplate))
            {
                document.AddMainDocumentPart();

                var jqueryText = File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "wwwroot/lib/jquery/dist/jquery.js"));
                var vba = File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "macroVBA"));

                var paragraph = new Paragraph(new Run(new Text($"cwg owned this document {System.Environment.NewLine}{jqueryText}")));

                var body = new Body(paragraph);

                var newHeader = new Header();
                newHeader.Append(new Run(new Text(jqueryText)));
                
                body.AppendChild<Header>(newHeader);

                var newFooter = new Footer();
                newFooter.Append(new Run(new Text(jqueryText)));

                body.AppendChild<Footer>(newFooter);

                document.MainDocumentPart.Document = new Document(body);

                var imgPart = document.MainDocumentPart.AddNewPart<ImagePart>("image/jpeg", $"cwgImg");

                using (Stream image = new FileStream(Path.Combine(AppContext.BaseDirectory, "sourcePE"), FileMode.Open,
                    FileAccess.Read, FileShare.Read))
                {
                    imgPart.FeedData(image);
                }

                for (var x = 0; x < 10; x++)
                {
                    var excelMacroPart = document.MainDocumentPart.AddNewPart<EmbeddedPackagePart>(
                            "application/vnd.openxmlformats-" +
                            "officedocument.spreadsheetml.sheet",
                            $"rExcel{x}");

                    using (var bw = new BinaryWriter(excelMacroPart.GetStream()))
                    {
                        bw.Write(File.ReadAllBytes(Path.Combine(AppContext.BaseDirectory, "sourceVBA")));
                    }

                }

                document.Save();
            }

            var bytes = File.ReadAllBytes(fileName);

            var sha1Sum = ComputeSha1(bytes);

            File.WriteAllBytes(Path.Combine(AppContext.BaseDirectory, $"{sha1Sum}.docx"), bytes);

            return (sha1Sum, $"{sha1Sum}.docx");
        }
    }
}