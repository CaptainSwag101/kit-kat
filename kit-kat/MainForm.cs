using kit_kat.Properties;
using kit_kat.httpserver;
using ntrbase;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace kit_kat
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            // Log handling
            delLog = new LogDelegate(log);

            Program.viewer.InfoReady += getGame;
            Program.viewer.Connected += onConnect;

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
        sRequest ss;
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
            #endregion

            // If Auto-Connect is enabled
            if (Settings.Default.AutoConnect == true) { mempatch = false; connect(Settings.Default.IPAddress, 8000); Program.viewer.sendEmptyPacket(5); }

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
            s.Close();
            ss.Stop();
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
            Program.ir.disconnect();
            Program.viewer.disconnect();
            ConnectButton.Text = "CONNECT";
            DisconnectTimeout.Stop();
        }
        #endregion
        #region Log Handler
        public delegate void LogDelegate(string l, string c = "logger", string s = "");
        public LogDelegate delLog;
        public string lastlog;
        public void log(string l, string c = "logger", string s = "")
        {
            //lastlog = l;
            if (!l.Contains("\r\n"))
                l = l.Replace("\n", "\r\n");
            if (!l.EndsWith("\n"))
                l += "\r\n";
            Invoke(new MethodInvoker(() =>
            {
                if(l != "" && !l.Contains("false"))
                {
                    if (c == "logger")
                    {
                        logger.Text = l;
                    }
                    else if (c == "logger2")
                    {
                        logger2.Text = l;
                    }
                }

                if (s != "")
                {
                    if (c == "logger")
                    {
                        if (s.Contains("Failed")) { status1panel.BackColor = Settings.Default.AlertColor; }
                        else if (s.Contains("Success")) { status1panel.BackColor = Color.LightGreen; }
                        else { status1panel.BackColor = Color.FromArgb(90, 184, 255); }
                        status1.Text = s;
                    }
                    else if (c == "logger2")
                    {
                        if (s.Contains("Failed")) { status2panel.BackColor = Settings.Default.AlertColor; }
                        else if (s.Contains("Success")) { status2panel.BackColor = Color.LightGreen; }
                        else { status2panel.BackColor = Color.FromArgb(90, 184, 255); }
                        status2.Text = s;
                    }
                    else if (c == "logger3")
                    {
                        if (s.Contains("Failed")) { status3panel.BackColor = Settings.Default.AlertColor; }
                        else if (s.Contains("Success")) { status3panel.BackColor = Color.LightGreen; }
                        else { status3panel.BackColor = Color.FromArgb(90, 184, 255); }
                    }
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
                Program.viewer.sendHeartbeatPacket();
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
            }
            else
            {
                IsUsingMouse = false;
            }
        }
        
        private void pctSurface_MouseDown(object sender, MouseEventArgs e) { isMouseDown = true; }
        private void pctSurface_MouseUp(object sender, MouseEventArgs e) { isMouseDown = false; }
        public IntPtr getDrawSurface()
        {
            return pctSurface.Handle;
        }
        #endregion
        #region IPDetector Handling
        private void IPDetecter_Tick(object sender, EventArgs e)
        {
            if (ValidateIP(Settings.Default.IPAddress) == true)
            {
                IPDetecter.Stop();
                IPDetecter.Enabled = false;
            }
            else if (NetUtil.IPv4.GetFirst3DS().ToString() != "")
            {
                Settings.Default.IPAddress = NetUtil.IPv4.GetFirst3DS().ToString();
                Settings.Default.Save();
                ipaddress.Text = Settings.Default.IPAddress;
                UpdateIP();
            }
        }
        #endregion

        // Events
        #region ConnectButton
        private void ConnectButton_Click(object sender, EventArgs e)
        {
            if (ConnectButton.Text == "CONNECT") { mempatch = false; connect(Settings.Default.IPAddress, 8000); Program.viewer.sendEmptyPacket(5); } else { Program.viewer.disconnect(); }
        }
        #endregion
        #region MemPatchButton
        private void MemPatchButton_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("You are about to send a temporary patch that disables In-Game Online Services. Are you sure?", "Are You Sure?", MessageBoxButtons.YesNo);
            if (dr == DialogResult.Yes)
            {
                mempatch = true;
                connect(Settings.Default.IPAddress, 8000, false);
            }
        }
        #endregion
        #region PushButton
        private void PushButton_Click(object sender, EventArgs e)
        {
            
            try
            {
                
                if (PushFiles != null)
                {
                    
                    log("Connecting to '" + Settings.Default.IPAddress + "'...", "logger2", "Starting HTTPServer...");
                    ss = new httpserver.httpserver(8080, ActiveDir);
                    ss.Start();
                    
                    System.Threading.Thread.Sleep(100);

                    log("Connecting to '" + Settings.Default.IPAddress + "'...", "logger2", "Opening Socket...");
                    s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    IAsyncResult result = s.BeginConnect(Settings.Default.IPAddress, 5000, null, null);
                    result.AsyncWaitHandle.WaitOne(5000, true);

                    if (!s.Connected)
                    {
                        s.Close();
                        ss.Stop();
                        log("Failed!\n- Make sure FBI is open and you are in 'Network Install' -> 'Receive URLs over the network',\n- Wi-Fi Adapter and Router might not be getting a strong enough connection,\n- IP Address could be incorrect (It changes every now and then).", "logger2", "Failed to connect!");
                        return;
                    }
                    else
                    {

                        logger2.Text = "Pushing...\n";
                        foreach (var file in PushFiles) { logger2.Text += Path.GetFileName(file) + "\n"; }
                        log("false", "logger2", "Sending file-list...");

                        string b = "";
                        foreach (var file in PushFiles) { b += NetUtil.IPv4.Local + ":8080/" + Uri.EscapeUriString(Path.GetFileName(file)) + "\n"; }
                        byte[] Largo = BitConverter.GetBytes((uint)Encoding.ASCII.GetBytes(b).Length);
                        byte[] Adress = Encoding.ASCII.GetBytes(b);
                        Array.Reverse(Largo); //Endian fix
                        byte[] outputBytes = new byte[Largo.Length + Adress.Length];
                        Buffer.BlockCopy(Largo, 0, outputBytes, 0, Largo.Length);
                        Buffer.BlockCopy(Adress, 0, outputBytes, Largo.Length, Adress.Length);
                        s.Send(outputBytes);
                        
                        log("false", "logger2", "Pushing files...");
                        s.BeginReceive(new byte[1], 0, 1, 0, new AsyncCallback(onPushed), null);

                    }

                }
                else
                {
                    MessageBox.Show("Please add some files to Queue using the + button before trying to push.");
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something went really wrong: " + Environment.NewLine + Environment.NewLine + "\"" + ex.Message + "\"" + Environment.NewLine + Environment.NewLine + "If this keeps happening, please take a screenshot of this message and post it on github." + Environment.NewLine + Environment.NewLine + "The program will close now", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }

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
                    logger2.Text = "";
                    foreach (string file in PushFiles)
                    {
                        if (ActiveDir == Path.GetDirectoryName(file))
                        {
                            log("Added '" + Path.GetFileName(file) + "' to Queue", "logger2", "Ready to push...");
                            PushButton.Enabled = true;
                        }
                        else
                        {
                            MessageBox.Show("Somehow you managed to pick 2 files that are in different folders." + Environment.NewLine + "Multi-File pushing would need the entire computer hosted to the network and that doesn't feel safe in my book." + Environment.NewLine + "Maybe in the future I'll find a way to do this.", "Woah there...", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
        #region InputRedirection CIA Download
        private void customLabel4_Click(object sender, EventArgs e) { Process.Start("https://github.com/initPRAGMA/kit-kat/raw/master/InputRedirectionNTR.cia"); }
        #endregion
        #region CustomTabControl SelectedIndexChanged
        private void customTabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // If it's the InputRedirecter Tab
            if (customTabControl1.SelectedIndex == 2 && irstatus == false)
            {
                // Start InputRedirecter
                InputRedirection.Game1 game = new InputRedirection.Game1(getDrawSurface());
                game.Run();
                // Store that it's running
                irstatus = true;
            }
        }
        #endregion
        #region Tutorial Buttons
        private void materialButton1_Click(object sender, EventArgs e) { Process.Start("https://www.youtube.com/watch?v=VYJcxlGz02w"); }

        private void materialButton2_Click(object sender, EventArgs e) { Process.Start("https://www.youtube.com/watch?v=PFaac9DmPQo"); }

        private void materialButton5_Click(object sender, EventArgs e) { Process.Start("https://gbatemp.net/threads/how-to-use-input-redirection-on-kit-kat.455233"); }
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
                Task<bool> Patch = Program.viewer.waitNTRwrite(0x3DFFD0, 0xE3A01000, pid);
                if (!(await Patch))
                    Console.WriteLine("[ERROR: An error has ocurred while applying the connection patch.]");
                log("[Written Sun/Moon NFC Patch!]", "logger");
                Program.viewer.disconnect();
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
            if (ValidateIP(host) == true)
            {
                log("If you are stuck here with a blank NTRViewer screen\nTry disabling Firewall and/or Anti-Virus's (even Windows Defender).\nThis is a common issue where NTRViewer is being denied from receiving packets but can send them.", "logger", "Trying to connect...");

                // Shut down NTRViewer
                if (closeNTR == true) { foreach (Process p in Process.GetProcessesByName("NTRViewer")) { p.Kill(); p.WaitForExit(); } }

                // Connect to Server
                Program.viewer.setServer(host, port);
                Program.viewer.connectToServer();
            }
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
                Program.viewer.sendWriteMemPacket(0x0105AE4, 0x1a, bytes);

            }
            else
            {

                // Activate Remote Play
                Program.viewer.sendEmptyPacket(901, (uint)Settings.Default.ScreenPriority << 8 | (uint)Settings.Default.PriorityFactor, (uint)Settings.Default.Quality, (uint)(Settings.Default.QOSValue * 1024 * 1024 / 8));
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

                        if (Settings.Default.ShowConsole == false)
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
                else
                {
                    log("[ERROR: NTRViewer failed to extract, try downloading and running NTRViewer manually as an Administrator.]", "logger");
                }
                #endregion

                // Open Linked Batch File
                if (Settings.Default.BatchFile != "") { Process.Start(Settings.Default.BatchFile); }

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
                if (ValidateIP(Settings.Default.IPAddress) != false)
                {
                    ConnectButton.Enabled = true;
                    PushFileSelectButton.Enabled = true;
                    logger.Text = "Ready to connect to '" + Settings.Default.IPAddress + "'";
                    logger2.Text = "Ready to connect to '" + Settings.Default.IPAddress + "'";
                }
                else
                {
                    ConnectButton.Enabled = false;
                    PushButton.Enabled = false;
                    PushFileSelectButton.Enabled = false;
                    logger.Text = "The IP '" + Settings.Default.IPAddress + "' is not valid.";
                    logger2.Text = "The IP '" + Settings.Default.IPAddress + "' is not valid.";
                }
            }
            else
            {
                ConnectButton.Enabled = false;
                PushButton.Enabled = false;
                PushFileSelectButton.Enabled = false;
                logger.Text = "3DS IP is not configured.";
                logger2.Text = "3DS IP is not configured.";
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
                log("false", "logger2", "Successfully Pushed Files!");
            });
            
            s.Close();
            ss.Stop();
        }
        #endregion

    }
}
