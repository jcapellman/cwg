using System;
using System.IO;
using System.Linq;

using cwg.web.Data;

namespace cwg.web.Generators
{
    public class MACHOGenerator : BaseGenerator
    {
        public override string Name => "Mach-o";

        public override bool Active => true;

        protected override string SourceName => "sourceMACHO";

        protected override string CleanSourceName => "sourceCleanMacho";

        protected override string OutputExtension => ".macho";

        public override bool Packable => true;

        protected override (string sha1, string fileName) Generate(GenerationRequestModel model)
        {
            var originalBytes = File.ReadAllBytes(Path.Combine(AppContext.BaseDirectory, SourceName));
            var machioBytes = File.ReadAllBytes(Path.Combine(AppContext.BaseDirectory, SourceName));

            var newBytes = new byte[GetRandomInt()];
            
            FillArray(newBytes);

            for (var y = 0; y < newBytes.Length; y++)
            {
                originalBytes[originalBytes.Length - 1 - y] = newBytes[y];
            }

            var finalBytes = originalBytes.Concat(machioBytes).ToArray();

            var sha1Sum = ComputeSha1(finalBytes);

            var filePath = Path.Combine(AppContext.BaseDirectory, $"{sha1Sum}");

            File.WriteAllBytes(filePath, finalBytes);
            
            return (sha1Sum, $"{sha1Sum}");
        }
    }
}