using FileToSQL.Models;

namespace FileToSQL.Services
{
    public interface IFileParserService
    {
        FilePreviewResult Parse(Stream fileStream);
    }
}
