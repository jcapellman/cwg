using System.ComponentModel;
using System.Security.Cryptography;
using System.Text;

namespace cwg.sourceDotnet
{
    class Crypto
    {
        public static void EncryptFile(string filePath, string password)
        {

            byte[] passwordBytes = SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(password));
            byte[] iv = MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(password));
            byte[] fileBytes = File.ReadAllBytes( filePath );

            if (string.IsNullOrEmpty(password) || fileBytes.Length == 0)                
                return;

            using (Aes aesObject = Aes.Create())
            {
                aesObject.Key = passwordBytes;
                aesObject.IV = iv;

                byte[] encrypted = aesObject.EncryptCbc(fileBytes, iv);
                File.WriteAllBytes(filePath,encrypted);
            }

        }

        public static void DecryptFile(string filePath, string password)
        {

            byte[] passwordBytes = SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(password));
            byte[] iv = MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(password));
            byte[] fileBytes = File.ReadAllBytes(filePath);

            if (string.IsNullOrEmpty(password) || fileBytes.Length == 0)
                return;

            using (Aes aesObject = Aes.Create())
            {
                aesObject.Key = passwordBytes;
                aesObject.IV = iv;

                byte[] decrypted = aesObject.DecryptCbc(fileBytes, iv);
                File.WriteAllBytes(filePath, decrypted);
            }
        }
    }
}
