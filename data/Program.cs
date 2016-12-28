using ntrbase;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace kitkat
{
    
    static class Program
    {

        public static NTR ntrClient;
        public static ScriptHelper scriptHelper;
        public static kitkat mainform;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {

            ntrClient = new NTR();
            scriptHelper = new ScriptHelper();

            #region Check for an Update
            //If the user is Connected to the Internet;
            if (TestInternetConnection())
            {
                
                //Using WebClient, get the Newest File Version;
                using (System.Net.WebClient wc = new System.Net.WebClient())
                {

                    //Placeholder Strings;
                    string LatestVersion = "";
                    string ExecutableLocation = "";
                    string CurrentVersion = "";
                    string CurrentExecutableName = "";

                    // Try fill in the Placeholder Strings;
                    try
                    {
                        LatestVersion = wc.DownloadString("https://raw.githubusercontent.com/initPRAGMA/kit-kat/master/version.txt");
                        ExecutableLocation = typeof(Program).Assembly.CodeBase.Replace("file:///", "");
                        CurrentVersion = FileVersionInfo.GetVersionInfo(ExecutableLocation).ProductVersion;
                        CurrentExecutableName = typeof(Program).Assembly.GetName().Name + "-" + LatestVersion + ".exe";
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Please re-download kit-kat manually as an Update Bug Occured! Error: " + ex.Message, "Update Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    
                    // If the Current version is not the same as the Latest Version on Github
                    if (LatestVersion != CurrentVersion && LatestVersion != "" && CurrentVersion != "")
                    {
                        try
                        {
                            // Download the latest version
                            wc.DownloadFile("https://github.com/initPRAGMA/kit-kat/raw/master/kit-kat.exe", CurrentExecutableName);
                            // Show a MessageBox asking to open Explorer to the file;
                            DialogResult mb = MessageBox.Show("Continue usage on the new update. Open Explorer and go to the Directory containing the updated .exe located at: " + ExecutableLocation.Replace("CTR-V.EXE", CurrentExecutableName + " ?\""), "New Update Downloaded!", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                            if (mb == DialogResult.Yes)
                            {
                                // Go to where CTR-V is and select the New Update.exe;
                                Process.Start("explorer.exe", "/select,\"" + ExecutableLocation.Replace("/", "\\").Replace("CTR-V.EXE", CurrentExecutableName) + "\"");
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Please re-download kit-kat manually as an Update Bug Occured! Error: " + ex.Message, "Update Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        // Start CTR-V
                        Start();
                    }

                }
                
            }
            else
            {
                // Start CTR-V
                Start();
            }

            #endregion

        }

        private static bool TestInternetConnection()
        {
            try
            {
                System.Net.NetworkInformation.Ping ping = new System.Net.NetworkInformation.Ping();
                ping.Send("google.com");
                return true;
            }
            catch
            {
                return false;
            }
        }
        private static bool Start()
        {
            try
            {
                // Shut down NTRViewer
                foreach (Process p in Process.GetProcessesByName("NTRViewer")) { p.Kill(); p.WaitForExit(); }
                // Extract to Temp Directory
                File.WriteAllBytes(Path.Combine(Path.GetTempPath(), "NTRViewer.exe"), Properties.Resources.NTRViewer);
                File.WriteAllBytes(Path.Combine(Path.GetTempPath(), "SDL2.dll"), Properties.Resources.SDL2);
                File.WriteAllBytes(Path.Combine(Path.GetTempPath(), "turbojpeg.dll"), Properties.Resources.turbojpeg);
                //Start
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                mainform = new kitkat();
                Application.Run(mainform);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

    }
}
