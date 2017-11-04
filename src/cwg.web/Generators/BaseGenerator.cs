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

        protected abstract (string sha1, string fileName) Generate();

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