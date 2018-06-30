using System;
using System.IO;

using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

namespace cwg.sourceDOCX
{
    class Program
    {
        private static Stream GetBinaryDataStream(byte[] baseData) => new MemoryStream(baseData);
        
        static void Main(string[] args)
        {
            using (var document = WordprocessingDocument.Create("sourceDOCX", WordprocessingDocumentType.MacroEnabledDocument))
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

                footerPart.FeedData(new MemoryStream(File.ReadAllBytes(Path.Combine(AppContext.BaseDirectory,"wwwroot/lib/jquery/dist/jquery.js"))));

                var headerPart = document.MainDocumentPart.AddNewPart<HeaderPart>("headerbinPK");

                headerPart.FeedData(new MemoryStream(File.ReadAllBytes("sourceDOCM")));
                
                document.Save();
            }
        }
    }
}