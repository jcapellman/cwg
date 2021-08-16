using System;
using System.IO;

using cwg.web.Data;

using METL;

namespace cwg.web.Generators
{
    public class PEMETLGenerator : BaseGenerator
    {
        public override string Name => "PE32 (MET&L)";

        public override bool Active => false;

        protected override string SourceName => "sourcePEMTL";

        protected override string CleanSourceName => "sourcePEMTL";

        protected override string OutputExtension => "exe";

        public override bool Packable => false;

        protected override (string sha1, string fileName) Generate(GenerationRequestModel model)
        {
            var sourceFile = Path.Combine(AppContext.BaseDirectory, "PE32.cs");

            var malFile = Path.Combine(AppContext.BaseDirectory, SourceName);

            var injectedBytes = METLInjector.InjectMalwareFromFile(sourceFile, malFile);

            var sha1Sum = ComputeSha1(injectedBytes);
            
            File.WriteAllBytes(Path.Combine(AppContext.BaseDirectory, $"{sha1Sum}.{OutputExtension}"), injectedBytes);
            
            return (sha1Sum, $"{sha1Sum}.{OutputExtension}");
        }
    }
}