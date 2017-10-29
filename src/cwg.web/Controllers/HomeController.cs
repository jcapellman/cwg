using System;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;

using cwg.web.Common;
using cwg.web.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

using VirusTotalNET;
using VirusTotalNET.Results;

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
        
        private async Task<FileReport> getAVListAsync(byte[] fileBytes)
        {
            try
            {
                var virusTotal = new VirusTotal(_settingsFile.VTKey)
                {
                    UseTLS = true
                };

                var result = await virusTotal.GetFileReportAsync(fileBytes);

                return result.Scans == null ? null : result;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<IActionResult> Generate()
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
                FileReportResult = await getAVListAsync(originalBytes)
            });
        }
    }
}