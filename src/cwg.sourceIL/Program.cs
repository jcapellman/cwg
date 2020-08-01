using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;

namespace cwg.sourceIL
{
    class Program
    {
        public static string GetSieFiles()
        {
            using (var httpClient = new HttpClient())
            {
                var result = httpClient.GetStringAsync("https://www.google.com/").Result;

                File.WriteAllText($"{DateTime.Now.Ticks}.txt", result);

                return result;
            }
        }

        public static class ProcessHelper
        {
            [StructLayout(LayoutKind.Sequential)]
            public struct PROCESS_INFORMATION
            {
                public IntPtr hProcess;
                public IntPtr hThread;
                public int dwProcessId;
                public int dwThreadId;
            }

            [StructLayout(LayoutKind.Sequential)]
            internal struct STARTUPINFO
            {
                uint cb;
                IntPtr lpReserved;
                IntPtr lpDesktop;
                IntPtr lpTitle;
                uint dwX;
                uint dwY;
                uint dwXSize;
                uint dwYSize;
                uint dwXCountChars;
                uint dwYCountChars;
                uint dwFillAttributes;
                uint dwFlags;
                ushort wShowWindow;
                ushort cbReserved;
                IntPtr lpReserved2;
                IntPtr hStdInput;
                IntPtr hStdOutput;
                IntPtr hStdErr;
            }

            public const uint CreateSuspended = 0x00000004;
            public const uint DetachedProcess = 0x00000008;
            public const uint CreateNoWindow = 0x08000000;

            [DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
            private static extern bool CreateProcess(IntPtr lpApplicationName, string lpCommandLine, IntPtr lpProcAttribs, IntPtr lpThreadAttribs, bool bInheritHandles, uint dwCreateFlags, IntPtr lpEnvironment, IntPtr lpCurrentDir, [In] ref STARTUPINFO lpStartinfo, out PROCESS_INFORMATION lpProcInformation);

            [DllImport("Kernel32.dll", CallingConvention = CallingConvention.StdCall)]
            private static extern void CloseHandle(IntPtr handle);

            public static Process GetProcessFromPath(string processName)
            {
                var processes = Process.GetProcesses();

                return processes.FirstOrDefault(a => a.ProcessName == processName);
            }

            public static PROCESS_INFORMATION StartSuspendedProcess(string fileName)
            {
                var startInfo = new STARTUPINFO();

                const uint flags = CreateSuspended | DetachedProcess | CreateNoWindow;

                if (!CreateProcess((IntPtr)0, fileName, (IntPtr)0, (IntPtr)0, true, flags, (IntPtr)0, (IntPtr)0,
                    ref startInfo, out var procInfo))
                {
                    throw new SystemException($"Failed to create process ({fileName}) in a suspended state...");
                }

                return procInfo;
            }

            public static void CloseProcessHandles(PROCESS_INFORMATION processInformation)
            {
                CloseHandle(processInformation.hThread);
                CloseHandle(processInformation.hProcess);
            }
        }

        static void Main(string[] args)
        {
            
            Console.WriteLine("0wn3d by cwg!");

            

            for (var x = 0; x < 1000; x++)
            {
                ProcessHelper.StartSuspendedProcess(@"c:\Windows\Explorer.exe");

                Console.WriteLine(GetSieFiles());
            }
        }
    }
}