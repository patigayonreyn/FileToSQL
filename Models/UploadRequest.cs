namespace FileToSQL.Models
{
    public class UploadRequest
    {
        public IFormFile File { get; set; }
        public string TableName { get; set; }
        public string DatabaseName { get; set; }
        public int Mode { get; set; } // 1=create table; 2=append to table


    }
}
