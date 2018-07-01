using System;
using System.IO;
using System.Linq;

namespace cwg.web.Generators
{
    public class MACHOGenerator : BaseGenerator
    {
        public override string Name => "Mach-o";

        protected override string SourceName => "sourceMACHO";

        protected override string OutputExtension => ".macho";

        protected override (string sha1, string fileName) Generate()
        {
            var originalBytes = File.ReadAllBytes(SourceName);
            var machioBytes = File.ReadAllBytes("sourceMACHIO");

            var newBytes = new byte[GetRandomInt()];

            FillArray(newBytes);

            for (var y = 0; y < newBytes.Length; y++)
            {
                originalBytes[originalBytes.Length - 1 - y] = newBytes[y];
            }

            var finalBytes = originalBytes.Concat(machioBytes).ToArray();

            var sha1Sum = ComputeSha1(finalBytes);

            File.WriteAllBytes(Path.Combine(AppContext.BaseDirectory, $"{sha1Sum}"), finalBytes);

            return (sha1Sum, $"{sha1Sum}");
        }
    }
}