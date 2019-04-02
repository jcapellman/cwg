using System;
using System.IO;
using System.IO.Compression;

namespace cwg.web.Generators
{
    public class ZIPGenerator : BaseGenerator
    {
        public override string Name => "ZIP";

        protected override string SourceName => "sourcePE.exe";

        protected override string OutputExtension => "zip";

        protected override (string sha1, string fileName) Generate()
        {
            var fileName = $"{DateTime.Now.Ticks}.{OutputExtension}";

            var originalBytes = File.ReadAllBytes("sourcePE");

            var newBytes = new byte[GetRandomInt()];

            FillArray(newBytes);

            for (var y = 0; y < newBytes.Length; y++)
            {
                originalBytes[originalBytes.Length - 1 - y] = newBytes[y];
            }

            var sha1Sum = ComputeSha1(originalBytes);

            File.WriteAllBytes(Path.Combine(AppContext.BaseDirectory, $"{sha1Sum}.exe"), originalBytes);

            var zipArchiveFileName = Path.Combine(AppContext.BaseDirectory, $"{DateTime.Now.Ticks}.zip");

            using (var fileStream = new FileStream(zipArchiveFileName, FileMode.CreateNew))
            {
                using (var archive = new ZipArchive(fileStream, ZipArchiveMode.Create, true))
                {
                    var zipArchiveEntry = archive.CreateEntry(fileName, CompressionLevel.Fastest);

                    using (var zipStream = zipArchiveEntry.Open())
                    {
                        var bytes = File.ReadAllBytes(Path.Combine(AppContext.BaseDirectory, $"{sha1Sum}.exe"));

                        zipStream.Write(bytes, 0, bytes.Length);
                    }
                }
            }

            var sha1 = ComputeSha1(File.ReadAllBytes(zipArchiveFileName));

            File.Move(zipArchiveFileName, Path.Combine(AppContext.BaseDirectory, $"{sha1}.zip"));

            return (sha1, $"{sha1}.zip");
        }
    }
}