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
using System.Text.RegularExpressions;

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

            ThemeUpdater();

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
            #region Remove Extracted files
            File.Delete(Path.Combine(Path.GetTempPath(), "NTRViewer.exe"));
            File.Delete(Path.Combine(Path.GetTempPath(), "SDL2.dll"));
            File.Delete(Path.Combine(Path.GetTempPath(), "turbojpeg.dll"));
            File.Delete(Path.Combine(Path.GetTempPath(), "3dstool.exe"));
            File.Delete(Path.Combine(Path.GetTempPath(), "ctrtool.exe"));
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
                    else if (c == "logger4")
                    {
                        logger4.Text = l;
                    }
                }

                if (s != "")
                {
                    if (c == "logger")
                    {
                        if (s.Contains("Failed")) { status1panel.BackColor = Settings.Default.AlertColor; }
                        else if (s.Contains("Success")) { status1panel.BackColor = Color.LightGreen; }
                        else { status1panel.BackColor = Color.FromArgb(90, 184, 255); }
                    }
                    else if (c == "logger2")
                    {
                        if (s.Contains("Failed")) { status2panel.BackColor = Settings.Default.AlertColor; }
                        else if (s.Contains("Success")) { status2panel.BackColor = Color.LightGreen; }
                        else { status2panel.BackColor = Color.FromArgb(90, 184, 255); }
                    }
                    else if (c == "logger3")
                    {
                        if (s.Contains("Failed")) { status3panel.BackColor = Settings.Default.AlertColor; }
                        else if (s.Contains("Success")) { status3panel.BackColor = Color.LightGreen; }
                        else { status3panel.BackColor = Color.FromArgb(90, 184, 255); }
                    }
                    else if (c == "logger4")
                    {
                        if (s.Contains("Failed")) { status4panel.BackColor = Settings.Default.AlertColor; }
                        else if (s.Contains("Success")) { status4panel.BackColor = Color.LightGreen; }
                        else { status4panel.BackColor = Color.FromArgb(90, 184, 255); }
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
                MessageBox.Show("Linked " + dg.FileName);
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
            if (MainTab.SelectedIndex == 2 && irstatus == false)
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
        private void RK_HelpButton_Click(object sender, EventArgs e) { Process.Start("https://gbatemp.net/threads/how-to-use-rom-kit-from-kit-kat-to-extract-rebuild-3ds-and-cia-files.456630"); }
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
                log("Trying to connect...", "logger");

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
                        log("Successfully Connected...\n\nWhitescreen? Disable Firewall and any Anti-Virus including Windows Defender.\nFirewall and Anti-Virus's are usually the cause of the White Screen of Death.", "logger", "Success");
                    }
                    catch (Exception err)
                    {
                        log(err.Message, "logger", "Failed");
                    }
                }
                else
                {
                    log("NTRViewer failed to extract, try downloading and running NTRViewer manually as an Administrator.", "logger");
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
        #region ColorPicker
        private void ColorPicker(string Control)
        {
            if(Control != "")
            {
                Control c = null;
                if (Control == "WindowColorPanel") { c = WindowColorPanel; }
                else if(Control == "HighlightColorPanel") { c = HighlightColorPanel; }
                else if (Control == "TabBackgroundColorPanel") { c = TabBackgroundColorPanel; }
                else if (Control == "TextColorPanel") { c = TextColorPanel; }

                ColorPicker frm = new ColorPicker(c.BackColor);
                frm.Font = SystemFonts.DialogFont;
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    c.BackColor = frm.PrimaryColor;
                    if (Control == "WindowColorPanel") { Settings.Default["WindowColor"] = frm.PrimaryColor; }
                    else if (Control == "HighlightColorPanel") { Settings.Default["HighlightColor"] = frm.PrimaryColor; }
                    else if (Control == "TabBackgroundColorPanel") { Settings.Default["TabBackgroundColor"] = frm.PrimaryColor; }
                    else if (Control == "TextColorPanel") { Settings.Default["FontColor"] = frm.PrimaryColor; }
                    Settings.Default.Save();
                    ThemeUpdater();
                }
            }
        }
        #endregion

        private void materialButton4_Click(object sender, EventArgs e)
        {

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "3DS Rom Dump (*.3ds, *.cia)|*.3ds;*.cia";
            ofd.FilterIndex = 0;
            ofd.Multiselect = true;

            if (ofd.ShowDialog() == DialogResult.OK && ofd.FileNames.Length > 0)
            {

                log("Extracting...", "logger4");

                string folder = "Rom-Kit/" + ofd.SafeFileName.Split('.')[0].ToString();
                StringBuilder sb = new StringBuilder();

                ProcessStartInfo p = new ProcessStartInfo(Path.Combine(Path.GetTempPath(), "3dstool.exe"));
                p.Verb = "runas";
                p.WorkingDirectory = Path.Combine(Directory.GetCurrentDirectory(), folder+" (.3ds)");
                p.UseShellExecute = false;
                p.CreateNoWindow = true;
                
                ProcessStartInfo p2 = new ProcessStartInfo(Path.Combine(Path.GetTempPath(), "ctrtool.exe"));
                p2.Verb = "runas";
                p2.WorkingDirectory = Path.Combine(Directory.GetCurrentDirectory(), folder+" (.3ds)");
                p2.UseShellExecute = false;
                p2.CreateNoWindow = true;

                if (ofd.SafeFileName.Contains(".3ds"))
                {

                    #region Extract .3DS

                    // If the Rom-Kit folder doesn't exist create it
                    if (!Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), folder+" (.3ds)"))) {
                        Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), folder+" (.3ds)"));
                    }
                    else
                    {
                        log("There are already extracted rom's in the Rom-Kit folder\nRemoving them before we start the next Extraction.\n\nIt may freeze and look like Kit-Kat is not-responding, simply leave it a while and it will start responding.", "logger4");
                        DirectoryInfo directory = new DirectoryInfo(@Path.Combine(Directory.GetCurrentDirectory(), folder+" (.3ds)"));
                        foreach (FileInfo file in directory.GetFiles()) file.Delete();
                        foreach (DirectoryInfo subDirectory in directory.GetDirectories()) subDirectory.Delete(true);
                    }

                    log("Decrypting partitions...", "logger4");
                    sb = new StringBuilder();
                    sb.Append("-xtf 3ds \"" + ofd.FileName + "\" ");
                    sb.Append("--header HeaderNCCH.bin -0 DecryptedPartition0.bin -1 DecryptedPartition1.bin -2 DecryptedPartition2.bin -6 DecryptedPartition6.bin -7 DecryptedPartition7.bin");
                    p.Arguments = sb.ToString().Replace(',', '.');
                    Process.Start(p).WaitForExit();

                    if (Directory.EnumerateFiles(folder + " (.cia)").Any())
                    {

                        log("Decrypting ExeFS, RomFS, LogoLZ and PlainRGN...", "logger4");
                        sb = new StringBuilder();
                        sb.Append("-xtf cxi DecryptedPartition0.bin --header HeaderNCCH0.bin --exh DecryptedExHeader.bin --exefs DecryptedExeFS.bin --romfs DecryptedRomFS.bin --logo LogoLZ.bin --plain PlainRGN.bin");
                        p.Arguments = sb.ToString().Replace(',', '.');
                        Process.Start(p).WaitForExit();

                        log("Decrypting Manual...", "logger4");
                        sb = new StringBuilder();
                        sb.Append("-xtf cfa DecryptedPartition1.bin --header HeaderNCCH1.bin --romfs DecryptedManual.bin");
                        p.Arguments = sb.ToString().Replace(',', '.');
                        Process.Start(p).WaitForExit();

                        log("Decrypting DownloadPlay...", "logger4");
                        sb = new StringBuilder();
                        sb.Append("-xtf cfa DecryptedPartition2.bin --header HeaderNCCH2.bin --romfs DecryptedDownloadPlay.bin");
                        p.Arguments = sb.ToString().Replace(',', '.');
                        Process.Start(p).WaitForExit();

                        log("Decrypting n3DSUpdate...", "logger4");
                        sb = new StringBuilder();
                        sb.Append("-xtf cfa DecryptedPartition6.bin --header HeaderNCCH6.bin --romfs DecryptedN3DSUpdate.bin");
                        p.Arguments = sb.ToString().Replace(',', '.');
                        Process.Start(p).WaitForExit();

                        log("Decrypting o3DSUpdate...", "logger4");
                        sb = new StringBuilder();
                        sb.Append("-xtf cfa DecryptedPartition6.bin --header HeaderNCCH6.bin --romfs DecryptedO3DSUpdate.bin");
                        p.Arguments = sb.ToString().Replace(',', '.');
                        Process.Start(p).WaitForExit();

                        log("Deleting Decrypted Partitions...", "logger4");
                        File.Delete(folder+" (.3ds)"+"/DecryptedPartition0.bin");
                        File.Delete(folder+" (.3ds)"+"/DecryptedPartition1.bin");
                        File.Delete(folder+" (.3ds)"+"/DecryptedPartition2.bin");
                        File.Delete(folder+" (.3ds)"+"/DecryptedPartition6.bin");
                        File.Delete(folder+" (.3ds)"+"/DecryptedPartition7.bin");

                        log("Extracting RomFS...", "logger4");
                        sb = new StringBuilder();
                        sb.Append("-xtf romfs DecryptedRomFS.bin --romfs-dir ExtractedRomFS");
                        p.Arguments = sb.ToString().Replace(',', '.');
                        Process.Start(p).WaitForExit();

                        log("Extracting Manual...", "logger4");
                        sb = new StringBuilder();
                        sb.Append("-xtf romfs DecryptedManual.bin --romfs-dir ExtractedManual");
                        p.Arguments = sb.ToString().Replace(',', '.');
                        Process.Start(p).WaitForExit();

                        log("Extracting DownloadPlay...", "logger4");
                        sb = new StringBuilder();
                        sb.Append("-xtf romfs DecryptedDownloadPlay.bin --romfs-dir ExtractedDownloadPlay");
                        p.Arguments = sb.ToString().Replace(',', '.');
                        Process.Start(p).WaitForExit();

                        log("Extracting n3DSUpdate...", "logger4");
                        sb = new StringBuilder();
                        sb.Append("-xtf romfs DecryptedN3DSUpdate.bin --romfs-dir ExtractedN3DSUpdate");
                        p.Arguments = sb.ToString().Replace(',', '.');
                        Process.Start(p).WaitForExit();

                        log("Extracting o3DSUpdate...", "logger4");
                        sb = new StringBuilder();
                        sb.Append("-xtf romfs DecryptedO3DSUpdate.bin --romfs-dir ExtractedO3DSUpdate");
                        p.Arguments = sb.ToString().Replace(',', '.');
                        Process.Start(p).WaitForExit();

                        log("Extracting ExeFS...", "logger4");
                        sb = new StringBuilder();
                        sb.Append("-xutf exefs DecryptedExeFS.bin --exefs-dir ExtractedExeFS --header HeaderExeFS.bin");
                        p.Arguments = sb.ToString().Replace(',', '.');
                        Process.Start(p).WaitForExit();

                        log("Renaming banner and icon files...", "logger4");
                        File.Move(folder+" (.3ds)"+"/ExtractedExeFS/banner.bnr", folder+" (.3ds)"+"/ExtractedExeFS/banner.bin");
                        File.Move(folder+" (.3ds)"+"/ExtractedExeFS/icon.icn", folder+" (.3ds)"+"/ExtractedExeFS/icon.bin");

                        log("Extracting Banner...", "logger4");
                        sb = new StringBuilder();
                        sb.Append("-x -t banner -f \"ExtractedExeFS/banner.bin\" --banner-dir ExtractedBanner");
                        p.Arguments = sb.ToString().Replace(',', '.');
                        Process.Start(p).WaitForExit();

                        log("Renaming banner0.bcmdl and deleting banner.bin...", "logger4");
                        File.Move(folder+" (.3ds)"+"/ExtractedBanner/banner0.bcmdl", folder+" (.3ds)"+"/ExtractedBanner/banner.cgfx");
                        File.Delete(folder+" (.3ds)"+"/ExtractedBanner/banner.bin");

                        log("Successfully extracted to '"+folder+" (.3ds)"+"'.", "logger4", "Success");

                    }
                    else
                    {
                        log(".3ds file must be decrypted in order to be extracted.", "logger4", "Failed");
                    }
                    #endregion

                } else
                {

                    #region Extract.cia

                    // If the Rom-Kit folder doesn't exist create it
                    if (!Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), folder+" (.cia)")))
                    {
                        Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), folder+" (.cia)"));
                    }
                    else
                    {
                        log("There are already extracted rom's in the Rom-Kit folder\nRemoving them before we start the next Extraction.\n\nIt may freeze and look like Kit-Kat is not-responding, simply leave it a while and it will start responding.", "logger4");
                        DirectoryInfo directory = new DirectoryInfo(@Path.Combine(Directory.GetCurrentDirectory(), folder+" (.cia)"));
                        foreach (FileInfo file in directory.GetFiles()) file.Delete();
                        foreach (DirectoryInfo subDirectory in directory.GetDirectories()) subDirectory.Delete(true);
                    }
                    
                    log("Extracting partition files...", "logger4");
                    sb = new StringBuilder();
                    sb.Append("--content=DecryptedApp \"" + ofd.FileName + "\"");
                    p2.Arguments = sb.ToString().Replace(',', '.');
                    Process.Start(p2).WaitForExit();

                    if (Directory.EnumerateFiles(folder+" (.cia)").Any()) {

                        log("Renaming partition files...", "logger4");
                        DirectoryInfo dir = new DirectoryInfo(@Path.Combine(Directory.GetCurrentDirectory(), folder+" (.cia)"));
                        int i = -1;
                        foreach (FileInfo file in dir.GetFiles())
                        {
                            i++;
                            File.Move(file.FullName, folder+" (.cia)"+"/DecryptedPartition"+i+".bin");
                        }

                        log("Decrypting ExeFS, RomFS, LogoLZ and PlainRGN...", "logger4");
                        sb = new StringBuilder();
                        sb.Append("-xtf cxi DecryptedPartition0.bin --header HeaderNCCH0.bin --exh DecryptedExHeader.bin --exefs DecryptedExeFS.bin --romfs DecryptedRomFS.bin --logo LogoLZ.bin --plain PlainRGN.bin");
                        p.Arguments = sb.ToString().Replace(',', '.');
                        Process.Start(p).WaitForExit();

                        log("Decrypting Manual...", "logger4");
                        sb = new StringBuilder();
                        sb.Append("-xtf cfa DecryptedPartition1.bin --header HeaderNCCH1.bin --romfs DecryptedManual.bin");
                        p.Arguments = sb.ToString().Replace(',', '.');
                        Process.Start(p).WaitForExit();

                        log("Decrypting DownloadPlay...", "logger4");
                        sb = new StringBuilder();
                        sb.Append("-xtf cfa DecryptedPartition2.bin --header HeaderNCCH2.bin --romfs DecryptedDownloadPlay.bin");
                        p.Arguments = sb.ToString().Replace(',', '.');
                        Process.Start(p).WaitForExit();

                        log("Deleting Decrypted Partitions...", "logger4");
                        File.Delete(folder+" (.cia)"+"/DecryptedPartition0.bin");
                        File.Delete(folder+" (.cia)"+"/DecryptedPartition1.bin");
                        File.Delete(folder+" (.cia)"+"/DecryptedPartition2.bin");

                        log("Extracting ExeFS...", "logger4");
                        sb = new StringBuilder();
                        sb.Append("-xutf exefs DecryptedExeFS.bin --exefs-dir ExtractedExeFS --header HeaderExeFS.bin");
                        p.Arguments = sb.ToString().Replace(',', '.');
                        Process.Start(p).WaitForExit();

                        log("Renaming banner and icon files...", "logger4");
                        File.Move(folder+" (.cia)"+"/ExtractedExeFS/banner.bnr", folder+" (.cia)"+"/ExtractedExeFS/banner.bin");
                        File.Move(folder+" (.cia)"+"/ExtractedExeFS/icon.icn", folder+" (.cia)"+"/ExtractedExeFS/icon.bin");

                        log("Extracting Banner...", "logger4");
                        sb = new StringBuilder();
                        sb.Append("-x -t banner -f \"ExtractedExeFS/banner.bin\" --banner-dir ExtractedBanner");
                        p.Arguments = sb.ToString().Replace(',', '.');
                        Process.Start(p).WaitForExit();

                        log("Renaming banner0.bcmdl and deleting banner.bin...", "logger4");
                        File.Move(folder+" (.cia)"+"/ExtractedBanner/banner0.bcmdl", folder+" (.cia)"+"/ExtractedBanner/banner.cgfx");
                        File.Delete(folder+" (.cia)"+"/ExtractedBanner/banner.bin");

                        sb = new StringBuilder();
                        sb.Append("-xtf romfs DecryptedRomFS.bin --romfs-dir ExtractedRomFS");
                        p.Arguments = sb.ToString().Replace(',', '.');
                        Process.Start(p).WaitForExit();

                        sb = new StringBuilder();
                        sb.Append("-xtf romfs DecryptedManual.bin --romfs-dir ExtractedManual");
                        p.Arguments = sb.ToString().Replace(',', '.');
                        Process.Start(p).WaitForExit();

                        sb = new StringBuilder();
                        sb.Append("-xtf romfs DecryptedDownloadPlay.bin --romfs-dir ExtractedDownloadPlay");
                        p.Arguments = sb.ToString().Replace(',', '.');
                        Process.Start(p).WaitForExit();

                        log("Successfully extracted to '" + folder + " (.cia)" + "'.", "logger4", "Success");

                    } else
                    {
                        log(".cia file must be decrypted in order to be extracted.", "logger4", "Failed");
                    }

                    #endregion

                }

            }

        }

        #region Themes

        #region Updater
        private void ThemeUpdater()
        {
            #region Main
            BackColor = Settings.Default.WindowColor;
            SettingsWindow.BackColor = ControlPaint.Dark(Settings.Default.WindowColor, (float)0.15);
            MainTab.BackColor = Settings.Default.TabBackgroundColor;
            MainTab.TabPages[0].BackColor = Settings.Default.TabBackgroundColor;
            MainTab.TabPages[1].BackColor = Settings.Default.TabBackgroundColor;
            MainTab.TabPages[2].BackColor = Settings.Default.TabBackgroundColor;
            MainTab.TabPages[3].BackColor = Settings.Default.TabBackgroundColor;
            MainTab.TabPages[4].BackColor = Settings.Default.TabBackgroundColor;
            MainTab.TabPages[5].BackColor = Settings.Default.TabBackgroundColor;
            MainTab.TabPages[6].BackColor = Settings.Default.TabBackgroundColor;
            MainTab.TabPages[7].BackColor = Settings.Default.TabBackgroundColor;
            SettingsTab.BackColor = Settings.Default.TabBackgroundColor;
            SettingsTab.TabPages[0].BackColor = Settings.Default.TabBackgroundColor;
            SettingsTab.TabPages[1].BackColor = Settings.Default.TabBackgroundColor;
            SettingsTab.TabPages[2].BackColor = Settings.Default.TabBackgroundColor;
            SettingsTab.TabPages[3].BackColor = Settings.Default.TabBackgroundColor;
            SettingsTab.TabPages[4].BackColor = Settings.Default.TabBackgroundColor;
            SettingsTab.TabPages[5].BackColor = Settings.Default.TabBackgroundColor;
            SettingsTab.TabPages[6].BackColor = Settings.Default.TabBackgroundColor;
            if (BatchLinkButton.Enabled == true) { BatchLinkButton.BackColor = Settings.Default.HighlightColor; }
            if (ConnectButton.Enabled == true) { ConnectButton.BackColor = Settings.Default.HighlightColor; }
            if (ConnectButtonHelp.Enabled == true) { ConnectButtonHelp.BackColor = Settings.Default.HighlightColor; }
            if (PushButton.Enabled == true) { PushButton.BackColor = Settings.Default.HighlightColor; }
            if (PushButtonHelp.Enabled == true) { PushButtonHelp.BackColor = Settings.Default.HighlightColor; }
            if (IRHelp.Enabled == true) { IRHelp.BackColor = Settings.Default.HighlightColor; }
            if (RK_ExtractButton.Enabled == true) { RK_ExtractButton.BackColor = Settings.Default.HighlightColor; }
            if (RK_RebuildButton.Enabled == true) { RK_RebuildButton.BackColor = Settings.Default.HighlightColor; }
            if (RK_HelpButton.Enabled == true) { RK_HelpButton.BackColor = Settings.Default.HighlightColor; }
            #endregion
            #region Custom Settings
            WindowColorPanel.BackColor = Settings.Default.WindowColor;
            WindowColorPanel2.BackColor = ControlPaint.Light(ControlPaint.Dark(Settings.Default.WindowColor, (float)0.1), (float)0.35);
            HighlightColorPanel.BackColor = Settings.Default.HighlightColor;
            TabBackgroundColorPanel.BackColor = Settings.Default.TabBackgroundColor;
            #endregion
        }
        #endregion
        #region Default
        private void DefaultTheme()
        {
            Settings.Default["WindowColor"] = Color.FromArgb(104, 118, 138);
            Settings.Default["TabBackgroundColor"] = Color.White;
            Settings.Default["HighlightColor"] = Color.FromArgb(90, 184, 255);
            Settings.Default["FontColor"] = Color.Black;
            Settings.Default["SeperatorColor"] = Color.FromArgb(240, 240, 240);
            Settings.Default.Save();
            ThemeUpdater();
            AppTitle.ForeColor = Color.White;
        }
        private void panel11_Click(object sender, EventArgs e) { DefaultTheme(); }
        private void panel14_Click(object sender, EventArgs e) { DefaultTheme(); }
        private void panel10_Click(object sender, EventArgs e) { DefaultTheme(); }
        private void customLabel13_Click(object sender, EventArgs e) { DefaultTheme(); }
        #endregion
        #region Discord Light
        private void DiscordLightTheme()
        {
            Settings.Default["WindowColor"] = Color.FromArgb(46, 49, 54);
            Settings.Default["TabBackgroundColor"] = Color.White;
            Settings.Default["HighlightColor"] = Color.FromArgb(90, 184, 255);
            Settings.Default["FontColor"] = Color.Black;
            Settings.Default["SeperatorColor"] = Color.FromArgb(240, 240, 240);
            Settings.Default.Save();
            ThemeUpdater();
            AppTitle.ForeColor = Color.White;
        }
        private void panel9_Click(object sender, EventArgs e) { DiscordLightTheme(); }
        private void panel12_Click(object sender, EventArgs e) { DiscordLightTheme(); }
        private void panel8_Click(object sender, EventArgs e) { DiscordLightTheme(); }
        private void customLabel12_Click(object sender, EventArgs e) { DiscordLightTheme(); }
        #endregion
        #region Discord Dark
        private void DiscordDarkTheme()
        {
            Settings.Default["WindowColor"] = Color.FromArgb(46, 49, 54);
            Settings.Default["TabBackgroundColor"] = Color.FromArgb(54, 57, 62);
            Settings.Default["HighlightColor"] = Color.FromArgb(90, 184, 255);
            Settings.Default["FontColor"] = Color.White;
            Settings.Default["SeperatorColor"] = Color.FromArgb(60, 60, 60);
            Settings.Default.Save();
            ThemeUpdater();
            AppTitle.ForeColor = Color.White;
        }
        private void panel17_Click(object sender, EventArgs e) { DiscordDarkTheme(); }
        private void panel18_Click(object sender, EventArgs e) { DiscordDarkTheme(); }
        private void panel15_Click(object sender, EventArgs e) { DiscordDarkTheme(); }
        private void customLabel14_Click(object sender, EventArgs e) { DiscordDarkTheme(); }
        #endregion

        #endregion

        private void panelColor_Click(object sender, EventArgs e)
        {
            ColorPicker("WindowColorPanel");
        }

        private void panel20_Click(object sender, EventArgs e)
        {
            ColorPicker("HighlightColorPanel");
        }

        private void WindowColorPanel2_Click(object sender, EventArgs e)
        {
            ColorPicker("WindowColorPanel");
        }

        private void TabBackgroundColorPanel_Click(object sender, EventArgs e)
        {
            ColorPicker("TabBackgroundColorPanel");
        }

        private void customLabel25_Click(object sender, EventArgs e)
        {
            ColorPicker("TextColorPanel");
        }
        
        private void materialButton1_Click_1(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            DialogResult result = fbd.ShowDialog();
            if (!string.IsNullOrWhiteSpace(fbd.SelectedPath))
            {

                log("Extracting...", "logger4");
                StringBuilder sb = new StringBuilder();

                ProcessStartInfo p = new ProcessStartInfo(Path.Combine(Path.GetTempPath(), "3dstool.exe"));
                p.Verb = "runas";
                p.WorkingDirectory = fbd.SelectedPath;
                p.UseShellExecute = false;
                p.CreateNoWindow = true;

                ProcessStartInfo p2 = new ProcessStartInfo(Path.Combine(Path.GetTempPath(), "ctrtool.exe"));
                p2.Verb = "runas";
                p2.WorkingDirectory = fbd.SelectedPath;
                p2.UseShellExecute = false;
                p2.CreateNoWindow = true;

                if (fbd.SelectedPath.Contains(".3ds"))
                {

                    #region Rebuild .3DS

                    log("Rebuilding RomFS...", "logger4");
                    sb = new StringBuilder();
                    sb.Append("-ctf romfs CustomRomFS.bin --romfs-dir ExtractedRomFS");
                    p.Arguments = sb.ToString().Replace(',', '.');
                    Process.Start(p).WaitForExit();

                    log("Renaming banner and icon files...", "logger4");
                    File.Move(fbd.SelectedPath + "/ExtractedExeFS/banner.bin", fbd.SelectedPath + "/ExtractedExeFS/banner.bnr");
                    File.Move(fbd.SelectedPath + "/ExtractedExeFS/icon.bin", fbd.SelectedPath + "/ExtractedExeFS/icon.icn");

                    log("Rebuilding ExeFS...", "logger4");
                    sb = new StringBuilder();
                    sb.Append("-ctf exefs CustomExeFS.bin --exefs-dir ExtractedExeFS --header HeaderExeFS.bin");
                    p.Arguments = sb.ToString().Replace(',', '.');
                    Process.Start(p).WaitForExit();

                    log("Renaming banner and icon files...", "logger4");
                    File.Move(fbd.SelectedPath + "/ExtractedExeFS/banner.bnr", fbd.SelectedPath + "/ExtractedExeFS/banner.bin");
                    File.Move(fbd.SelectedPath + "/ExtractedExeFS/icon.icn", fbd.SelectedPath + "/ExtractedExeFS/icon.bin");

                    log("Rebuilding Manual...", "logger4");
                    sb = new StringBuilder();
                    sb.Append("-ctf romfs CustomManual.bin --romfs-dir ExtractedManual");
                    p.Arguments = sb.ToString().Replace(',', '.');
                    Process.Start(p).WaitForExit();

                    log("Rebuilding DownloadPlay...", "logger4");
                    sb = new StringBuilder();
                    sb.Append("-ctf romfs CustomDownloadPlay.bin --romfs-dir ExtractedDownloadPlay");
                    p.Arguments = sb.ToString().Replace(',', '.');
                    Process.Start(p).WaitForExit();

                    log("Rebuilding n3DSUpdate...", "logger4");
                    sb = new StringBuilder();
                    sb.Append("-ctf romfs CustomN3DSUpdate.bin --romfs-dir ExtractedN3DSUpdate");
                    p.Arguments = sb.ToString().Replace(',', '.');
                    Process.Start(p).WaitForExit();

                    log("Rebuilding o3DSUpdate...", "logger4");
                    sb = new StringBuilder();
                    sb.Append("-ctf romfs CustomO3DSUpdate.bin --romfs-dir ExtractedO3DSUpdate");
                    p.Arguments = sb.ToString().Replace(',', '.');
                    Process.Start(p).WaitForExit();

                    log("Rebuilding Partition0...", "logger4");
                    sb = new StringBuilder();
                    sb.Append("-ctf cxi CustomPartition0.bin --header HeaderNCCH0.bin --exh DecryptedExHeader.bin --exefs CustomExeFS.bin --romfs CustomRomFS.bin --logo LogoLZ.bin --plain PlainRGN.bin");
                    p.Arguments = sb.ToString().Replace(',', '.');
                    Process.Start(p).WaitForExit();

                    log("Rebuilding Partition1...", "logger4");
                    sb = new StringBuilder();
                    sb.Append("-ctf cfa CustomPartition1.bin --header HeaderNCCH1.bin --romfs CustomManual.bin");
                    p.Arguments = sb.ToString().Replace(',', '.');
                    Process.Start(p).WaitForExit();

                    log("Rebuilding Partition2...", "logger4");
                    sb = new StringBuilder();
                    sb.Append("-ctf cfa CustomPartition2.bin --header HeaderNCCH2.bin --romfs CustomDownloadPlay.bin");
                    p.Arguments = sb.ToString().Replace(',', '.');
                    Process.Start(p).WaitForExit();

                    log("Rebuilding Partition6...", "logger4");
                    sb = new StringBuilder();
                    sb.Append("-ctf cfa CustomPartition6.bin --header HeaderNCCH6.bin --romfs CustomN3DSUpdate.bin");
                    p.Arguments = sb.ToString().Replace(',', '.');
                    Process.Start(p).WaitForExit();

                    log("Rebuilding Partition7...", "logger4");
                    sb = new StringBuilder();
                    sb.Append("-ctf cfa CustomPartition7.bin --header HeaderNCCH7.bin --romfs CustomO3DSUpdate.bin");
                    p.Arguments = sb.ToString().Replace(',', '.');
                    Process.Start(p).WaitForExit();

                    log("Parsing Partitions...", "logger4");
                    DirectoryInfo di = new DirectoryInfo(fbd.SelectedPath);
                    FileInfo[] fi = di.GetFiles("Custom*.bin");
                    foreach (FileInfo file in fi) { if (file.Length <= 20000) { file.Delete(); } }

                    MatchCollection matches = new Regex(@"[a-z]:\\(?:[^\\:]+\\)*((?:[^:\\]+))", RegexOptions.IgnoreCase).Matches(fbd.SelectedPath);
                    log("Rebuilding .3ds Rom...", "logger4");
                    sb = new StringBuilder();
                    sb.Append("-ctf 3ds Rebuilt.3ds --header HeaderNCCH.bin -0 CustomPartition0.bin -1 CustomPartition1.bin -2 CustomPartition2.bin -6 CustomPartition6.bin -7 CustomPartition7.bin");
                    p.Arguments = sb.ToString().Replace(',', '.');
                    Process.Start(p).WaitForExit();
                    
                    log("Rebuilt the .3ds rom to '"+fbd.SelectedPath+"'.", "logger4", "Success");

                    #endregion

                }

            }
        }

    }

}
