using System;
using System.IO;

using Microsoft.AspNetCore.Mvc;

namespace cwg.web.Controllers
{
    public class FileController : Controller
    {
        [Route("file/{filename}")]
        public IActionResult Index(string filename) => 
            new FileContentResult(System.IO.File.ReadAllBytes(Path.Combine(AppContext.BaseDirectory, filename)), "application/octet-stream")
        {
            FileDownloadName = filename
        };
    }
}