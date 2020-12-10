using System;
using System.ComponentModel;
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
        public IActionResult Generate(
            [Description("Options Include: DLL, DOCX, ELF (ARM), ELF (x86), GZIP, HTA (Encrypted), HTA (PE32 Embedded), MACHO, PDF, PE32, PE32+ (IL), PE32+, PE32 SIGNED, Powershell, TAR, XLS, XLSM")]string fileType, 
            [Description("Number of files to Generate")]int numToGenerate = 1, 
            bool bosartige = false, 
            [Description("String to append to the generation")]string injection = null, 
            [Description("Repack with UPX (Only works on PE32/ELF/MACHO)")]bool repack = false, 
            [Description("Threat Level Options: 0 - ABNORMAL, 1 - SUSPICIOUS, 2 - MALICIOUS")]ThreatLevels threatLevel = ThreatLevels.MALICIOUS)
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