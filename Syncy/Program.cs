using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;


namespace Syncy
{
    class Program
    {
        static void Main(string[] args)
        {
            
            var parsed = ArgumentParser.Parse(args);
            if (parsed.ParsedOk == false)
            {
                print_help();
                return;
            }

            var commandName = args.Length != 0 ? args[0] : "";

            String browser = "";
            String action = "";
            if (parsed.Arguments.ContainsKey("-h"))
            {
                print_help();
                return;
            }
            if (parsed.Arguments.ContainsKey("--help"))
            {
                print_help();
                return;
            }
            if (parsed.Arguments.ContainsKey("All"))
            {
                browser = "All";
            }
            else if (parsed.Arguments.ContainsKey("Chrome"))
            {
                browser = "Chrome";
            }
            else if (parsed.Arguments.ContainsKey("Firefox"))
            {
                browser = "Firefox";
            }
            else if (parsed.Arguments.ContainsKey("Edge"))
            {
                browser = "Edge";
            }
            else
            {
                print_help();
                return;
            }

            if (parsed.Arguments.ContainsKey("Loop"))
            {
                action = "Loop";
            }
            else if (parsed.Arguments.ContainsKey("RunOnce"))
            {
                action = "RunOnce";
            }
            else if (parsed.Arguments.ContainsKey("Unzip"))
            {
                action = "Unzip";
            }
            else if (parsed.Arguments.ContainsKey("Zip"))
            {
                action = "Zip";
            }
            else if (parsed.Arguments.ContainsKey("Start"))
            {
                action = "Start";
            }
            else if (parsed.Arguments.ContainsKey("Stop"))
            {
                action = "Stop";
            }

            if (action == "Zip")
            {
                if (browser == "All")
                {
                    close_firefox();
                    zip_firefox();
                    close_edge();
                    zip_edge();
                    close_chrome();
                    zip_chrome();
                }
                else if (browser == "Firefox")
                {
                    close_firefox();
                    zip_firefox();
                }
                else if (browser == "Edge")
                {
                    close_edge();
                    zip_edge();
                }
                else if (browser == "Chrome")
                {
                    close_chrome();
                    zip_chrome();
                }
            }
            else if (action == "Unzip")
            {
                if (browser == "All")
                {
                    close_firefox();
                    unzip_firefox();
                    close_edge();
                    unzip_edge();
                    close_chrome();
                    unzip_chrome();
                }
                else if (browser == "Firefox")
                {
                    close_firefox();
                    unzip_firefox();
                }
                else if (browser == "Edge")
                {
                    close_edge();
                    unzip_edge();
                }
                else if (browser == "Chrome")
                {
                    close_chrome();
                    unzip_chrome();
                }
            }
            else if (action == "RunOnce")
            {
                if (browser == "All")
                {
                    loop_firefox();
                    loop_edge();
                    loop_chrome();
                }
                else if (browser == "Firefox")
                {
                    loop_firefox();
                }
                else if (browser == "Edge")
                {
                    loop_edge();
                }
                else if (browser == "Chrome")
                {
                    loop_chrome();
                }
            }
            else if (action == "Loop")
            {
                while (true)
                {
                    wait(10000);
                    if (browser == "All")
                    {
                        loop_firefox();
                        loop_edge();
                        loop_chrome();
                    }
                    else if (browser == "Firefox")
                    {
                        loop_firefox();
                    }
                    else if (browser == "Edge")
                    {
                        loop_edge();
                    }
                    else if (browser == "Chrome")
                    {
                        loop_chrome();
                    }
                }
            }
            else if (action == "Start")
            {
                if (browser == "All")
                {
                    start_firefox();
                    start_edge();
                    start_chrome();
                }
                else if (browser == "Firefox")
                {
                    start_firefox();
                }
                else if (browser == "Edge")
                {
                    start_edge();
                }
                else if (browser == "Chrome")
                {
                    start_chrome();
                }
            }
            else if (action == "Stop")
            {
                if (browser == "All")
                {
                    close_firefox();
                    close_edge();
                    close_chrome();
                }
                else if (browser == "Firefox")
                {
                    close_firefox();
                }
                else if (browser == "Edge")
                {
                    close_edge();
                }
                else if (browser == "Chrome")
                {
                    close_chrome();
                }
            }
            else
            {
                print_help();
                return;
            }
        }

        static void print_help()
        {
            Console.WriteLine("Usage:");
            Console.WriteLine("./Synchy {Unzip|Zip|RunOnce|Loop|Start|Stop} {All|Edge|Chrome|Firefox}");
            Console.WriteLine("Unzip: Writes the cloudsync profiles to disk");
            Console.WriteLine("Zip: Zips the current cloudsync profiles");
            Console.WriteLine("RunOnce: Starts and closes each browser profile once");
            Console.WriteLine("Loop: Periodically restarts each browser");
            Console.WriteLine("Start: Starts each browser");
            Console.WriteLine("Stop: Kills each browser");
            return;
        }

