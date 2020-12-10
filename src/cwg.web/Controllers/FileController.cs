using System;
using System.IO;

using cwg.web.Data;
using cwg.web.Enums;

using Microsoft.AspNetCore.Mvc;

namespace cwg.web.Controllers
{
    public class FileController : ControllerBase
    {
        [Route("file/{filename}")]
        public IActionResult Index(string filename) => 
            new FileContentResult(System.IO.File.ReadAllBytes(Path.Combine(AppContext.BaseDirectory, filename)), "application/octet-stream")
        {
            FileDownloadName = filename
        };

        [Route("file/generate")]
        public IActionResult Generate(string fileType, int numToGenerate = 1, bool bosartige = false, string injection = null, bool repack = false, ThreatLevels threatLevel = ThreatLevels.MALICIOUS)
        {
            var gService = new GeneratorsService();

            var response = gService.GenerateFile(new GenerationRequestModel
            {
                FileType = fileType,
                NumberToGenerate = numToGenerate,
                ThreatLevel = threatLevel.ToString(),
                Bosartige = bosartige,
                Injection = injection,
                Repack = repack
            });

            return Index(response.FileName);
        }
    }
}