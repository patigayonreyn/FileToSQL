using Microsoft.AspNetCore.Mvc;

namespace FileToSQL.Controllers
{
    public class FileUploadController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
