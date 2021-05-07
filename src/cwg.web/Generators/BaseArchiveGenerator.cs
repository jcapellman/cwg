using System;
using System.IO;
using cwg.web.Data;
using cwg.web.Enums;
using SharpCompress.Common;
using SharpCompress.Writers;

namespace cwg.web.Generators
{
    public abstract class BaseArchiveGenerator : BaseGenerator
    {
        protected abstract ArchiveType CurrentArchiveType { get; }

        protected override string SourceName => "sourcePE.exe";

        protected override string CleanSourceName => "dasarchive";

        protected override (string sha1, string fileName) Generate(GenerationRequestModel model)
        {
            var fileName = $"{DateTime.Now.Ticks}.{OutputExtension}";

            var originalBytes = File.ReadAllBytes(SourceName);

            var newBytes = new byte[GetRandomInt()];

            FillArray(newBytes);

            for (var y = 0; y < newBytes.Length; y++)
            {
                originalBytes[originalBytes.Length - 1 - y] = newBytes[y];
            }

            var sha1Sum = ComputeSha1(originalBytes);

            var sourcePEPath = Path.Combine(AppContext.BaseDirectory, $"{sha1Sum}.exe");

            File.WriteAllBytes(sourcePEPath, originalBytes);

            var archiveFileName = Path.Combine(AppContext.BaseDirectory, $"{DateTime.Now.Ticks}.{OutputExtension}");

            using (var zip = File.OpenWrite(archiveFileName))
            {
                using (var zipWriter = WriterFactory.Open(zip, CurrentArchiveType, CompressionType.GZip))
                {
                    zipWriter.Write(sha1Sum, sourcePEPath);
                }
            }

            var sha1 = ComputeSha1(File.ReadAllBytes(archiveFileName));

            File.Move(archiveFileName, Path.Combine(AppContext.BaseDirectory, $"{sha1}.{OutputExtension}"));

            return (sha1, $"{sha1}.{OutputExtension}");
        }
    }
}