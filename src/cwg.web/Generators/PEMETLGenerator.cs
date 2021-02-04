using System;
using System.IO;

using cwg.web.Data;

using METL;

namespace cwg.web.Generators
{
    public class PEMETLGenerator : BaseGenerator
    {
        public override string Name => "PE32 (MET&L)";

        protected override string SourceName => "sourcePE.exe";

        protected override string OutputExtension => ".exe";

        public override bool Packable => false;

        protected override (string sha1, string fileName) Generate(GenerationRequestModel model)
        {
            var sourceFile = Path.Combine(AppContext.BaseDirectory, "PE32.cs");

            var malFile = Path.Combine(AppContext.BaseDirectory, SourceName);

            var injectedBytes = METLInjector.InjectMalwareFromFile(sourceFile, malFile);

            var sha = ComputeSha1(injectedBytes);

            var fileName = Path.Combine(AppContext.BaseDirectory, sha + OutputExtension);

            File.WriteAllBytes(fileName, injectedBytes);

            return (sha, fileName);
        }
    }
}