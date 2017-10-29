using System;
using System.Security.Cryptography;

using Microsoft.AspNetCore.Mvc;

namespace cwg.web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index() => View();
        
        private int getRandomInt(int min = 1, int max = 100) => new Random((int)DateTime.Now.Ticks).Next(min, max);

        private static void FillArray(byte[] bytes)
        {
            new Random((int)DateTime.Now.Ticks).NextBytes(bytes);
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

            return new FileContentResult(originalBytes, "application/octet-stream")
            {
                FileDownloadName = $"{sha1Sum}.exe"
            };
        }
    }
}