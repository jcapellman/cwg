using System;
using System.IO;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace cwg.web.Generators
{
    public class XLSXGenerator : BaseGenerator
    {
        public override string GetName() => "XLSX";

        public override string GetSourceName() => string.Empty;

        protected override (string sha1, string fileName) Generate()
        {
            var fileName = $"{DateTime.Now.Ticks}.xlsx";

            var spreadsheetDocument = SpreadsheetDocument.Create(fileName, SpreadsheetDocumentType.Workbook);

            WorkbookPart workbookpart = spreadsheetDocument.AddWorkbookPart();
            workbookpart.Workbook = new Workbook();

            workbookpart.Workbook.Append(new Text(fileName));

            WorksheetPart worksheetPart = workbookpart.AddNewPart<WorksheetPart>();
            worksheetPart.Worksheet = new Worksheet(new SheetData());

            Sheets sheets = spreadsheetDocument.WorkbookPart.Workbook.AppendChild<Sheets>(new Sheets());

            var jqueryText = File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "wwwroot/lib/jquery/dist/jquery.js"));
            var sourceText = Convert.ToBase64String(File.ReadAllBytes(Path.Combine(AppContext.BaseDirectory, "sourcePE")));
            
            for (uint x = 0; x < 100; x++)
            {
                Sheet sheet = new Sheet() { Id = spreadsheetDocument.WorkbookPart.GetIdOfPart(worksheetPart), SheetId = x, Name = $"mySheet{x}" };
                
                sheets.Append(sheet);

                Worksheet worksheet = new Worksheet();
                SheetData sheetData = new SheetData();
                Row row = new Row();

                for (uint y = 1; y < 1000; y++)
                {
                    Cell cell = new Cell()
                    {
                        CellReference = $"A{y}",
                        DataType = CellValues.String,
                        CellValue = new CellValue(sourceText)
                    };

                    row.Append(cell);
                }

                sheetData.Append(row);
                worksheet.Append(sheetData);
                worksheetPart.Worksheet = worksheet;
            }

            workbookpart.Workbook.Save();

            spreadsheetDocument.Close();


            var bytes = File.ReadAllBytes(fileName);

            var sha1Sum = ComputeSha1(bytes);

            File.WriteAllBytes(Path.Combine(AppContext.BaseDirectory, $"{sha1Sum}.xlsx"), bytes);

            return (sha1Sum, $"{sha1Sum}.xlsx");
        }
    }
}
