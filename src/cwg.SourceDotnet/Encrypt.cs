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

                //ICryptoTransform encryptor = aesObject.CreateEncryptor();
                byte[] test = aesObject.EncryptCbc(fileBytes, iv);
                File.WriteAllBytes(filePath,test);
                //using (MemoryStream mStream = new MemoryStream())
                //{
                //    using (CryptoStream cStream = new CryptoStream(mStream, encryptor, CryptoStreamMode.Write))
                //    {
                //        using (StreamWriter sw = new StreamWriter(cStream))
                //        {
                //            sw.Write(fileBytes);
                //        }
                //        File.WriteAllBytes(filePath, mStream.ToArray());
                //    }
                //}
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

                byte[] test = aesObject.DecryptCbc(fileBytes, iv);
                File.WriteAllBytes(filePath, test);

                //ICryptoTransform decryptor = aesObject.CreateDecryptor();

                //using (MemoryStream mStream = new MemoryStream())
                //{
                //    using (CryptoStream cStream = new CryptoStream(mStream, decryptor, CryptoStreamMode.Read))
                //    {
                //        using (StreamReader sr = new StreamReader(cStream))
                //        {
                //            plainText = sr.ReadToEnd();
                //        }
                //        File.WriteAllBytes(filePath, mStream.ToArray());
                //    }
                //}
            }
        }
    }
}
