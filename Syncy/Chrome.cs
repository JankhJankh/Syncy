using System;
using Ionic.Zip;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;

namespace Syncy
{
    class Chrome
    {

        public static void unzip_chrome()
        {
            Console.WriteLine("Writing Chrome preference to: " + get_chromesync_path());
            try
            {
                using (var zipFile = new ZipFile(@".\ChromeSync.zip"))
                {
                    zipFile.ExtractAll(get_chromesync_path(), ExtractExistingFileAction.OverwriteSilently);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Unzipping Chrome failed, make sure the process isn't running in the background.");
            }

        }

        public static void zip_chrome()
        {
            Console.WriteLine("Zipping Chrome preference to the current directory");
            //To compress files out of a zip archive:  
            try
            {
                using (ZipFile zip = new ZipFile())
                {
                    zip.AddDirectory(get_chromesync_path());
                    zip.Save(@".\ChromeSync.zip");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Zipping Chrome failed, make sure the process isn't running in the background.");
            }

        }

        public static void loop_chrome()
        {
            Program.wait(2000);
            start_chrome();
            Program.wait(Configs.Uptime);
            close_chrome();
            check_for_killnote_chrome();
        }
        public static void start_chrome()
        {
            try
            {
                string path = get_chromesync_path() + @"\Preferences";
                string readText = File.ReadAllText(path);
                readText = readText.Replace("\"exit_type\":\"Crashed\"", "\"exit_type\":\"Normal\"");
                File.WriteAllText(path, readText);
            }
            catch (Exception e)
            {
                Console.WriteLine("Failed to modify Chromesync file.");
                Console.WriteLine(e);
            }
            try
            {
                String chromeLocation = @"C:\Program Files\Google\Chrome\Application\chrome.exe";
                if (!File.Exists(chromeLocation))
                {
                    chromeLocation = @"C:\Program Files (x86)\Google\Chrome\Application\chrome.exe";
                }
                Program.spawn_process(chromeLocation, "--profile-directory=ChromeSync");
            }
            catch (Exception e)
            {
                Console.WriteLine("Starting Chrome failed, make sure it's installed.");
            }
        }
        public static void close_chrome()
        {
            string proc = "CHROME";
            Process[] processes = Process.GetProcesses();
            var pc = from p in processes
                     where p.ProcessName.ToUpper().Contains(proc)
                     select p;
            foreach (var item in pc)
            {
                item.CloseMainWindow();
            }
            Program.wait(3000);
            processes = Process.GetProcesses();
            pc = from p in processes
                 where p.ProcessName.ToUpper().Contains(proc)
                 select p;
            foreach (var item in pc)
            {
                item.Kill();
            }
        }
        public static string get_chromesync_path()
        {
            return @"C:\Users\" + Program.get_username() + @"\AppData\Local\Google\Chrome\User Data\ChromeSync";
        }

        static void check_for_killnote_chrome()
        {
            //Check for killnote
            string path = get_chromesync_path() + @"\Secure Preferences";
            string readText = File.ReadAllText(path);
            string pattern = "SUPERSECRETKILLCODE";
            if (readText.ToUpper().Contains(pattern))
            {
                Program.kill();
            }
        }
    }
}
