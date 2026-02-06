using FileToSQL.Models;
using OfficeOpenXml;

namespace FileToSQL.Services
{
    public class ExcelParserService : IFileParserService
    {
        public FilePreviewResult Parse(Stream stream)
        {
           ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using var package = new ExcelPackage(stream);
            var sheet = package.Workbook.Worksheets[0];

            var columnNames = new List<string>();
            var rows = new List<List<string>>();

            for (int col = 1; col <= sheet.Dimension.End.Column; col++)
            {
                columnNames.Add(sheet.Cells[1, col].Text);
            }

            for (int row = 2; row <= sheet.Dimension.End.Row; row++)
            {
                var rowData = new List<string>();
                for (int col = 1; col <= sheet.Dimension.End.Column; col++)
                {
                    rowData.Add(sheet.Cells[row, col].Text);
                }
                rows.Add(rowData);
            }

            return new FilePreviewResult
            {
                ColumnNames = columnNames,
                Rows = rows
            };
        }
    }
}
