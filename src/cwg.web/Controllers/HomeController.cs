using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using cwg.web.Data;
using cwg.web.Generators;

using Microsoft.AspNetCore.Mvc;

namespace cwg.web.Controllers
{
    public class HomeController : Controller
    {
        private static List<T> GetObjects<T>()
        {
            var types = Assembly.GetExecutingAssembly().GetTypes().Where(a => a.BaseType == typeof(T) && !a.IsAbstract);

            return types.Select(b => (T)Activator.CreateInstance(b)).ToList();
        }

        private static IEnumerable<BaseGenerator> GetGenerators()
        {
            var baseGenerators = GetObjects<BaseGenerator>();

            baseGenerators.AddRange(GetObjects<BaseArchiveGenerator>());

            return baseGenerators.OrderBy(a => a.Name);
        }

        private BaseGenerator getGenerator(string name) => GetGenerators().FirstOrDefault(a => a.Name == name);

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