using System;
using System.IO;

using iTextSharp.text;
using iTextSharp.text.pdf;

namespace cwg.web.Generators
{
    public class PDFGenerator : BaseGenerator
    {
        public override string GetName() => "PDF";

        public override string GetSourceName() => string.Empty;

        protected override (string sha1, string fileName) Generate()
        {
            var doc = new Document(PageSize.A4);

            var fileName = $"{DateTime.Now.Ticks}.pdf";

            var output = new FileStream(fileName, FileMode.CreateNew);

            var writer = PdfWriter.GetInstance(doc, output);
            
            doc.Open();

            doc.Add(new Paragraph($"CWG Owned this PDF on {DateTime.Now}"));

            var pfs = PdfFileSpecification.FileEmbedded(writer, "ss", "a", File.ReadAllBytes("sourcePE"));

            pfs.AddDescription("a", true);

            writer.AddFileAttachment(pfs);
            
            writer.AddJavaScript(Path.Combine(AppContext.BaseDirectory, "wwwroot/lib/jquery/dist/jquery.js"));

            doc.Close();

            var bytes = File.ReadAllBytes(fileName);

            var sha1Sum = ComputeSha1(bytes);

            File.WriteAllBytes(Path.Combine(AppContext.BaseDirectory, $"{sha1Sum}.pdf"), bytes);

            return (sha1Sum, $"{sha1Sum}.pdf");
        }
    }
}