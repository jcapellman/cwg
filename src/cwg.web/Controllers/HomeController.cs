using System;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;

using cwg.web.Common;
using cwg.web.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

using VirusTotalNET;
using VirusTotalNET.ResponseCodes;

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
        
        private int getRandomInt(int min = 1, int max = 100) => new Random((int)DateTime.Now.Ticks).Next(min, max);

        private static void FillArray(byte[] bytes)
        {
            new Random((int)DateTime.Now.Ticks).NextBytes(bytes);
        }
        
        [Route("vtinfo/{fileName}")]
        public async Task<PartialViewResult> VirusTotalInformation(string fileName)
        {
            if (string.IsNullOrEmpty(_settingsFile.VTKey))
            {
                return null;
            }

            var virusTotal = new VirusTotal(_settingsFile.VTKey)
            {
                UseTLS = true
            };

            var fileBytes = System.IO.File.ReadAllBytes(Path.Combine(AppContext.BaseDirectory, fileName));

            var scanFileResult = await virusTotal.ScanFileAsync(fileBytes, fileName);

            var result = await virusTotal.GetFileReportAsync(scanFileResult.ScanId);

            while (result.ResponseCode != FileReportResponseCode.Present)
            {
                System.Threading.Thread.Sleep(10000);

                result = await virusTotal.GetFileReportAsync(scanFileResult.ScanId);
            }

            return PartialView(result);
        }

        public IActionResult Generate()
        {
            var originalBytes = System.IO.File.ReadAllBytes("sourcePE");

            var newBytes = new byte[getRandomInt()];
            
            FillArray(newBytes);

            for (var y = 0; y < newBytes.Length; y++)
            {
                originalBytes[originalBytes.Length - 1 - y] = newBytes[y];
            }

            string sha1Sum;

            using (var shaManager = new SHA1Managed())
            {
                sha1Sum = BitConverter.ToString(shaManager.ComputeHash(originalBytes)).Replace("-", "");
            }

            System.IO.File.WriteAllBytes(Path.Combine(AppContext.BaseDirectory, $"{sha1Sum}.exe"), originalBytes);

            return View("Generation", new GenerationResponseModel
            {
                FileName = $"{sha1Sum}.exe",
                SHA1 = sha1Sum
            });
        }
    }
}