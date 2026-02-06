using FileToSQL.Models;

namespace FileToSQL.Services
{
    public class CsvParserService : IFileParserService
    {
        public FilePreviewResult Parse(Stream stream)
        {
            using var reader = new StreamReader(stream);
            using var csv = new CsvHelper.CsvReader(reader, System.Globalization.CultureInfo.InvariantCulture);


            csv.Context.Configuration.MissingFieldFound = null;
            csv.Context.Configuration.BadDataFound = null;
            csv.Context.Configuration.HeaderValidated = null;


            csv.Read();
            csv.ReadHeader();

            var columnNames = csv.HeaderRecord.ToList();
            var rows = new List<List<string>>();

            while (csv.Read())
            {
                var row = new List<string>();

                foreach (var column in columnNames)
                {
                    row.Add(csv.TryGetField(column, out string value) &&
                            !string.IsNullOrWhiteSpace(value)
                            ? value
                            : null);
                }

                rows.Add(row);
            }

            return new FilePreviewResult
            {
                ColumnNames = columnNames,
                Rows = rows
            };
        }
    }
}
