using FileToSQL.Models;
using FileToSQL.Services;
using Microsoft.AspNetCore.Mvc;

namespace FileToSQL.Controllers
{
    [ApiController]
    [Route("api/file-upload")]
    public class FileUploadApiController : ControllerBase
    {
        private readonly DatabaseService _databaseService;

        public FileUploadApiController(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        [HttpPost("upload")]
        public IActionResult ProcessFile([FromForm] UploadRequest request)
        {
            IFileParserService parser = Path.GetExtension(request.File.FileName).ToLower() == ".csv"
                 ? new CsvParserService()
                 : new ExcelParserService();

            var preview = parser.Parse(request.File.OpenReadStream());

            if (request.Mode == 1)
            {
                _databaseService.CreateTableAndInsert(request.DatabaseName,request.TableName, preview);
            }
            else
            {
                _databaseService.InsertData(request.DatabaseName, request.TableName, preview);
            }

            return Ok(new { message = "File processed successfully" });

        }

        [HttpPost("preview")]
        public IActionResult PreviewFile(IFormFile file)
        {
            if (file == null)
                return BadRequest("File is required");

            IFileParserService parser = Path.GetExtension(file.FileName).ToLower() == ".csv"
                ? new CsvParserService()
                : new ExcelParserService();

            var result = parser.Parse(file.OpenReadStream());

            return Ok(result);
        }

    }
}


