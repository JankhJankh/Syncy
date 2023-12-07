using System;
using Ionic.Zip;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;

namespace Syncy
{
    class Firefox
    {
        public static void unzip_firefox()
        {
            //To decompress files out of a zip archive:  
            Console.WriteLine("Writing Firefox preference to: " + get_firefoxsync_path());
            try
            {
                using (var zipFile = new ZipFile(@".\FirefoxSync.zip"))
                {
                    zipFile.ExtractAll(get_firefoxsync_path(), ExtractExistingFileAction.OverwriteSilently);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Unzipping Firefox failed, make sure the process isn't running in the background.");
            }
        }

        public static void zip_firefox()
        {
            Console.WriteLine("Zipping Firefox preference to the current directory");
            //To compress files out of a zip archive:  
            try
            {
                using (ZipFile zip = new ZipFile())
                {
                    zip.AddDirectory(get_firefoxsync_path());
                    zip.Save(@".\FirefoxSync.zip");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Zipping Firefox failed, make sure the process isn't running in the background.");
            }
        }

        public static void loop_firefox()
        {
            Program.wait(2000);
            start_firefox();
            Program.wait(Configs.Uptime);
            close_firefox();
            check_for_killnote_firefox();
        }

        public static void start_firefox()
        {
            try
            {
                String firefoxLocation = @"C:\Program Files\Mozilla Firefox\Firefox.exe";
                if (!File.Exists(firefoxLocation))
                {
                    firefoxLocation = @"C:\Program Files (x86)\Mozilla Firefox\Firefox.exe";
                }
                Program.spawn_process(firefoxLocation, "--profile " + get_firefoxsync_path());
            }
            catch (Exception e)
            {
                Console.WriteLine("Starting Firefox failed, make sure the process is installed.");
            }
        }
        public static void close_firefox()
        {
            string proc = "FIREFOX";
            Process[] processes = Process.GetProcesses();
            var pc = from p in processes
                     where p.ProcessName.ToUpper().Contains(proc)
                     select p;
            foreach (var item in pc)
            {
                item.CloseMainWindow();
            }
        }

        static string get_firefoxsync_path()
        {
            return @"C:\Users\" + Program.get_username() + @"\AppData\Local\Mozilla\Firefox\Profiles\FirefoxSync";
        }

        static void check_for_killnote_firefox()
        {
            //Check for killnote
            string path = get_firefoxsync_path() + @"\prefs.js";
            string readText = File.ReadAllText(path);
            string pattern = "SUPERSECRETKILLCODE";
            if (readText.ToUpper().Contains(pattern))
            {
                Program.kill();
            }
        }
    }
}
