using cwg.web.Common;
using cwg.web.Generators;
using cwg.web.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace cwg.web.Controllers
{
    public class HomeController : Controller
    {
        private readonly Settings _settingsFile;

        public HomeController(IOptions<Settings> settingsFile)
        {
            _settingsFile = settingsFile.Value;
        }

        public IActionResult Index() => View();
        
        [HttpGet]
        [HttpPost]
        public IActionResult Generate(int numberToGenerate, string fileType)
        {
            var (sha1, fileName) = (string.Empty, string.Empty);

            switch (fileType)
            {
                case "PE32":
                    (sha1, fileName) = new PE32Generator().GenerateFiles(numberToGenerate);
                    break;
            }
            
            return View("Generation", new GenerationResponseModel
            {
                FileName = fileName,
                SHA1 = sha1,
                FileType = fileType
            });
        }
    }
}