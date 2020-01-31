using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using cwg.web.Generators;
using cwg.web.Models;

using Microsoft.AspNetCore.Mvc;

namespace cwg.web.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        [HttpPost]
        public IActionResult Generate(int numberToGenerate, string fileType)
        {
            var generator = getGenerator(fileType);

            if (generator == null)
            {
                throw new Exception($"{fileType} was not found");
            }

            var (sha1, fileName) = generator.GenerateFiles(numberToGenerate);
            
            return View("Generation", new GenerationResponseModel
            {
                FileName = fileName,
                SHA1 = sha1,
                FileType = fileType
            });
        }
    }
}