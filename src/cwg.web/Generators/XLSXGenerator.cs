using System;
using System.IO;
using System.Linq;
    
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace cwg.web.Generators
{
    public class XLSXGenerator : BaseGenerator
    {
        public override string Name => "XLSM";
        protected override string SourceName => "sourceXLSM";
        protected override string OutputExtension => "xlsm";

        protected override (string sha1, string fileName) Generate(bool bosartige)
        {
            var fileName = Path.Combine(AppContext.BaseDirectory, $"{DateTime.Now.Ticks}.xlsm");
            uint sheetId = 1;

            File.Copy(Path.Combine(AppContext.BaseDirectory, SourceName), fileName);

            var spreadsheetDocument = SpreadsheetDocument.Open(fileName, true);

            var sheetPart = spreadsheetDocument.WorkbookPart.AddNewPart<WorksheetPart>();
            var sheetData = new SheetData();
            sheetPart.Worksheet = new Worksheet(sheetData);

            var sheets = spreadsheetDocument.WorkbookPart.Workbook.GetFirstChild<DocumentFormat.OpenXml.Spreadsheet.Sheets>();
            string relationshipId = spreadsheetDocument.WorkbookPart.GetIdOfPart(sheetPart);

            if (sheets.Elements<Sheet>().Any())
            {
                sheetId = sheets.Elements<Sheet>().Select(s => s.SheetId.Value).Max() + 1;
            }

            Sheet sheet = new Sheet() { Id = relationshipId, SheetId = sheetId, Name = $"Owned by CWG on {DateTime.Now}" };
            sheets.Append(sheet);
            
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