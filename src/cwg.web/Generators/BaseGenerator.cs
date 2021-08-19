using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.Cryptography;

using cwg.web.Data;
using cwg.web.Enums;

namespace cwg.web.Generators
{
    public abstract class BaseGenerator
    {
        public abstract string Name { get; }

        protected abstract string SourceName { get; }

        public abstract bool Active { get; }

        protected abstract string CleanSourceName { get; }

        protected abstract string OutputExtension { get; }    

        public abstract bool Packable { get; }

        protected static int GetRandomInt(int min = 1, int max = 100) => new Random((int)DateTime.Now.Ticks).Next(min, max);

        protected static void FillArray(byte[] bytes) => new Random((int)DateTime.Now.Ticks).NextBytes(bytes);

        protected string ComputeSha1(byte[] bytes)
        {
            using var shaManager = new SHA1Managed();

            return BitConverter.ToString(shaManager.ComputeHash(bytes)).Replace("-", "");
        }

        protected string ComputeSha1(string fileName) => ComputeSha1(File.ReadAllBytes(fileName));

        private (string sha1, string fileName) GenerateBytesForFile(string sourceName)
        {
            var originalBytes = File.ReadAllBytes(sourceName).ToList();
            
            var newBytes = new byte[GetRandomInt()];

            FillArray(newBytes);

            for (var y = 0; y < newBytes.Length; y++)
            {
                originalBytes[originalBytes.Count - 1 - y] = newBytes[y];
            }

            var sha1Sum = ComputeSha1(originalBytes.ToArray());

            var fileName = Path.Combine(AppContext.BaseDirectory, $"{sha1Sum}.{OutputExtension}");

            File.WriteAllBytes(fileName, originalBytes.ToArray());

            return (sha1Sum, $"{sha1Sum}.{OutputExtension}");
        }

        protected virtual (string sha1, string fileName) Generate(GenerationRequestModel model) => 
            GenerateBytesForFile(model.ThreatLevelEnum == ThreatLevels.CLEAN ? CleanSourceName : SourceName);

        public (string sha1, string fileName) GenerateFiles(GenerationRequestModel model)
        {
            switch (model.NumberToGenerate)
            {
                case 0:
                    return (null, null);
                case 1:
                    return Generate(model);
            }

            var fileNames = new List<string>();

            for (var x = 0; x < model.NumberToGenerate; x++)
            {
                fileNames.Add(Generate(model).fileName);
            }

            var zipArchiveFileName = Path.Combine(AppContext.BaseDirectory, $"{DateTime.Now.Ticks}.zip");

            using (var fileStream = new FileStream(zipArchiveFileName, FileMode.CreateNew))
            {
                using var archive = new ZipArchive(fileStream, ZipArchiveMode.Create, true);

                foreach (var fileName in fileNames)
                {
                    var zipArchiveEntry = archive.CreateEntry(fileName, CompressionLevel.Fastest);

                    using var zipStream = zipArchiveEntry.Open();

                    var bytes = File.ReadAllBytes(Path.Combine(AppContext.BaseDirectory, fileName));

                    zipStream.Write(bytes, 0, bytes.Length);
                }
            }

            var sha1 = ComputeSha1(File.ReadAllBytes(zipArchiveFileName));

            File.Move(zipArchiveFileName, Path.Combine(AppContext.BaseDirectory, $"{sha1}.zip"));

            return (sha1, $"{sha1}.zip");
        }    
    }
}