namespace kitkat
{
    partial class CTRV
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CTRV));
            this.DisconnectTimeout = new System.Windows.Forms.Timer(this.components);
            this.TabFixer = new System.Windows.Forms.Panel();
            this.Heartbeat = new System.Windows.Forms.Timer(this.components);
            this.controlBox1 = new kitkat.ControlBox();
            this.customLabel1 = new kitkat.CustomLabel();
            this.customTabControl1 = new kitkat.CustomTabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.logger = new kitkat.CustomLabel();
            this.MemPatchButton = new kitkat.MaterialButton();
            this.ConnectButton = new kitkat.MaterialButton();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.hnlogger = new System.Windows.Forms.RichTextBox();
            this.NetPass = new System.Windows.Forms.TextBox();
            this.RestartHostedNetwork = new kitkat.MaterialButton();
            this.separator4 = new kitkat.Separator();
            this.NetSSID = new System.Windows.Forms.TextBox();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.customLabel2 = new kitkat.CustomLabel();
            this.PushFileSelectButton = new kitkat.MaterialButton();
            this.logger3 = new kitkat.CustomLabel();
            this.PushButton = new kitkat.MaterialButton();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.customLabel3 = new kitkat.CustomLabel();
            this.irlog = new kitkat.CustomLabel();
            this.pctSurface = new System.Windows.Forms.PictureBox();
            this.Hidden4 = new System.Windows.Forms.TabPage();
            this.Hidden5 = new System.Windows.Forms.TabPage();
            this.Hidden6 = new System.Windows.Forms.TabPage();
            this.tabPage8 = new System.Windows.Forms.TabPage();
            this.ShowConsole = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tScale = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.bScale = new System.Windows.Forms.TextBox();
            this.AutoConnect = new System.Windows.Forms.CheckBox();
            this.separator3 = new kitkat.Separator();
            this.BatchLinkButton = new kitkat.MaterialButton();
            this.label8 = new System.Windows.Forms.Label();
            this.separator2 = new kitkat.Separator();
            this.separator1 = new kitkat.Separator();
            this.ViewMode = new System.Windows.Forms.NumericUpDown();
            this.ipaddress = new System.Windows.Forms.TextBox();
            this.ScreenPriority = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.QOSValue = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.PriorityFactor = new System.Windows.Forms.NumericUpDown();
            this.Quality = new System.Windows.Forms.NumericUpDown();
            this.label9 = new System.Windows.Forms.Label();
            this.customTabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pctSurface)).BeginInit();
            this.tabPage8.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ViewMode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ScreenPriority)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.QOSValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PriorityFactor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Quality)).BeginInit();
            this.SuspendLayout();
            // 
            // DisconnectTimeout
            // 
            this.DisconnectTimeout.Interval = 2000;
            this.DisconnectTimeout.Tick += new System.EventHandler(this.DisconnectTimeout_Tick);
            // 
            // TabFixer
            // 
            this.TabFixer.Location = new System.Drawing.Point(0, 27);
            this.TabFixer.Name = "TabFixer";
            this.TabFixer.Size = new System.Drawing.Size(690, 2);
            this.TabFixer.TabIndex = 21;
            // 
            // Heartbeat
            // 
            this.Heartbeat.Enabled = true;
            this.Heartbeat.Interval = 1000;
            this.Heartbeat.Tick += new System.EventHandler(this.Heartbeat_Tick);
            // 
            // controlBox1
            // 
            this.controlBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.controlBox1.Font = new System.Drawing.Font("Verdana", 8F);
            this.controlBox1.Location = new System.Drawing.Point(622, 0);
            this.controlBox1.Name = "controlBox1";
            this.controlBox1.Size = new System.Drawing.Size(68, 29);
            this.controlBox1.TabIndex = 18;
            this.controlBox1.Text = "controlBox1";
            // 
            // customLabel1
            // 
            this.customLabel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.customLabel1.ForeColor = System.Drawing.Color.White;
            this.customLabel1.Location = new System.Drawing.Point(0, 0);
            this.customLabel1.Name = "customLabel1";
            this.customLabel1.Size = new System.Drawing.Size(54, 29);
            this.customLabel1.TabIndex = 22;
            this.customLabel1.Text = "kit-kat";
            // 
            // customTabControl1
            // 
            this.customTabControl1.Alignment = System.Windows.Forms.TabAlignment.Left;
            this.customTabControl1.Controls.Add(this.tabPage1);
            this.customTabControl1.Controls.Add(this.tabPage2);
            this.customTabControl1.Controls.Add(this.tabPage3);
            this.customTabControl1.Controls.Add(this.tabPage4);
            this.customTabControl1.Controls.Add(this.Hidden4);
            this.customTabControl1.Controls.Add(this.Hidden5);
            this.customTabControl1.Controls.Add(this.Hidden6);
            this.customTabControl1.Controls.Add(this.tabPage8);
            this.customTabControl1.ItemSize = new System.Drawing.Size(36, 38);
            this.customTabControl1.Location = new System.Drawing.Point(-2, 27);
            this.customTabControl1.Margin = new System.Windows.Forms.Padding(0);
            this.customTabControl1.Multiline = true;
            this.customTabControl1.Name = "customTabControl1";
            this.customTabControl1.SelectedIndex = 0;
            this.customTabControl1.Size = new System.Drawing.Size(692, 293);
            this.customTabControl1.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.customTabControl1.TabIndex = 20;
            this.customTabControl1.TabStop = false;
            this.customTabControl1.SelectedIndexChanged += new System.EventHandler(this.customTabControl1_SelectedIndexChanged);
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(229)))), ((int)(((byte)(229)))));
            this.tabPage1.Controls.Add(this.logger);
            this.tabPage1.Controls.Add(this.MemPatchButton);
            this.tabPage1.Controls.Add(this.ConnectButton);
            this.tabPage1.Font = new System.Drawing.Font("Kozuka Gothic Pro B", 9F);
            this.tabPage1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.tabPage1.Location = new System.Drawing.Point(38, 2);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(654, 291);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "tabPage1";
            // 
            // logger
            // 
            this.logger.ForeColor = System.Drawing.Color.Black;
            this.logger.Location = new System.Drawing.Point(0, 41);
            this.logger.Name = "logger";
            this.logger.Size = new System.Drawing.Size(654, 247);
            this.logger.TabIndex = 6;
            this.logger.Text = "Loading IP Address...";
            // 
            // MemPatchButton
            // 
            this.MemPatchButton.BackColor = System.Drawing.Color.IndianRed;
            this.MemPatchButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.MemPatchButton.Font = new System.Drawing.Font("Segoe UI Symbol", 48F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MemPatchButton.ForeColor = System.Drawing.Color.Black;
            this.MemPatchButton.Location = new System.Drawing.Point(377, 10);
            this.MemPatchButton.Margin = new System.Windows.Forms.Padding(0);
            this.MemPatchButton.Name = "MemPatchButton";
            this.MemPatchButton.Size = new System.Drawing.Size(31, 31);
            this.MemPatchButton.TabIndex = 5;
            this.MemPatchButton.Tag = "Alert";
            this.MemPatchButton.Text = "⚠";
            this.MemPatchButton.UseVisualStyleBackColor = false;
            this.MemPatchButton.Click += new System.EventHandler(this.MemPatchButton_Click);
            // 
            // ConnectButton
            // 
            this.ConnectButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(144)))), ((int)(((byte)(202)))), ((int)(((byte)(249)))));
            this.ConnectButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ConnectButton.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ConnectButton.ForeColor = System.Drawing.Color.White;
            this.ConnectButton.Location = new System.Drawing.Point(246, 10);
            this.ConnectButton.Name = "ConnectButton";
            this.ConnectButton.Size = new System.Drawing.Size(131, 31);
            this.ConnectButton.TabIndex = 4;
            this.ConnectButton.Text = "CONNECT";
            this.ConnectButton.UseVisualStyleBackColor = false;
            this.ConnectButton.Click += new System.EventHandler(this.ConnectButton_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(229)))), ((int)(((byte)(229)))));
            this.tabPage2.Controls.Add(this.hnlogger);
            this.tabPage2.Controls.Add(this.NetPass);
            this.tabPage2.Controls.Add(this.RestartHostedNetwork);
            this.tabPage2.Controls.Add(this.separator4);
            this.tabPage2.Controls.Add(this.NetSSID);
            this.tabPage2.Font = new System.Drawing.Font("Kozuka Gothic Pro B", 9F);
            this.tabPage2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.tabPage2.Location = new System.Drawing.Point(38, 2);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(654, 291);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Tag = "";
            this.tabPage2.Text = "tabPage2";
            // 
            // hnlogger
            // 
            this.hnlogger.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.hnlogger.Location = new System.Drawing.Point(2, 69);
            this.hnlogger.Name = "hnlogger";
            this.hnlogger.ReadOnly = true;
            this.hnlogger.Size = new System.Drawing.Size(650, 217);
            this.hnlogger.TabIndex = 25;
            this.hnlogger.Text = "";
            // 
            // NetPass
            // 
            this.NetPass.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.NetPass.Location = new System.Drawing.Point(328, 37);
            this.NetPass.Name = "NetPass";
            this.NetPass.Size = new System.Drawing.Size(324, 26);
            this.NetPass.TabIndex = 24;
            this.NetPass.Text = "Password (min. 8 char.)";
            this.NetPass.TextChanged += new System.EventHandler(this.NetPass_TextChanged);
            // 
            // RestartHostedNetwork
            // 
            this.RestartHostedNetwork.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(144)))), ((int)(((byte)(202)))), ((int)(((byte)(249)))));
            this.RestartHostedNetwork.Cursor = System.Windows.Forms.Cursors.Hand;
            this.RestartHostedNetwork.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RestartHostedNetwork.ForeColor = System.Drawing.Color.White;
            this.RestartHostedNetwork.Location = new System.Drawing.Point(2, 2);
            this.RestartHostedNetwork.Name = "RestartHostedNetwork";
            this.RestartHostedNetwork.Size = new System.Drawing.Size(650, 34);
            this.RestartHostedNetwork.TabIndex = 22;
            this.RestartHostedNetwork.Text = "RESTART HOSTEDNETWORK";
            this.RestartHostedNetwork.UseVisualStyleBackColor = false;
            this.RestartHostedNetwork.Click += new System.EventHandler(this.RestartHostedNetwork_Click);
            // 
            // separator4
            // 
            this.separator4.BackColor = System.Drawing.Color.Gainsboro;
            this.separator4.Location = new System.Drawing.Point(6, 65);
            this.separator4.Name = "separator4";
            this.separator4.Size = new System.Drawing.Size(642, 2);
            this.separator4.TabIndex = 20;
            this.separator4.Text = "separator4";
            // 
            // NetSSID
            // 
            this.NetSSID.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.NetSSID.Location = new System.Drawing.Point(2, 37);
            this.NetSSID.Name = "NetSSID";
            this.NetSSID.Size = new System.Drawing.Size(324, 26);
            this.NetSSID.TabIndex = 19;
            this.NetSSID.Text = "Name";
            this.NetSSID.TextChanged += new System.EventHandler(this.NetSSID_TextChanged);
            // 
            // tabPage3
            // 
            this.tabPage3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(229)))), ((int)(((byte)(229)))));
            this.tabPage3.Controls.Add(this.customLabel2);
            this.tabPage3.Controls.Add(this.PushFileSelectButton);
            this.tabPage3.Controls.Add(this.logger3);
            this.tabPage3.Controls.Add(this.PushButton);
            this.tabPage3.Font = new System.Drawing.Font("Kozuka Gothic Pro B", 9F);
            this.tabPage3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.tabPage3.Location = new System.Drawing.Point(38, 2);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(654, 291);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Tag = "";
            this.tabPage3.Text = "tabPage3";
            // 
            // customLabel2
            // 
            this.customLabel2.Font = new System.Drawing.Font("Kozuka Gothic Pro B", 7F);
            this.customLabel2.ForeColor = System.Drawing.Color.Black;
            this.customLabel2.Location = new System.Drawing.Point(0, 275);
            this.customLabel2.Name = "customLabel2";
            this.customLabel2.Size = new System.Drawing.Size(654, 13);
            this.customLabel2.TabIndex = 12;
            this.customLabel2.Text = "credit: elementalcode";
            // 
            // PushFileSelectButton
            // 
            this.PushFileSelectButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(144)))), ((int)(((byte)(202)))), ((int)(((byte)(249)))));
            this.PushFileSelectButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.PushFileSelectButton.Font = new System.Drawing.Font("Segoe UI Symbol", 48F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PushFileSelectButton.ForeColor = System.Drawing.Color.White;
            this.PushFileSelectButton.Location = new System.Drawing.Point(377, 10);
            this.PushFileSelectButton.Margin = new System.Windows.Forms.Padding(0);
            this.PushFileSelectButton.Name = "PushFileSelectButton";
            this.PushFileSelectButton.Size = new System.Drawing.Size(31, 31);
            this.PushFileSelectButton.TabIndex = 11;
            this.PushFileSelectButton.Tag = "";
            this.PushFileSelectButton.Text = "+";
            this.PushFileSelectButton.UseVisualStyleBackColor = false;
            this.PushFileSelectButton.Click += new System.EventHandler(this.PushFileSelectButton_Click);
            // 
            // logger3
            // 
            this.logger3.ForeColor = System.Drawing.Color.Black;
            this.logger3.Location = new System.Drawing.Point(0, 41);
            this.logger3.Name = "logger3";
            this.logger3.Size = new System.Drawing.Size(654, 247);
            this.logger3.TabIndex = 9;
            this.logger3.Text = "Loading IP Address...";
            // 
            // PushButton
            // 
            this.PushButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(144)))), ((int)(((byte)(202)))), ((int)(((byte)(249)))));
            this.PushButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.PushButton.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PushButton.ForeColor = System.Drawing.Color.White;
            this.PushButton.Location = new System.Drawing.Point(246, 10);
            this.PushButton.Name = "PushButton";
            this.PushButton.Size = new System.Drawing.Size(131, 31);
            this.PushButton.TabIndex = 7;
            this.PushButton.Text = "PUSH";
            this.PushButton.UseVisualStyleBackColor = false;
            this.PushButton.Click += new System.EventHandler(this.PushButton_Click);
            // 
            // tabPage4
            // 
            this.tabPage4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(229)))), ((int)(((byte)(229)))));
            this.tabPage4.Controls.Add(this.customLabel3);
            this.tabPage4.Controls.Add(this.irlog);
            this.tabPage4.Controls.Add(this.pctSurface);
            this.tabPage4.Font = new System.Drawing.Font("Kozuka Gothic Pro B", 9F);
            this.tabPage4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.tabPage4.Location = new System.Drawing.Point(38, 2);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(654, 291);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Tag = "";
            this.tabPage4.Text = "tabPage4";
            // 
            // customLabel3
            // 
            this.customLabel3.Font = new System.Drawing.Font("Kozuka Gothic Pro B", 7F);
            this.customLabel3.ForeColor = System.Drawing.Color.Black;
            this.customLabel3.Location = new System.Drawing.Point(0, 274);
            this.customLabel3.Name = "customLabel3";
            this.customLabel3.Size = new System.Drawing.Size(654, 13);
            this.customLabel3.TabIndex = 24;
            this.customLabel3.Text = "credit: kazo";
            // 
            // irlog
            // 
            this.irlog.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.irlog.ForeColor = System.Drawing.Color.Black;
            this.irlog.Location = new System.Drawing.Point(0, 0);
            this.irlog.Name = "irlog";
            this.irlog.Size = new System.Drawing.Size(654, 32);
            this.irlog.TabIndex = 23;
            this.irlog.Text = "idle";
            // 
            // pctSurface
            // 
            this.pctSurface.BackColor = System.Drawing.Color.Gainsboro;
            this.pctSurface.Location = new System.Drawing.Point(167, 32);
            this.pctSurface.Name = "pctSurface";
            this.pctSurface.Size = new System.Drawing.Size(320, 240);
            this.pctSurface.TabIndex = 10;
            this.pctSurface.TabStop = false;
            this.pctSurface.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pctSurface_MouseDown);
            this.pctSurface.MouseLeave += new System.EventHandler(this.pctSurface_MouseLeave);
            this.pctSurface.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pctSurface_MouseMove);
            this.pctSurface.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pctSurface_MouseUp);
            // 
            // Hidden4
            // 
            this.Hidden4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(229)))), ((int)(((byte)(229)))));
            this.Hidden4.Font = new System.Drawing.Font("Kozuka Gothic Pro B", 9F);
            this.Hidden4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.Hidden4.Location = new System.Drawing.Point(38, 2);
            this.Hidden4.Name = "Hidden4";
            this.Hidden4.Padding = new System.Windows.Forms.Padding(3);
            this.Hidden4.Size = new System.Drawing.Size(654, 291);
            this.Hidden4.TabIndex = 4;
            this.Hidden4.Tag = "Hidden";
            this.Hidden4.Text = "tabPage5";
            // 
            // Hidden5
            // 
            this.Hidden5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(229)))), ((int)(((byte)(229)))));
            this.Hidden5.Font = new System.Drawing.Font("Kozuka Gothic Pro B", 9F);
            this.Hidden5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.Hidden5.Location = new System.Drawing.Point(38, 2);
            this.Hidden5.Name = "Hidden5";
            this.Hidden5.Padding = new System.Windows.Forms.Padding(3);
            this.Hidden5.Size = new System.Drawing.Size(654, 291);
            this.Hidden5.TabIndex = 5;
            this.Hidden5.Tag = "Hidden";
            this.Hidden5.Text = "tabPage6";
            // 
            // Hidden6
            // 
            this.Hidden6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(229)))), ((int)(((byte)(229)))));
            this.Hidden6.Font = new System.Drawing.Font("Kozuka Gothic Pro B", 9F);
            this.Hidden6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.Hidden6.Location = new System.Drawing.Point(38, 2);
            this.Hidden6.Name = "Hidden6";
            this.Hidden6.Padding = new System.Windows.Forms.Padding(3);
            this.Hidden6.Size = new System.Drawing.Size(654, 291);
            this.Hidden6.TabIndex = 6;
            this.Hidden6.Tag = "Hidden";
            this.Hidden6.Text = "tabPage7";
            // 
            // tabPage8
            // 
            this.tabPage8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(229)))), ((int)(((byte)(229)))));
            this.tabPage8.Controls.Add(this.ShowConsole);
            this.tabPage8.Controls.Add(this.label1);
            this.tabPage8.Controls.Add(this.tScale);
            this.tabPage8.Controls.Add(this.label10);
            this.tabPage8.Controls.Add(this.bScale);
            this.tabPage8.Controls.Add(this.AutoConnect);
            this.tabPage8.Controls.Add(this.separator3);
            this.tabPage8.Controls.Add(this.BatchLinkButton);
            this.tabPage8.Controls.Add(this.label8);
            this.tabPage8.Controls.Add(this.separator2);
            this.tabPage8.Controls.Add(this.separator1);
            this.tabPage8.Controls.Add(this.ViewMode);
            this.tabPage8.Controls.Add(this.ipaddress);
            this.tabPage8.Controls.Add(this.ScreenPriority);
            this.tabPage8.Controls.Add(this.label2);
            this.tabPage8.Controls.Add(this.label7);
            this.tabPage8.Controls.Add(this.label4);
            this.tabPage8.Controls.Add(this.label3);
            this.tabPage8.Controls.Add(this.QOSValue);
            this.tabPage8.Controls.Add(this.label5);
            this.tabPage8.Controls.Add(this.label6);
            this.tabPage8.Controls.Add(this.PriorityFactor);
            this.tabPage8.Controls.Add(this.Quality);
            this.tabPage8.Controls.Add(this.label9);
            this.tabPage8.Font = new System.Drawing.Font("Kozuka Gothic Pro B", 9F);
            this.tabPage8.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.tabPage8.Location = new System.Drawing.Point(38, 2);
            this.tabPage8.Name = "tabPage8";
            this.tabPage8.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage8.Size = new System.Drawing.Size(654, 291);
            this.tabPage8.TabIndex = 7;
            this.tabPage8.Tag = "";
            this.tabPage8.Text = "tabPage8";
            // 
            // ShowConsole
            // 
            this.ShowConsole.AutoSize = true;
            this.ShowConsole.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.ShowConsole.Location = new System.Drawing.Point(633, 176);
            this.ShowConsole.Name = "ShowConsole";
            this.ShowConsole.Size = new System.Drawing.Size(15, 14);
            this.ShowConsole.TabIndex = 33;
            this.ShowConsole.UseVisualStyleBackColor = true;
            this.ShowConsole.CheckedChanged += new System.EventHandler(this.ShowConsole_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(7, 175);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(151, 17);
            this.label1.TabIndex = 32;
            this.label1.Text = "Show NTRViewer Console";
            // 
            // tScale
            // 
            this.tScale.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tScale.Location = new System.Drawing.Point(598, 71);
            this.tScale.Name = "tScale";
            this.tScale.Size = new System.Drawing.Size(48, 21);
            this.tScale.TabIndex = 31;
            this.tScale.Text = "1.0";
            this.tScale.TextChanged += new System.EventHandler(this.tScale_TextChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.ForeColor = System.Drawing.Color.Black;
            this.label10.Location = new System.Drawing.Point(7, 74);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(183, 17);
            this.label10.TabIndex = 30;
            this.label10.Text = "Top Screen Scale (0 = Disabled)";
            // 
            // bScale
            // 
            this.bScale.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.bScale.Location = new System.Drawing.Point(598, 95);
            this.bScale.Name = "bScale";
            this.bScale.Size = new System.Drawing.Size(48, 21);
            this.bScale.TabIndex = 29;
            this.bScale.Text = "1.0";
            this.bScale.TextChanged += new System.EventHandler(this.bScale_TextChanged);
            // 
            // AutoConnect
            // 
            this.AutoConnect.AutoSize = true;
            this.AutoConnect.Location = new System.Drawing.Point(633, 51);
            this.AutoConnect.Name = "AutoConnect";
            this.AutoConnect.Size = new System.Drawing.Size(15, 14);
            this.AutoConnect.TabIndex = 27;
            this.AutoConnect.UseVisualStyleBackColor = true;
            this.AutoConnect.CheckedChanged += new System.EventHandler(this.AutoConnect_CheckedChanged);
            // 
            // separator3
            // 
            this.separator3.BackColor = System.Drawing.Color.Gainsboro;
            this.separator3.Location = new System.Drawing.Point(6, 268);
            this.separator3.Name = "separator3";
            this.separator3.Size = new System.Drawing.Size(642, 2);
            this.separator3.TabIndex = 20;
            this.separator3.Text = "separator3";
            // 
            // BatchLinkButton
            // 
            this.BatchLinkButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(144)))), ((int)(((byte)(202)))), ((int)(((byte)(249)))));
            this.BatchLinkButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BatchLinkButton.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BatchLinkButton.ForeColor = System.Drawing.Color.White;
            this.BatchLinkButton.Location = new System.Drawing.Point(511, 3);
            this.BatchLinkButton.Name = "BatchLinkButton";
            this.BatchLinkButton.Size = new System.Drawing.Size(140, 34);
            this.BatchLinkButton.TabIndex = 24;
            this.BatchLinkButton.Text = "LINK .bat";
            this.BatchLinkButton.UseVisualStyleBackColor = false;
            this.BatchLinkButton.Click += new System.EventHandler(this.BatchLinkButton_Click);
            // 
            // label8
            // 
            this.label8.Font = new System.Drawing.Font("Trebuchet MS", 7F, System.Drawing.FontStyle.Bold);
            this.label8.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(104)))), ((int)(((byte)(118)))), ((int)(((byte)(138)))));
            this.label8.Location = new System.Drawing.Point(8, 268);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(640, 20);
            this.label8.TabIndex = 22;
            this.label8.Text = "Created by PRAGMA";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label8.Click += new System.EventHandler(this.label8_Click);
            // 
            // separator2
            // 
            this.separator2.BackColor = System.Drawing.Color.Gainsboro;
            this.separator2.Location = new System.Drawing.Point(6, 168);
            this.separator2.Name = "separator2";
            this.separator2.Size = new System.Drawing.Size(642, 2);
            this.separator2.TabIndex = 19;
            this.separator2.Text = "separator2";
            // 
            // separator1
            // 
            this.separator1.BackColor = System.Drawing.Color.Gainsboro;
            this.separator1.Location = new System.Drawing.Point(6, 39);
            this.separator1.Name = "separator1";
            this.separator1.Size = new System.Drawing.Size(642, 2);
            this.separator1.TabIndex = 18;
            this.separator1.Text = "separator1";
            // 
            // ViewMode
            // 
            this.ViewMode.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.ViewMode.Location = new System.Drawing.Point(598, 119);
            this.ViewMode.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.ViewMode.Name = "ViewMode";
            this.ViewMode.Size = new System.Drawing.Size(48, 21);
            this.ViewMode.TabIndex = 9;
            this.ViewMode.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.ViewMode.ValueChanged += new System.EventHandler(this.ViewMode_ValueChanged);
            // 
            // ipaddress
            // 
            this.ipaddress.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.ipaddress.Location = new System.Drawing.Point(6, 6);
            this.ipaddress.Name = "ipaddress";
            this.ipaddress.Size = new System.Drawing.Size(499, 26);
            this.ipaddress.TabIndex = 3;
            this.ipaddress.Text = "3DS IP Address";
            this.ipaddress.TextChanged += new System.EventHandler(this.ipaddress_TextChanged);
            // 
            // ScreenPriority
            // 
            this.ScreenPriority.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.ScreenPriority.Location = new System.Drawing.Point(598, 243);
            this.ScreenPriority.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.ScreenPriority.Name = "ScreenPriority";
            this.ScreenPriority.Size = new System.Drawing.Size(48, 21);
            this.ScreenPriority.TabIndex = 15;
            this.ScreenPriority.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.ScreenPriority.ValueChanged += new System.EventHandler(this.ScreenPriority_ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(7, 98);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(205, 17);
            this.label2.TabIndex = 6;
            this.label2.Text = "Bottom Screen Scale (0 = Disabled)";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.ForeColor = System.Drawing.Color.Black;
            this.label7.Location = new System.Drawing.Point(7, 145);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(48, 17);
            this.label7.TabIndex = 16;
            this.label7.Text = "Quality";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.Color.Black;
            this.label4.Location = new System.Drawing.Point(6, 197);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(88, 17);
            this.label4.TabIndex = 10;
            this.label4.Text = "Priority Factor";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Location = new System.Drawing.Point(7, 121);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(231, 17);
            this.label3.TabIndex = 7;
            this.label3.Text = "View Mode (1 = Vertical, 0 = Horizontal)";
            // 
            // QOSValue
            // 
            this.QOSValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.QOSValue.Location = new System.Drawing.Point(598, 219);
            this.QOSValue.Maximum = new decimal(new int[] {
            101,
            0,
            0,
            0});
            this.QOSValue.Name = "QOSValue";
            this.QOSValue.Size = new System.Drawing.Size(48, 21);
            this.QOSValue.TabIndex = 14;
            this.QOSValue.Value = new decimal(new int[] {
            101,
            0,
            0,
            0});
            this.QOSValue.ValueChanged += new System.EventHandler(this.QOSValue_ValueChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.Color.Black;
            this.label5.Location = new System.Drawing.Point(6, 221);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(141, 17);
            this.label5.TabIndex = 11;
            this.label5.Text = "Quality of Service Value";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.ForeColor = System.Drawing.Color.Black;
            this.label6.Location = new System.Drawing.Point(7, 245);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(214, 17);
            this.label6.TabIndex = 12;
            this.label6.Text = "Screen Priority (1 = Top, 0 = Bottom)";
            // 
            // PriorityFactor
            // 
            this.PriorityFactor.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.PriorityFactor.Location = new System.Drawing.Point(598, 195);
            this.PriorityFactor.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.PriorityFactor.Name = "PriorityFactor";
            this.PriorityFactor.Size = new System.Drawing.Size(48, 21);
            this.PriorityFactor.TabIndex = 13;
            this.PriorityFactor.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.PriorityFactor.ValueChanged += new System.EventHandler(this.PriorityFactor_ValueChanged);
            // 
            // Quality
            // 
            this.Quality.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.Quality.Location = new System.Drawing.Point(598, 143);
            this.Quality.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.Quality.Name = "Quality";
            this.Quality.Size = new System.Drawing.Size(48, 21);
            this.Quality.TabIndex = 17;
            this.Quality.Value = new decimal(new int[] {
            90,
            0,
            0,
            0});
            this.Quality.ValueChanged += new System.EventHandler(this.Quality_ValueChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.ForeColor = System.Drawing.Color.Black;
            this.label9.Location = new System.Drawing.Point(7, 50);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(145, 17);
            this.label9.TabIndex = 26;
            this.label9.Text = "Auto-Connect on Launch";
            // 
            // CTRV
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(104)))), ((int)(((byte)(118)))), ((int)(((byte)(138)))));
            this.ClientSize = new System.Drawing.Size(690, 317);
            this.Controls.Add(this.controlBox1);
            this.Controls.Add(this.customLabel1);
            this.Controls.Add(this.TabFixer);
            this.Controls.Add(this.customTabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(690, 317);
            this.MinimumSize = new System.Drawing.Size(690, 317);
            this.Name = "CTRV";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CTR-V";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.FormDrag);
            this.customTabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pctSurface)).EndInit();
            this.tabPage8.ResumeLayout(false);
            this.tabPage8.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ViewMode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ScreenPriority)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.QOSValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PriorityFactor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Quality)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TextBox ipaddress;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown ViewMode;
        private System.Windows.Forms.Timer DisconnectTimeout;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown PriorityFactor;
        private System.Windows.Forms.NumericUpDown QOSValue;
        private System.Windows.Forms.NumericUpDown ScreenPriority;
        private System.Windows.Forms.NumericUpDown Quality;
        private System.Windows.Forms.Label label7;
        private ControlBox controlBox1;
        private CustomTabControl customTabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Panel TabFixer;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.TabPage Hidden4;
        private System.Windows.Forms.TabPage Hidden5;
        private System.Windows.Forms.TabPage Hidden6;
        private System.Windows.Forms.TabPage tabPage8;
        private MaterialButton ConnectButton;
        private MaterialButton MemPatchButton;
        private Separator separator1;
        private Separator separator2;
        public System.Windows.Forms.Label label8;
        private Separator separator3;
        private Separator separator4;
        private System.Windows.Forms.TextBox NetSSID;
        private MaterialButton RestartHostedNetwork;
        private System.Windows.Forms.TextBox NetPass;
        private MaterialButton BatchLinkButton;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.CheckBox AutoConnect;
        private System.Windows.Forms.Timer Heartbeat;
        private System.Windows.Forms.TextBox bScale;
        private System.Windows.Forms.TextBox tScale;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.CheckBox ShowConsole;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RichTextBox hnlogger;
        private CustomLabel customLabel1;
        private CustomLabel logger;
        private CustomLabel logger3;
        private MaterialButton PushButton;
        private MaterialButton PushFileSelectButton;
        private CustomLabel customLabel2;
        public System.Windows.Forms.PictureBox pctSurface;
        private CustomLabel irlog;
        private CustomLabel customLabel3;
    }
}

