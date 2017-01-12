using kit_kat.Properties;
using ntrbase;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace kit_kat
{
    static class Program
    {
        public static NTR viewer;
        public static NTR ir;
        public static MainForm mainform;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {

            viewer = new NTR();
            ir = new NTR();

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
                File.WriteAllBytes(Path.Combine(Path.GetTempPath(), "NTRViewer.exe"), Resources.NTRViewer);
                File.WriteAllBytes(Path.Combine(Path.GetTempPath(), "SDL2.dll"), Resources.SDL2);
                File.WriteAllBytes(Path.Combine(Path.GetTempPath(), "turbojpeg.dll"), Resources.turbojpeg);
                File.WriteAllBytes(Path.Combine(Path.GetTempPath(), "3dstool.exe"), Resources._3dstool);
                File.WriteAllBytes(Path.Combine(Path.GetTempPath(), "ctrtool.exe"), Resources.ctrtool);
                File.WriteAllBytes(Path.Combine(Path.GetTempPath(), "MakeRom.exe"), Resources.MakeRom);
                //Start
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                mainform = new MainForm();
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