        static void unzip_firefox()
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

        static void zip_firefox()
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

        static void loop_firefox()
        {
            wait(3000);
            start_firefox();
            wait(10000);
            close_firefox();
            check_for_killnote_firefox();
        }

        static void start_firefox() 
        {
            try
            {
                String firefoxLocation = @"C:\Program Files\Mozilla Firefox\Firefox.exe";
                if (!File.Exists(firefoxLocation))
                {
                    firefoxLocation = @"C:\Program Files (x86)\Mozilla Firefox\Firefox.exe";
                }
                Console.WriteLine("Spawning process: " + firefoxLocation + " --profile " + get_firefoxsync_path());
                using (Process process = Process.Start(firefoxLocation, "--profile " + get_firefoxsync_path()))
                {
                    process.WaitForInputIdle();
                    Console.WriteLine(process.Id);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Starting Firefox failed, make sure the process is installed.");
            }
}
        static void close_firefox()
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
            return @"C:\Users\" + get_username() + @"\AppData\Local\Mozilla\Firefox\Profiles\FirefoxSync";
        }

        static void unzip_edge()
        {
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

        static void zip_edge()
        {
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

        static void loop_edge()
        {
            wait(3000);
            start_edge();
            wait(10000);
            close_edge();
            check_for_killnote_edge();
        }
        static void start_edge()
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
                Process.Start("C:\\Program Files (x86)\\Microsoft\\Edge\\Application\\msedge.exe", "--profile-directory=EdgeSync");
            }
            catch (Exception e)
            {
                Console.WriteLine("Starting Edge failed, make sure the process is installed.");
            }

        }

        static void close_edge()
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

        static string get_edgesync_path()
        {
            return @"C:\Users\" + get_username() + @"\AppData\Local\Microsoft\Edge\User Data\EdgeSync";
        }

        static void unzip_chrome()
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

        static void zip_chrome()
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

        static void loop_chrome()
        {
            wait(3000);
            start_chrome();
            wait(10000);
            close_chrome();
            check_for_killnote_chrome();
        }
        static void start_chrome()
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
                Process.Start(chromeLocation, "--profile-directory=ChromeSync");
            }
            catch (Exception e)
            {
                Console.WriteLine("Starting Chrome failed, make sure it's installed.");
            }
        }
        static void close_chrome()
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
            wait(3000);
            processes = Process.GetProcesses();
            pc = from p in processes
                     where p.ProcessName.ToUpper().Contains(proc)
                     select p;
            foreach (var item in pc)
            {
                item.Kill();
            }
        }
        static string get_chromesync_path()
        {
            return @"C:\Users\" + get_username() + @"\AppData\Local\Google\Chrome\User Data\ChromeSync";
        }


        static void wait(int time)
        {
            System.Threading.Thread.Sleep(time);
        }

        static void run_and_log_output(String filename, String arguments)
        {
            Process process = new Process();
            process.StartInfo.FileName = filename;
            process.StartInfo.Arguments = arguments;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.Start();
            //* Read the output (or the error)
            string output = process.StandardOutput.ReadToEnd();
            Console.WriteLine(output);
            string err = process.StandardError.ReadToEnd();
            Console.WriteLine(err);
            process.WaitForExit();
        }

        static string get_username()
        {
            return Environment.UserName;
        }

        static void check_for_killnote_chrome()
        {
            //Check for killnote
            string path = get_chromesync_path() + @"\Secure Preferences";
            string readText = File.ReadAllText(path);
            string pattern = "SUPERSECRETKILLCODE";
            if (readText.ToUpper().Contains(pattern))
            {
                kill();
            }
        }


        static void check_for_killnote_firefox()
        {
            //Check for killnote
            string path = get_firefoxsync_path() + @"\prefs.js";
            string readText = File.ReadAllText(path);
            string pattern = "SUPERSECRETKILLCODE";
            if (readText.ToUpper().Contains(pattern))
            {
                kill();
            }
        }

        static void check_for_killnote_edge()
        {
            //Check for killnote
            string path = get_edgesync_path() + @"\Secure Preferences";
            string readText = File.ReadAllText(path);
            string pattern = "SUPERSECRETKILLCODE";
            if (readText.ToUpper().Contains(pattern))
            {
                kill();
            }

        }
        static void kill()
        {   
            Console.Write("Kill Received");
            System.Environment.Exit(1); 
        }
    }
}
