using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

using cwg.web.Data;

namespace cwg.web.Generators
{
    public class PowerShellController : BaseGenerator
    {
        public override string Name => "Powershell";

        public override bool Active => true;

        protected override string SourceName => "sourcePS.ps1";
        
        protected override string CleanSourceName => "sourceCleanPS.ps1";

        protected override string OutputExtension => "ps1";

        public override bool Packable => false;

        protected override (string sha1, string fileName) Generate(GenerationRequestModel model)
        {
            var sourceFile = File.ReadAllText(SourceName);

            var exe = File.ReadAllBytes(Path.Combine(AppContext.BaseDirectory, "sourcePE"));

            var exeString = System.Convert.ToBase64String(exe);

            sourceFile = sourceFile.Replace("$timestamp;", $"$timestamp = \"{DateTime.Now.Ticks}\";");
            sourceFile = sourceFile.Replace("$placeholder", $"$placeholder = \"{exeString}\";");

            var bytes = Encoding.ASCII.GetBytes(sourceFile);

            var sha1Sum = ComputeSha1(bytes);

            File.WriteAllBytes(Path.Combine(AppContext.BaseDirectory, $"{sha1Sum}.{OutputExtension}"), bytes);

            return (sha1Sum, $"{sha1Sum}.{OutputExtension}");
        }
    }
}