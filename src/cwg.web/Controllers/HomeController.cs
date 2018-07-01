using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

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

        private static IEnumerable<BaseGenerator> GetGenerators()
        {
            var types = Assembly.GetExecutingAssembly().GetTypes().ToList().Where(b => b.BaseType == typeof(BaseGenerator) && !b.IsAbstract).ToList();

            return types.Select(a => (BaseGenerator) Activator.CreateInstance(a)).ToList();
        }

        public IActionResult Index() => View(GetGenerators().Select(a => a.Name).ToList());

        private BaseGenerator getGenerator(string name) => GetGenerators().FirstOrDefault(a => a.Name == name);

        [HttpGet]
        [HttpPost]
        public IActionResult Generate(int numberToGenerate, string fileType)
        {
            var (sha1, fileName) = (string.Empty, string.Empty);

            var generator = getGenerator(fileType);

            if (generator == null)
            {
                throw new Exception($"{fileType} was not found");
            }

            (sha1, fileName) = generator.GenerateFiles(numberToGenerate);
            
            return View("Generation", new GenerationResponseModel
            {
                FileName = fileName,
                SHA1 = sha1,
                FileType = fileType
            });
        }
    }
}