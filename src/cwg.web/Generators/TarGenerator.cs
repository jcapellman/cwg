using System;
using System.IO;
using System.IO.Compression;
using SharpCompress.Common;
using SharpCompress.Writers.Tar;

namespace cwg.web.Generators
{
    public class TarGenerator : BaseGenerator
    {
        public override string Name => "TAR";

        protected override string SourceName => "sourcePE.exe";

        protected override string OutputExtension => "tar";

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

            var sourcePEPath = Path.Combine(AppContext.BaseDirectory, $"{sha1Sum}.exe");

            File.WriteAllBytes(sourcePEPath, originalBytes);

            var zipArchiveFileName = Path.Combine(AppContext.BaseDirectory, $"{DateTime.Now.Ticks}.tar");

            using (var stream = new MemoryStream())
            {
                using (Stream content = File.OpenRead(sourcePEPath))
                {
                    using (var writer = new TarWriter(stream, new TarWriterOptions(CompressionType.BZip2, true)))
                    {
                        writer.Write(zipArchiveFileName, content, null);
                    }
                }
            }

            var sha1 = ComputeSha1(File.ReadAllBytes(zipArchiveFileName));

            File.Move(zipArchiveFileName, Path.Combine(AppContext.BaseDirectory, $"{sha1}.tar"));

            return (sha1, $"{sha1}.tar");
        }
    }
}