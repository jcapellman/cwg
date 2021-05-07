using System;
using System.Globalization;
using System.IO;

using cwg.web.Data;

using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

namespace cwg.web.Generators
{
    public class DocxGenerator : BaseGenerator
    {
        public override string Name => "DOCX";

        protected override string SourceName => string.Empty;

        protected override string CleanSourceName => "sourceCleanDOCX.docx";

        protected override string OutputExtension => "docx";

        public override bool Packable => false;
        
        protected override (string sha1, string fileName) Generate(GenerationRequestModel model)
        {
            try
            {
                var fileName = Path.Combine(AppContext.BaseDirectory, $"{DateTime.Now.Ticks}.dotm");

                File.Copy(Path.Combine(AppContext.BaseDirectory, "sourceDOCM"), fileName);

                var document = WordprocessingDocument.Open(fileName, true);

                var body = document.MainDocumentPart.Document.Body;

                var para = body.AppendChild(new Paragraph());
                var run = para.AppendChild(new Run());
                run.AppendChild(new Text($"0wn3d by Swifty on {DateTime.Now.ToString(CultureInfo.InvariantCulture)}"));

                document.Save();
                document.Close();

                var originalBytes = File.ReadAllBytes(fileName);

                var bSha1Sum = ComputeSha1(originalBytes);

                File.WriteAllBytes(Path.Combine(AppContext.BaseDirectory, $"{bSha1Sum}.docx"), originalBytes);

                return (bSha1Sum, $"{bSha1Sum}.docx");
            }
            catch (Exception ex)
            {
                var te = ex;

                return (null, null);
            }
        }
    }
}