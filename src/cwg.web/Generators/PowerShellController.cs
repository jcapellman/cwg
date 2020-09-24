using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using cwg.web.Enums;

namespace cwg.web.Generators
{
    public class PowerShellController : BaseGenerator
    {
        public override string Name => "Powershell";

        protected override string SourceName => "sourcePS.ps1";

        protected override string OutputExtension => "ps1";

        private string encryptString(string sourceString, string key, string iv)
        {
            byte[] encrypted;

            using (Aes aesAlg = Aes.Create())
            {
                var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (var msEncrypt = new MemoryStream())
                {
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (var swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(sourceString);
                        }

                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            return System.Convert.ToBase64String(encrypted, Base64FormattingOptions.None);
        }

        protected override (string sha1, string fileName) Generate(ThreatLevels threatLevel, string injection)
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