using System;
using System.IO;

using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace cwg.web.Generators
{
    public class XLSXGenerator : BaseGenerator
    {
        public override string GetName() => "XLSM";

        public override string GetSourceName() => "sourceXLSM";

        protected override (string sha1, string fileName) Generate()
        {
            var fileName = Path.Combine(AppContext.BaseDirectory, $"{DateTime.Now.Ticks}.xlsm");

            File.Copy(Path.Combine(AppContext.BaseDirectory, GetSourceName()), fileName);

            var spreadsheetDocument = SpreadsheetDocument.Open(fileName, true);

            spreadsheetDocument.AddAnnotation(new Text($"Owned by CWG on {DateTime.Now}"));

            spreadsheetDocument.Close();

            spreadsheetDocument.Dispose();
            spreadsheetDocument = null;

            var bytes = File.ReadAllBytes(fileName);

            var sha1Sum = ComputeSha1(bytes);

            File.WriteAllBytes(Path.Combine(AppContext.BaseDirectory, $"{sha1Sum}.xlsm"), bytes);

            return (sha1Sum, $"{sha1Sum}.xlsm");
        }
    }
}