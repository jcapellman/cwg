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

        private static List<T> GetObjects<T>()
        {
            var types = Assembly.GetExecutingAssembly().GetTypes().Where(a => a.BaseType == typeof(T) && !a.IsAbstract);

            return types.Select(b => (T) Activator.CreateInstance(b)).ToList();
        }

        private static IEnumerable<BaseGenerator> GetGenerators()
        {
            var baseGenerators = GetObjects<BaseGenerator>();

            baseGenerators.AddRange(GetObjects<BaseArchiveGenerator>());

            return baseGenerators.OrderBy(a => a.Name);
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