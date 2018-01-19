using System;
using System.IO;
using System.Linq;

namespace cwg.web.Generators
{
    public class MACHOGenerator : BaseGenerator
    {
        public override string GetName() => "Mach-o";

        public override string GetSourceName() => "sourceMACHO";

        protected override (string sha1, string fileName) Generate()
        {
            var originalBytes = System.IO.File.ReadAllBytes(GetSourceName());
            var machioBytes = System.IO.File.ReadAllBytes("sourceMACHIO");

            var newBytes = new byte[getRandomInt()];

            FillArray(newBytes);

            for (var y = 0; y < newBytes.Length; y++)
            {
                originalBytes[originalBytes.Length - 1 - y] = newBytes[y];
            }

            var finalBytes = originalBytes.Concat(machioBytes).ToArray();

            var sha1Sum = ComputeSha1(finalBytes);

            System.IO.File.WriteAllBytes(Path.Combine(AppContext.BaseDirectory, $"{sha1Sum}"), finalBytes);

            return (sha1Sum, $"{sha1Sum}");
        }
    }
}