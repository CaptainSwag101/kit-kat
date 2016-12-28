using kitkat.Properties;
using ntrbase;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace kitkat
{
    
    public partial class kitkat : Form
    {
        
        public kitkat()
        {
            // Log handling
            delLog = new LogDelegate(log);
            
            Program.ntrClient.InfoReady += getGame;
            Program.ntrClient.Connected += onConnect;

            InitializeComponent();
            UpdateIP();
            
        }
        
        // General Theme/Application Handling
        #region Drag Function

        //Variables;
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;
        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        //Function;
        private void FormDrag(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                IntPtr myHandle = Handle;
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        #endregion
        #region Dropshadow

        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn
        (
            int nLeftRect,
            int nTopRect,
            int nRightRect,
            int nBottomRect,
            int nWidthEllipse,
            int nHeightEllipse
        );

        [DllImport("dwmapi.dll")]
        public static extern int DwmExtendFrameIntoClientArea(IntPtr hWnd, ref MARGINS pMarInset);

        [DllImport("dwmapi.dll")]
        public static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);

        [DllImport("dwmapi.dll")]
        public static extern int DwmIsCompositionEnabled(ref int pfEnabled);

        private bool m_aeroEnabled;
        private const int CS_DROPSHADOW = 0x00020000;
        private const int WM_NCPAINT = 0x0085;
        private const int WM_ACTIVATEAPP = 0x001C;

        public struct MARGINS
        {
            public int leftWidth;
            public int rightWidth;
            public int topHeight;
            public int bottomHeight;
        }

        protected override CreateParams CreateParams
        {
            get
            {
                m_aeroEnabled = CheckAeroEnabled();

                CreateParams cp = base.CreateParams;
                if (!m_aeroEnabled)
                    cp.ClassStyle |= CS_DROPSHADOW;

                return cp;
            }
        }

        private bool CheckAeroEnabled()
        {
            if (Environment.OSVersion.Version.Major >= 6)
            {
                int enabled = 0;
                DwmIsCompositionEnabled(ref enabled);
                return (enabled == 1) ? true : false;
            }
            return false;
        }

        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case WM_NCPAINT:
                    if (m_aeroEnabled)
                    {
                        var v = 2;
                        DwmSetWindowAttribute(Handle, 2, ref v, 4);
                        MARGINS margins = new MARGINS()
                        {
                            bottomHeight = 1,
                            leftWidth = 0,
                            rightWidth = 0,
                            topHeight = 0
                        };
                        DwmExtendFrameIntoClientArea(Handle, ref margins);

                    }
                    break;
                default:
                    break;
            }
            base.WndProc(ref m);

        }

        #endregion
        #region LoadFont
        [DllImport("gdi32.dll")]
        private static extern IntPtr AddFontMemResourceEx(IntPtr pbFont, uint cbFont, IntPtr pvd, [In] ref uint pcFonts);
        private static readonly PrivateFontCollection privateFontCollection = new PrivateFontCollection();
        public static FontFamily LoadFont(byte[] fontResource)
        {
            int dataLength = fontResource.Length;
            IntPtr fontPtr = Marshal.AllocCoTaskMem(dataLength);
            Marshal.Copy(fontResource, 0, fontPtr, dataLength);

            uint cFonts = 0;
            AddFontMemResourceEx(fontPtr, (uint)fontResource.Length, IntPtr.Zero, ref cFonts);
            privateFontCollection.AddMemoryFont(fontPtr, dataLength);

            return privateFontCollection.Families.Last();
        }
        #endregion
        #region Disable Arrow Navigation
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Up || keyData == Keys.Down)
            {
                // do what you need to do and the
                // return true will stop processing
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }
        #endregion

        // Variables
        public bool mempatch = false;
        Socket s;
        SimpleHTTPServer ss;
        string[] PushFiles;
        string ActiveDir;

        // OnLoad/OnClose & Handlers
        #region Onload
        private void Form1_Load(object sender, EventArgs e)
        {

            #region Load Settings
            CheckState ac;
            CheckState sc;
            if (Settings.Default.AutoConnect == true) { ac = CheckState.Checked; } else { ac = CheckState.Unchecked; }
            if (Settings.Default.ShowConsole == true) { sc = CheckState.Checked; } else { sc = CheckState.Unchecked; }

            ipaddress.Text = Settings.Default.IPAddress;
            AutoConnect.CheckState = ac;
            tScale.Text = Settings.Default.tScale.ToString();
            bScale.Text = Settings.Default.bScale.ToString();
            ViewMode.Value = Settings.Default.ViewMode;
            ShowConsole.CheckState = sc;
            PriorityFactor.Value = Settings.Default.PriorityFactor;
            QOSValue.Value = Settings.Default.QOSValue;
            ScreenPriority.Value = Settings.Default.ScreenPriority;
            Quality.Value = Settings.Default.Quality;

            NetSSID.Text = Settings.Default.NetSSID;
            NetPass.Text = Settings.Default.NetPass;
            #endregion

            // Re-Start the HostedNetwork
            RestartNetwork();

            // If Auto-Connect is enabled
            if (Settings.Default.AutoConnect == true) { mempatch = false; connect(Settings.Default.IPAddress, 8000); Program.scriptHelper.listprocess(); }
            
            //new InputRedirection.Game1();

        }
        #endregion
        #region Onclose
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {

            #region Shut down NTRViewer
            foreach (Process p in Process.GetProcessesByName("NTRViewer"))
            {
                p.Kill();
                p.WaitForExit();
            }
            #endregion
            #region Remove Extracted NTRViewer.exe
            File.Delete(Path.Combine(Path.GetTempPath(), "NTRViewer.exe"));
            File.Delete(Path.Combine(Path.GetTempPath(), "SDL2.dll"));
            File.Delete(Path.Combine(Path.GetTempPath(), "turbojpeg.dll"));
            #endregion
            #region Stop HostedNetwork
            Process cmd = new Process();
            cmd.StartInfo.FileName = "cmd.exe";
            cmd.StartInfo.RedirectStandardInput = true;
            cmd.StartInfo.RedirectStandardOutput = true;
            cmd.StartInfo.CreateNoWindow = true;
            cmd.StartInfo.UseShellExecute = false;
            cmd.Start();
            cmd.StandardInput.WriteLine("netsh wlan stop hostednetwork");
            cmd.StandardInput.Flush();
            cmd.StandardInput.Close();
            cmd.WaitForExit();
            #endregion

        }
        #endregion
        #region Update Settings

        private void tScale_TextChanged(object sender, EventArgs e)
        {
            Settings.Default["tScale"] = tScale.Text;
            Settings.Default.Save();
        }

        private void bScale_TextChanged(object sender, EventArgs e)
        {
            Settings.Default["bScale"] = bScale.Text;
            Settings.Default.Save();
        }

        private void ViewMode_ValueChanged(object sender, EventArgs e)
        {
            Settings.Default["ViewMode"] = (int)ViewMode.Value;
            Settings.Default.Save();
        }

        private void PriorityFactor_ValueChanged(object sender, EventArgs e)
        {
            Settings.Default["PriorityFactor"] = (int)PriorityFactor.Value;
            Settings.Default.Save();
        }

        private void QOSValue_ValueChanged(object sender, EventArgs e)
        {
            Settings.Default["QOSValue"] = (int)QOSValue.Value;
            Settings.Default.Save();
        }

        private void ScreenPriority_ValueChanged(object sender, EventArgs e)
        {
            Settings.Default["ScreenPriority"] = (int)ScreenPriority.Value;
            Settings.Default.Save();
        }

        private void Quality_ValueChanged(object sender, EventArgs e)
        {
            Settings.Default["Quality"] = (int)Quality.Value;
            Settings.Default.Save();
        }

        private void ipaddress_TextChanged(object sender, EventArgs e)
        {
            Settings.Default["IPAddress"] = ipaddress.Text;
            Settings.Default.Save();
            UpdateIP();
        }

        private void NetSSID_TextChanged(object sender, EventArgs e)
        {
            Settings.Default["NetSSID"] = NetSSID.Text;
            Settings.Default.Save();
        }

        private void NetPass_TextChanged(object sender, EventArgs e)
        {
            Settings.Default["NetPass"] = NetPass.Text;
            Settings.Default.Save();
        }

        private void AutoConnect_CheckedChanged(object sender, EventArgs e)
        {
            if (AutoConnect.CheckState == CheckState.Checked)
            {
                Settings.Default["AutoConnect"] = true;
            }
            else
            {
                Settings.Default["AutoConnect"] = false;
            }
            Settings.Default.Save();
        }
        private void ShowConsole_CheckedChanged(object sender, EventArgs e)
        {
            if (ShowConsole.CheckState == CheckState.Checked)
            {
                Settings.Default["ShowConsole"] = true;
            }
            else
            {
                Settings.Default["ShowConsole"] = false;
            }
            Settings.Default.Save();
        }
        #endregion
        #region Timeout Handler
        private void DisconnectTimeout_Tick(object sender, EventArgs e)
        {
            DisconnectTimeout.Enabled = false;
            Program.ntrClient.disconnect();
            ConnectButton.Text = "CONNECT";
            DisconnectTimeout.Stop();
        }
        #endregion
        #region Log Handler
        public delegate void LogDelegate(string l, string c = "logger");
        public LogDelegate delLog;
        public string lastlog;
        public void log(string l, string c = "logger")
        {
            //lastlog = l;
            if (!l.Contains("\r\n"))
                l = l.Replace("\n", "\r\n");
            if (!l.EndsWith("\n"))
                l += "\r\n";
            Invoke(new MethodInvoker(() =>
            {
                if (c == "logger")
                {
                    logger.Text += l;
                }
                else if (c == "logger3")
                {
                    logger3.Text += l;
                }
            }));
            return;
        }
        #endregion
        #region Heartbeat Handler
        private void Heartbeat_Tick(object sender, EventArgs e)
        {
            try
            {
                Program.ntrClient.sendHeartbeatPacket();
            }
            catch (Exception)
            {

            }
        }
        #endregion
        #region InputRedirector Handling
        public static bool IsUsingMouse = false;
        public static bool isMouseDown = false;
        public static int MX;
        public static int MY;
        bool irstatus = false;

        private void pctSurface_MouseMove(object sender, MouseEventArgs e)
        {
            MX = e.X;
            MY = e.Y;

            if (ClientRectangle.Contains(PointToClient(MousePosition)))
            {
                IsUsingMouse = true;
                irlog.Text = "X: " + MX + " | Y: " + MY;
            }
            else
            {
                IsUsingMouse = false;
                irlog.Text = "send nudes";
            }
        }

        private void pctSurface_MouseLeave(object sender, EventArgs e) { irlog.Text = "idle"; }
        private void pctSurface_MouseDown(object sender, MouseEventArgs e) { isMouseDown = true; }
        private void pctSurface_MouseUp(object sender, MouseEventArgs e) { isMouseDown = false; }
        public IntPtr getDrawSurface()
        {
            return pctSurface.Handle;
        }
        #endregion

        // Events
        #region ConnectButton
        private void ConnectButton_Click(object sender, EventArgs e) {
            if (ConnectButton.Text == "CONNECT") { mempatch = false; connect(Settings.Default.IPAddress, 8000); Program.scriptHelper.listprocess(); } else { Program.scriptHelper.disconnect(); }
        }
        #endregion
        #region MemPatchButton
        private void MemPatchButton_Click(object sender, EventArgs e) {
            DialogResult dr = MessageBox.Show("You are about to send a temporary patch that disables In-Game Online Services. Are you sure?", "Are You Sure?", MessageBoxButtons.YesNo);
            if (dr == DialogResult.Yes)
            {
                mempatch = true;
                connect(Settings.Default.IPAddress, 8000, false);
            }
        }
        #endregion
        #region RestartHostedNetworkButton
        private void RestartHostedNetwork_Click(object sender, EventArgs e)
        {
            if (NetPass.Text.Length < 8)
            {
                MessageBox.Show("The Password '" + NetPass.Text + "' is less than 8 characters! Please correct it to continue.");
            }
            else
            {
                RestartNetwork();
            }
        }
        #endregion
        #region PushButton
        private void PushButton_Click(object sender, EventArgs e)
        {

            // Reset Logger
            logger3.Text = "";

            // Add Firewall Rule
            ProcessStartInfo procStartInfo = new ProcessStartInfo("netsh", "/c advfirewall firewall add rule name=\"CTRVFILESERVER\" dir=in action=allow protocol=TCP localport=8080");
            procStartInfo.UseShellExecute = false;
            procStartInfo.CreateNoWindow = true;
            Process.Start(procStartInfo);

            PushButton.Enabled = false;
            PushFileSelectButton.Enabled = false;

            log("Pushing files...", "logger3");

            ss = new SimpleHTTPServer(ActiveDir, 8080);

            System.Threading.Thread.Sleep(100);

            try
            {
                s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                s.Connect(Settings.Default.IPAddress, 5000);
            }
            catch (Exception)
            {
                log("Failed to Connect!\n- Make sure you have FBI 2.4.5 or higher and in 'Receive URLs over the network' menu,\n- Wi-Fi Adapter and Router might not be getting a strong enough connection,\n- IP Address could be incorrect (It changes every now and then),\n- 3DS and PC might not be connected to the same Network.", "logger3");
                PushButton.Enabled = true;
                PushFileSelectButton.Enabled = true;
            }

            string bstring = "";
            string localhost = "";

            // Get LocalHost;
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList) { if (ip.AddressFamily == AddressFamily.InterNetwork) { localhost = ip.ToString() + ":8080/"; } }

            // Add each file to the Byte String
            foreach (var CIA in PushFiles) { bstring += localhost + Path.GetFileName(CIA) + "\n"; }

            // Encoding
            byte[] Largo = BitConverter.GetBytes((uint)Encoding.ASCII.GetBytes(bstring).Length);
            byte[] Adress = Encoding.ASCII.GetBytes(bstring);
            Array.Reverse(Largo); //Endian fix
            byte[] outputBytes = new byte[Largo.Length + Adress.Length];
            Buffer.BlockCopy(Largo, 0, outputBytes, 0, Largo.Length);
            Buffer.BlockCopy(Adress, 0, outputBytes, Largo.Length, Adress.Length);

            // Send
            s.Send(outputBytes);
            s.BeginReceive(new byte[1], 0, 1, 0, new AsyncCallback(onPushed), null); //Call me back when the 3ds says something.

        }
        #endregion
        #region PushFileSelectButton
        private void PushFileSelectButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "CIA/Ticket (*.cia, *.tik)|*.cia;*.tik";
            ofd.FilterIndex = 0;
            ofd.Multiselect = true;

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                if (ofd.FileNames.Length > 0)
                {
                    PushFiles = ofd.FileNames;
                    ActiveDir = Path.GetDirectoryName(PushFiles[0]);
                    logger3.Text = "";
                    foreach (string file in PushFiles)
                    {
                        if (ActiveDir == Path.GetDirectoryName(file))
                        {
                            log("Added '" + Path.GetFileName(file) + "' to Queue", "logger3");
                        }
                        else
                        {
                            MessageBox.Show("Somehow you managed to pick 2 files that are in different folders." + Environment.NewLine + "Multi-File booping would need the entire computer hosted to the network and that doesn't feel safe in my book." + Environment.NewLine + "Maybe in the future I'll find a way to do this.", "Woah there...", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }

                    }
                }

            }
        }
        #endregion
        #region BatchLinkButton
        private void BatchLinkButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog dg = new OpenFileDialog();
            dg.Title = "Open Text File";
            dg.Filter = "Batch files|*.bat";
            dg.Multiselect = false;
            dg.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (dg.ShowDialog() == DialogResult.OK)
            {
                MessageBox.Show(dg.FileName);
                Settings.Default["BatchFile"] = dg.FileName;
                Settings.Default.Save();
            }
        }
        #endregion
        #region Credits
        private void label8_Click(object sender, EventArgs e) { Process.Start("https://twitter.com/initPRAGMA"); }
        #endregion
        #region CustomTabControl SelectedIndexChanged
        private void customTabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // If it's the InputRedirecter Tab
            if (customTabControl1.SelectedIndex == 3 && irstatus == false)
            {
                // Start InputRedirecter
                InputRedirection.Game1 game = new InputRedirection.Game1(getDrawSurface());
                game.Run();
                // Store that it's running
                irstatus = true;
            }
        }
        #endregion

        // Functions
        #region NFCPatch
        public async void getGame(object sender, EventArgs e)
        {
            InfoReadyEventArgs args = (InfoReadyEventArgs)e;
            if (args.info.Contains("niji_loc")) // Sun/Moon
            {
                log("Writing Sun/Moon NFC Patch...", "logger");
                int pid = Convert.ToInt32("0x" + args.info.Substring(args.info.IndexOf(", pname: niji_loc") - 8, args.info.Length - args.info.IndexOf(", pname: niji_loc")).Substring(0, 8), 16);
                Task<bool> Patch = Program.scriptHelper.waitNTRwrite(0x3DFFD0, 0xE3A01000, pid);
                if (!(await Patch))
                    Console.WriteLine("[ERROR: An error has ocurred while applying the connection patch.]");
                log("[Written Sun/Moon NFC Patch!]", "logger");
                Program.scriptHelper.disconnect();
            }
            else
            {
                return;
            }
        }
        #endregion
        #region Connect
        public void connect(string host, int port, bool closeNTR = true)
        {
            if(ValidateIP(host) == true)
            {
                logger.Text = "";

                // Shut down NTRViewer
                if (closeNTR == true) { foreach (Process p in Process.GetProcessesByName("NTRViewer")) { p.Kill(); p.WaitForExit(); } }

                // Connect to Server
                Program.ntrClient.setServer(host, port);
                Program.ntrClient.connectToServer();
            } else {
                log("IP Address is invalid.", "logger");
            }
        }
        #endregion
        #region RestartNetwork
        private void RestartNetwork()
        {
            hnlogger.Text = "";
            var proc = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = "/c netsh wlan stop hostednetwork && netsh wlan set hostednetwork mode=allow ssid=" + NetSSID.Text + " key=" + NetPass.Text + " && netsh wlan start hostednetwork && netsh wlan show hostednetwork",
                    UseShellExecute = false,
                    RedirectStandardInput = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                }
            };
            proc.Start();
            hnlogger.Text = proc.StandardOutput.ReadToEnd();
        }
        #endregion
        #region onConnect
        public void onConnect(object sender, EventArgs e)
        {
            
            ConnectButton.Text = "DISCONNECT";

            // If Memory Patching
            if (mempatch)
            {

                byte[] bytes = { 0x70, 0x47 };
                Program.ntrClient.sendWriteMemPacket(0x0105AE4, 0x1a, bytes);

            } else {

                // Activate Remote Play
                Program.ntrClient.sendEmptyPacket(901, (uint)Settings.Default.ScreenPriority << 8 | (uint)Settings.Default.PriorityFactor, (uint)Settings.Default.Quality, (uint)(Settings.Default.QOSValue * 1024 * 1024 / 8));
                log("RemotePlay Activated", "logger");

                // Start Disconnect Timeout
                DisconnectTimeout.Enabled = true;
                DisconnectTimeout.Start();

                #region Open NTRViewer
                if (File.Exists(Path.Combine(Path.GetTempPath(), "NTRViewer.exe")))
                {
                    try
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.Append("-l " + Settings.Default.ViewMode.ToString() + " ");
                        sb.Append("-t " + Settings.Default.tScale + " ");
                        sb.Append("-b " + Settings.Default.bScale);

                        ProcessStartInfo p = new ProcessStartInfo(Path.Combine(Path.GetTempPath(), "NTRViewer.exe"));
                        p.Verb = "runas";
                        p.Arguments = sb.ToString().Replace(',', '.');

                        if(Settings.Default.ShowConsole == false)
                        {
                            p.UseShellExecute = false;
                            p.CreateNoWindow = true;
                        }

                        Process.Start(p);
                        log("NTRViewer Started", "logger");
                    }
                    catch (Exception err)
                    {
                        log("[ERROR: " + err.Message + "]", "logger");
                    }

                }
                else {
                    log("[ERROR: NTRViewer failed to extract, try downloading and running NTRViewer manually as an Administrator.]", "logger");
                }
                #endregion

                // Open Linked Batch File
                if(Settings.Default.BatchFile != "") { Process.Start(Settings.Default.BatchFile); }

            }

        }
        #endregion
        #region ValidateIP
        public bool ValidateIP(string ip)
        {
            if (string.IsNullOrWhiteSpace(ip)) { return false; }
            string[] splitValues = ip.Split('.');
            if (splitValues.Length != 4) { return false; }
            byte tempForParsing;
            return splitValues.All(r => byte.TryParse(r, out tempForParsing));
        }
        #endregion
        #region UpdateIP
        public void UpdateIP()
        {
            if (Settings.Default.IPAddress != "3DS IP Address")
            {
                if(ValidateIP(Settings.Default.IPAddress) != false)
                {
                    ConnectButton.Enabled = true;
                    PushButton.Enabled = true;
                    PushFileSelectButton.Enabled = true;
                    logger.Text = "Ready to connect to '" + Settings.Default.IPAddress + "'";
                    logger3.Text = "Ready to connect to '" + Settings.Default.IPAddress + "'";
                } else
                {
                    ConnectButton.Enabled = false;
                    PushButton.Enabled = false;
                    PushFileSelectButton.Enabled = false;
                    logger.Text = "The IP '" + Settings.Default.IPAddress + "' is not valid.";
                    logger3.Text = "The IP '" + Settings.Default.IPAddress + "' is not valid.";
                }
            }
            else
            {
                ConnectButton.Enabled = false;
                PushButton.Enabled = false;
                PushFileSelectButton.Enabled = false;
                logger.Text = "3DS IP is not configured.";
                logger3.Text = "3DS IP is not configured.";
            }
        }
        #endregion
        #region onPushed
        private void onPushed(IAsyncResult ar)
        {
            
            Invoke((MethodInvoker)delegate
            {
                PushButton.Enabled = true;
                PushFileSelectButton.Enabled = true;
                log("Pushed files", "logger3");
            });
            
            // Remove Firewall Rule
            ProcessStartInfo procStartInfo = new ProcessStartInfo("netsh", "advfirewall firewall delete rule name=\"BOOPFILESERVER\"");
            procStartInfo.UseShellExecute = false;
            procStartInfo.CreateNoWindow = true;
            Process.Start(procStartInfo);
            
            s.Close();
            ss.Stop();
        }
        #endregion
        
    }

}
