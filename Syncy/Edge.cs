using System;
using Ionic.Zip;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;

namespace Syncy
{
    class Edge
    {

        public static void unzip_edge()
        {
            hard_close_edge();
            Program.wait(2000);
            //To decompress files out of a zip archive:  
            Console.WriteLine("Writing Edge preference to: " + get_edgesync_path());
            try
            {
                using (var zipFile = new ZipFile(@".\EdgeSync.zip"))
                {
                    zipFile.ExtractAll(get_edgesync_path(), ExtractExistingFileAction.OverwriteSilently);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Unzipping Edge failed, make sure the process isn't running in the background.");
            }
        }

        public static void zip_edge()
        {
            hard_close_edge();
            Program.wait(2000);
            Console.WriteLine("Zipping Edge preference to the current directory");
            try
            {
                //To compress files out of a zip archive:  
                using (ZipFile zip = new ZipFile())
                {
                    zip.AddDirectory(get_edgesync_path());
                    zip.Save(@".\EdgeSync.zip");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Zipping Edge failed, make sure the process isn't running in the background.");
            }
        }

        public static void loop_edge()
        {
            Program.wait(2000);
            start_edge();
            Program.wait(Configs.Uptime);
            close_edge();
            check_for_killnote_edge();
        }
        public static void start_edge()
        {
            try
            {
                string path = get_edgesync_path() + @"\Preferences";
                string readText = File.ReadAllText(path);
                readText = readText.Replace("\"exit_type\":\"Crashed\"", "\"exit_type\":\"Normal\"");
                File.WriteAllText(path, readText);
            }
            catch (Exception e)
            {
                Console.WriteLine("Failed to modify Edgesync config");
            }
            try
            {
                Program.spawn_process("C:\\Program Files (x86)\\Microsoft\\Edge\\Application\\msedge.exe", "--profile-directory=EdgeSync");
            }
            catch (Exception e)
            {
                Console.WriteLine("Starting Edge failed, make sure the process is installed.");
            }

        }

        public static void close_edge()
        {
            string proc = "MSEDGE";
            Process[] processes = Process.GetProcesses();
            var pc = from p in processes
                     where p.ProcessName.ToUpper().Contains(proc)
                     select p;
            foreach (var item in pc)
            {
                item.CloseMainWindow();
            }
        }

        public static void hard_close_edge()
        {
            string proc = "MSEDGE";
            Process[] processes = Process.GetProcesses();
            var pc = from p in processes
                     where p.ProcessName.ToUpper().Contains(proc)
                     select p;
            foreach (var item in pc)
            {
                item.Kill();
            }

        }
        public static string get_edgesync_path()
        {
            return @"C:\Users\" + Program.get_username() + @"\AppData\Local\Microsoft\Edge\User Data\EdgeSync";
        }

        static void check_for_killnote_edge()
        {
            //Check for killnote
            string path = get_edgesync_path() + @"\Secure Preferences";
            string readText = File.ReadAllText(path);
            string pattern = "SUPERSECRETKILLCODE";
            if (readText.ToUpper().Contains(pattern))
            {
                Program.kill();
            }

        }
    }
}
