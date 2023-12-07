using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace Syncy
{
    public static class Configs
    {
        public static int Uptime = 5000;
        public static int Downtime = 25000;
    }
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
                    Firefox.close_firefox();
                    Firefox.zip_firefox();
                    Edge.close_edge();
                    Edge.zip_edge();
                    Chrome.close_chrome();
                    Chrome.zip_chrome();
                }
                else if (browser == "Firefox")
                {
                    Firefox.close_firefox();
                    Firefox.zip_firefox();
                }
                else if (browser == "Edge")
                {
                    Edge.close_edge();
                    Edge.zip_edge();
                }
                else if (browser == "Chrome")
                {
                    Chrome.close_chrome();
                    Chrome.zip_chrome();
                }
            }
            else if (action == "Unzip")
            {
                if (browser == "All")
                {
                    Firefox.close_firefox();
                    Firefox.unzip_firefox();
                    Edge.close_edge();
                    Edge.unzip_edge();
                    Chrome.close_chrome();
                    Chrome.unzip_chrome();
                }
                else if (browser == "Firefox")
                {
                    Firefox.close_firefox();
                    Firefox.unzip_firefox();
                }
                else if (browser == "Edge")
                {
                    Edge.close_edge();
                    Edge.unzip_edge();
                }
                else if (browser == "Chrome")
                {
                    Chrome.close_chrome();
                    Chrome.unzip_chrome();
                }
            }
            else if (action == "RunOnce")
            {
                if (browser == "All")
                {
                    Firefox.loop_firefox();
                    Edge.loop_edge();
                    Chrome.loop_chrome();
                }
                else if (browser == "Firefox")
                {
                    Firefox.loop_firefox();
                }
                else if (browser == "Edge")
                {
                    Edge.loop_edge();
                }
                else if (browser == "Chrome")
                {
                    Chrome.loop_chrome();
                }
            }
            else if (action == "Loop")
            {
                while (true)
                {
                    wait(Configs.Downtime);
                    if (browser == "All")
                    {
                        Firefox.loop_firefox();
                        Edge.loop_edge();
                        Chrome.loop_chrome();
                    }
                    else if (browser == "Firefox")
                    {
                        Firefox.loop_firefox();
                    }
                    else if (browser == "Edge")
                    {
                        Edge.loop_edge();
                    }
                    else if (browser == "Chrome")
                    {
                        Chrome.loop_chrome();
                    }
                }
            }
            else if (action == "Start")
            {
                if (browser == "All")
                {
                    Firefox.start_firefox();
                    Edge.start_edge();
                    Chrome.start_chrome();
                }
                else if (browser == "Firefox")
                {
                    Firefox.start_firefox();
                }
                else if (browser == "Edge")
                {
                    Edge.start_edge();
                }
                else if (browser == "Chrome")
                {
                    Chrome.start_chrome();
                }
            }
            else if (action == "Stop")
            {
                if (browser == "All")
                {
                    Firefox.close_firefox();
                    Edge.close_edge();
                    Chrome.close_chrome();
                }
                else if (browser == "Firefox")
                {
                    Firefox.close_firefox();
                }
                else if (browser == "Edge")
                {
                    Edge.close_edge();
                }
                else if (browser == "Chrome")
                {
                    Chrome.close_chrome();
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

        public static void wait(int time)
        {
            System.Threading.Thread.Sleep(time);
        }

        public static void run_and_log_output(String filename, String arguments)
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

        public static string get_username()
        {
            return Environment.UserName;
        }


        public static void kill()
        {   
            Console.Write("Kill Received");
            System.Environment.Exit(1); 
        }

        public static void spawn_process(String processloc, String processparams)
        {
            //This is used to log the process locations, and let you know when a browser is spawned very handy for debugging.
            Console.WriteLine("Spawning process: " + processloc + " " + processparams);
            Process.Start(processloc, processparams);
        }
    }
}

