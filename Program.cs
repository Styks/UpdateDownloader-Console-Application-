using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Downloader
{
        class Program
        {
            static Timer checkTime = new Timer
            {
                Interval = 100,
                Enabled = true,
                AutoReset = true,
            };

            static void Main(string[] args)
            {
                checkTime.Elapsed += CheckProcess;
                Console.ReadKey();
            }

            static bool GetUrlFile(string address, string FileNme)
            {
                WebClient client = new WebClient();
                client.Credentials = CredentialCache.DefaultNetworkCredentials;
                try
                {
                    client.DownloadFile(address, FileNme);
                    return true;
                }
                catch
                {
                    return false;
                }
            }

            public static void CheckProcess(object source, ElapsedEventArgs e)
            {
                Console.WriteLine("Preparing to download");
                checkTime.Enabled = false;

                List<String> urlN = new List<string>();
                string updpath = "";

                try
                {
                    GetUrlFile("url", "update.txt"); //update list path (http://site.com/update.txt) with ((first string - version, other strings - files names))
                    updpath = AppDomain.CurrentDomain.BaseDirectory + "update.txt";
                    Console.WriteLine("Update list downloaded");
                    Console.WriteLine("");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }

                try
                {
                    StreamReader updReader = new StreamReader(updpath);
                    while (!updReader.EndOfStream)
                    {
                        urlN.Add(updReader.ReadLine());
                    }
                    string updVers = urlN[0];
                    Console.WriteLine("Version from server: " + updVers);
                    urlN.RemoveAt(0);
                    updReader.Close();
                    File.Delete(updpath);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }

                try
                {
                    var versionInfo = FileVersionInfo.GetVersionInfo("ProgramName");
                    string version = versionInfo.FileVersion;
                    Console.WriteLine("Actual version: " + version);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }

                Console.WriteLine("");

                try
                {
                    string urlname, urlFile;
                    foreach (string i in urlN)
                    {
                        urlname = "url" + i; //files path
                        urlFile = "" + i;
                        if (GetUrlFile(urlname, urlFile))
                            Console.WriteLine("File " + urlFile + " complite");
                        else
                            Console.WriteLine("File " + urlFile + " not found");
                    }
                    Console.WriteLine("Downloading completed");
                    Console.WriteLine("");
                    for (int a = 5; a >= 0; a--)
                    {
                        Console.WriteLine("Auto close after " + a + " seconds");
                        System.Threading.Thread.Sleep(1000);
                    }
                    Environment.Exit(0);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }
        }
    }
