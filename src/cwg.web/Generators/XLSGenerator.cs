using System;
using System.IO;

using NPOI.HSSF.UserModel;

namespace cwg.web.Generators
{
    public class XLSGenerator : BaseGenerator
    {
        public override string GetName() => "XLS";

        public override string GetSourceName() => string.Empty;

        protected override (string sha1, string fileName) Generate()
        {
            var vbaMacro = File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "macroVBA"));

            var fileName = Path.Combine(AppContext.BaseDirectory, $"{DateTime.Now.Ticks}.xls");

            var workbook = new HSSFWorkbook();
            var worksheet = workbook.CreateSheet("Sheet1");

            for (int rownum = 0; rownum < 10; rownum++)
            {
                var row = worksheet.CreateRow(rownum);

                for (int celnum = 0; celnum < 5; celnum++)
                {
                    var Cell = row.CreateCell(celnum);
                    Cell.SetCellValue($"{vbaMacro} owned by cwg {DateTime.Now}");
                }
            }

            FileStream sw = File.Create(fileName);

            workbook.Write(sw);

            sw.Close();

            var bytes = File.ReadAllBytes(fileName);

            var sha1Sum = ComputeSha1(bytes);

            File.WriteAllBytes(Path.Combine(AppContext.BaseDirectory, $"{sha1Sum}.xls"), bytes);

            return (sha1Sum, $"{sha1Sum}.xls");
        }
    }
}