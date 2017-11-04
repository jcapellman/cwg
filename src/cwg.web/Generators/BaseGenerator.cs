using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;

namespace cwg.web.Generators
{
    public abstract class BaseGenerator
    {
        public abstract string GetName();

        public abstract string GetSourceName();
        
        protected int getRandomInt(int min = 1, int max = 100) => new Random((int)DateTime.Now.Ticks).Next(min, max);

        protected static void FillArray(byte[] bytes)
        {
            new Random((int)DateTime.Now.Ticks).NextBytes(bytes);
        }

        protected string ComputeSha1(byte[] bytes)
        {
            using (var shaManager = new SHA1Managed())
            {
                return BitConverter.ToString(shaManager.ComputeHash(bytes)).Replace("-", "");
            }
        }

        protected (string sha1, string fileName) Generate()
        {
            var originalBytes = System.IO.File.ReadAllBytes(GetSourceName());

            var newBytes = new byte[getRandomInt()];

            FillArray(newBytes);

            for (var y = 0; y < newBytes.Length; y++)
            {
                originalBytes[originalBytes.Length - 1 - y] = newBytes[y];
            }

            var sha1Sum = ComputeSha1(originalBytes);

            System.IO.File.WriteAllBytes(Path.Combine(AppContext.BaseDirectory, $"{sha1Sum}.exe"), originalBytes);

            return (sha1Sum, $"{sha1Sum}.exe");
        }

        public (string sha1, string fileName) GenerateFiles(int numberToGenerate)
        {
            switch (numberToGenerate)
            {
                case 0:
                    return (null, null);
                case 1:
                    return Generate();
            }

            var fileNames = new List<string>();

            for (var x = 0; x < numberToGenerate; x++)
            {
                fileNames.Add(Generate().fileName);
            }

            var zipArchiveFileName = Path.Combine(AppContext.BaseDirectory, $"{DateTime.Now.Ticks}.zip");

            using (var fileStream = new FileStream(zipArchiveFileName, FileMode.CreateNew))
            {
                using (var archive = new ZipArchive(fileStream, ZipArchiveMode.Create, true))
                {
                    foreach (var fileName in fileNames)
                    {
                        var zipArchiveEntry = archive.CreateEntry(fileName, CompressionLevel.Fastest);

                        using (var zipStream = zipArchiveEntry.Open())
                        {
                            var bytes = File.ReadAllBytes(Path.Combine(AppContext.BaseDirectory, fileName));

                            zipStream.Write(bytes, 0, bytes.Length);
                        }
                    }                                    
                }
            }

            var sha1 = ComputeSha1(File.ReadAllBytes(zipArchiveFileName));

            File.Move(zipArchiveFileName, Path.Combine(AppContext.BaseDirectory, $"{sha1}.zip"));

            return (sha1, $"{sha1}.zip");
        }    
    }
}