﻿using System;
using System.IO;
using System.Text;

using cwg.web.Data;

namespace cwg.web.Generators
{
    public class HTAPowerShellGenerator : BaseGenerator
    {
        public override string Name => "HTA (PowerShell)";

        protected override string SourceName => "sourceHTAPS";

        protected override string CleanSourceName => "sourceHTAPS";

        protected override string OutputExtension => "hta";

        public override bool Active => false;

        public override bool Packable => false;

        protected override (string sha1, string fileName) Generate(GenerationRequestModel model)
        {
            var sourceFile = File.ReadAllText(SourceName);

            var exeString = File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "sourcePS.ps1"));

            sourceFile = sourceFile.Replace("$TIMESTAMP", $"Own3d on {DateTime.Now}");

            sourceFile = sourceFile.Replace("$BASE64PAYLOAD", exeString);
            
            var bytes = Encoding.ASCII.GetBytes(sourceFile);

            var sha1Sum = ComputeSha1(bytes);

            File.WriteAllBytes(Path.Combine(AppContext.BaseDirectory, $"{sha1Sum}.{OutputExtension}"), bytes);

            return (sha1Sum, $"{sha1Sum}.{OutputExtension}");
        }
    }
}