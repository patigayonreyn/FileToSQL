namespace FileToSQL.Models
{
    public class FilePreviewResult
    {
        public List<string> ColumnNames { get; set; }
        public List<List<string>> Rows { get; set; }
    }
}
