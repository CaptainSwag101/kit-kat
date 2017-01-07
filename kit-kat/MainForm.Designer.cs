namespace kit_kat
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.DisconnectTimeout = new System.Windows.Forms.Timer(this.components);
            this.TabFixer = new System.Windows.Forms.Panel();
            this.Heartbeat = new System.Windows.Forms.Timer(this.components);
            this.IPDetecter = new System.Windows.Forms.Timer(this.components);
            this.panel4 = new System.Windows.Forms.Panel();
            this.ipaddress = new System.Windows.Forms.TextBox();
            this.controlBox1 = new kit_kat.ControlBox();
            this.BatchLinkButton = new kit_kat.MaterialButton();
            this.customLabel1 = new kit_kat.CustomLabel();
            this.customTabControl1 = new kit_kat.cTabControl();
            this.CaptureCard = new System.Windows.Forms.TabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.customLabel7 = new kit_kat.CustomLabel();
            this.logger = new kit_kat.CustomLabel();
            this.status1panel = new System.Windows.Forms.Panel();
            this.separator5 = new kit_kat.Separator();
            this.MemPatchButton = new kit_kat.MaterialButton();
            this.ConnectButton = new kit_kat.MaterialButton();
            this.materialButton1 = new kit_kat.MaterialButton();
            this.Push = new System.Windows.Forms.TabPage();
            this.customLabel2 = new kit_kat.CustomLabel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.customLabel6 = new kit_kat.CustomLabel();
            this.logger2 = new kit_kat.CustomLabel();
            this.PushFileSelectButton = new kit_kat.MaterialButton();
            this.status2panel = new System.Windows.Forms.Panel();
            this.separator1 = new kit_kat.Separator();
            this.PushButton = new kit_kat.MaterialButton();
            this.materialButton2 = new kit_kat.MaterialButton();
            this.IR = new System.Windows.Forms.TabPage();
            this.customLabel3 = new kit_kat.CustomLabel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.status3panel = new System.Windows.Forms.Panel();
            this.customLabel5 = new kit_kat.CustomLabel();
            this.pctSurface = new System.Windows.Forms.PictureBox();
            this.customLabel8 = new kit_kat.CustomLabel();
            this.customLabel4 = new kit_kat.CustomLabel();
            this.separator4 = new kit_kat.Separator();
            this.materialButton5 = new kit_kat.MaterialButton();
            this.Hidden4 = new System.Windows.Forms.TabPage();
            this.Hidden5 = new System.Windows.Forms.TabPage();
            this.Hidden6 = new System.Windows.Forms.TabPage();
            this.Hidden7 = new System.Windows.Forms.TabPage();
            this.tabPage8 = new System.Windows.Forms.TabPage();
            this.panel5 = new System.Windows.Forms.Panel();
            this.cTabControl1 = new kit_kat.cTabControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.panel6 = new System.Windows.Forms.Panel();
            this.separator2 = new kit_kat.Separator();
            this.label9 = new System.Windows.Forms.Label();
            this.ShowConsole = new System.Windows.Forms.CheckBox();
            this.Quality = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.PriorityFactor = new System.Windows.Forms.NumericUpDown();
            this.tScale = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.bScale = new System.Windows.Forms.TextBox();
            this.QOSValue = new System.Windows.Forms.NumericUpDown();
            this.AutoConnect = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.ViewMode = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.ScreenPriority = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.customLabel9 = new kit_kat.CustomLabel();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.customLabel10 = new kit_kat.CustomLabel();
            this.panel4.SuspendLayout();
            this.customTabControl1.SuspendLayout();
            this.CaptureCard.SuspendLayout();
            this.panel1.SuspendLayout();
            this.Push.SuspendLayout();
            this.panel2.SuspendLayout();
            this.IR.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pctSurface)).BeginInit();
            this.tabPage8.SuspendLayout();
            this.panel5.SuspendLayout();
            this.cTabControl1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.panel6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Quality)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PriorityFactor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.QOSValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ViewMode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ScreenPriority)).BeginInit();
            this.tabPage1.SuspendLayout();
            this.tabPage3.SuspendLayout();
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
            // IPDetecter
            // 
            this.IPDetecter.Enabled = true;
            this.IPDetecter.Tick += new System.EventHandler(this.IPDetecter_Tick);
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.White;
            this.panel4.Controls.Add(this.ipaddress);
            this.panel4.Location = new System.Drawing.Point(36, 282);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(552, 35);
            this.panel4.TabIndex = 36;
            // 
            // ipaddress
            // 
            this.ipaddress.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.ipaddress.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.ipaddress.Location = new System.Drawing.Point(5, 8);
            this.ipaddress.Name = "ipaddress";
            this.ipaddress.Size = new System.Drawing.Size(546, 19);
            this.ipaddress.TabIndex = 3;
            this.ipaddress.Text = "3DS IP Address";
            this.ipaddress.TextChanged += new System.EventHandler(this.ipaddress_TextChanged);
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
            // BatchLinkButton
            // 
            this.BatchLinkButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(144)))), ((int)(((byte)(202)))), ((int)(((byte)(249)))));
            this.BatchLinkButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BatchLinkButton.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BatchLinkButton.ForeColor = System.Drawing.Color.White;
            this.BatchLinkButton.Location = new System.Drawing.Point(588, 282);
            this.BatchLinkButton.Name = "BatchLinkButton";
            this.BatchLinkButton.Size = new System.Drawing.Size(102, 35);
            this.BatchLinkButton.TabIndex = 35;
            this.BatchLinkButton.Text = "LINK .bat";
            this.BatchLinkButton.UseVisualStyleBackColor = false;
            this.BatchLinkButton.Click += new System.EventHandler(this.BatchLinkButton_Click);
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
            this.customLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // customTabControl1
            // 
            this.customTabControl1.Alignment = System.Windows.Forms.TabAlignment.Left;
            this.customTabControl1.Controls.Add(this.CaptureCard);
            this.customTabControl1.Controls.Add(this.Push);
            this.customTabControl1.Controls.Add(this.IR);
            this.customTabControl1.Controls.Add(this.Hidden4);
            this.customTabControl1.Controls.Add(this.Hidden5);
            this.customTabControl1.Controls.Add(this.Hidden6);
            this.customTabControl1.Controls.Add(this.Hidden7);
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
            // CaptureCard
            // 
            this.CaptureCard.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(244)))), ((int)(((byte)(249)))));
            this.CaptureCard.Controls.Add(this.panel1);
            this.CaptureCard.Font = new System.Drawing.Font("Kozuka Gothic Pro B", 9F);
            this.CaptureCard.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.CaptureCard.Location = new System.Drawing.Point(38, 2);
            this.CaptureCard.Name = "CaptureCard";
            this.CaptureCard.Padding = new System.Windows.Forms.Padding(3);
            this.CaptureCard.Size = new System.Drawing.Size(654, 291);
            this.CaptureCard.TabIndex = 0;
            this.CaptureCard.Text = "tabPage1";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.customLabel7);
            this.panel1.Controls.Add(this.logger);
            this.panel1.Controls.Add(this.status1panel);
            this.panel1.Controls.Add(this.separator5);
            this.panel1.Controls.Add(this.MemPatchButton);
            this.panel1.Controls.Add(this.ConnectButton);
            this.panel1.Controls.Add(this.materialButton1);
            this.panel1.Location = new System.Drawing.Point(0, 19);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(654, 215);
            this.panel1.TabIndex = 7;
            // 
            // customLabel7
            // 
            this.customLabel7.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.customLabel7.ForeColor = System.Drawing.Color.Black;
            this.customLabel7.Location = new System.Drawing.Point(6, 6);
            this.customLabel7.Name = "customLabel7";
            this.customLabel7.Size = new System.Drawing.Size(151, 31);
            this.customLabel7.TabIndex = 23;
            this.customLabel7.Tag = "Left";
            this.customLabel7.Text = "Capture Card\r\nStream your 3DS to PC";
            this.customLabel7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // logger
            // 
            this.logger.ForeColor = System.Drawing.Color.Black;
            this.logger.Location = new System.Drawing.Point(0, 46);
            this.logger.Name = "logger";
            this.logger.Size = new System.Drawing.Size(654, 165);
            this.logger.TabIndex = 6;
            this.logger.Text = "Loading IP Address...";
            // 
            // status1panel
            // 
            this.status1panel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(184)))), ((int)(((byte)(255)))));
            this.status1panel.Location = new System.Drawing.Point(0, 211);
            this.status1panel.Margin = new System.Windows.Forms.Padding(0);
            this.status1panel.Name = "status1panel";
            this.status1panel.Size = new System.Drawing.Size(654, 4);
            this.status1panel.TabIndex = 7;
            // 
            // separator5
            // 
            this.separator5.Location = new System.Drawing.Point(0, 43);
            this.separator5.Name = "separator5";
            this.separator5.Size = new System.Drawing.Size(654, 6);
            this.separator5.TabIndex = 6;
            this.separator5.Text = "separator5";
            // 
            // MemPatchButton
            // 
            this.MemPatchButton.BackColor = System.Drawing.Color.IndianRed;
            this.MemPatchButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.MemPatchButton.Font = new System.Drawing.Font("Segoe UI Symbol", 48F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MemPatchButton.ForeColor = System.Drawing.Color.Black;
            this.MemPatchButton.Location = new System.Drawing.Point(377, 6);
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
            this.ConnectButton.Location = new System.Drawing.Point(246, 6);
            this.ConnectButton.Name = "ConnectButton";
            this.ConnectButton.Size = new System.Drawing.Size(131, 31);
            this.ConnectButton.TabIndex = 4;
            this.ConnectButton.Text = "CONNECT";
            this.ConnectButton.UseVisualStyleBackColor = false;
            this.ConnectButton.Click += new System.EventHandler(this.ConnectButton_Click);
            // 
            // materialButton1
            // 
            this.materialButton1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(144)))), ((int)(((byte)(202)))), ((int)(((byte)(249)))));
            this.materialButton1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.materialButton1.Font = new System.Drawing.Font("Segoe UI Symbol", 48F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.materialButton1.ForeColor = System.Drawing.Color.White;
            this.materialButton1.Location = new System.Drawing.Point(493, 6);
            this.materialButton1.Margin = new System.Windows.Forms.Padding(0);
            this.materialButton1.Name = "materialButton1";
            this.materialButton1.Size = new System.Drawing.Size(154, 31);
            this.materialButton1.TabIndex = 8;
            this.materialButton1.Tag = "";
            this.materialButton1.Text = "Tutorial";
            this.materialButton1.UseVisualStyleBackColor = false;
            this.materialButton1.Click += new System.EventHandler(this.materialButton1_Click);
            // 
            // Push
            // 
            this.Push.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(244)))), ((int)(((byte)(249)))));
            this.Push.Controls.Add(this.customLabel2);
            this.Push.Controls.Add(this.panel2);
            this.Push.Font = new System.Drawing.Font("Kozuka Gothic Pro B", 9F);
            this.Push.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.Push.Location = new System.Drawing.Point(38, 2);
            this.Push.Name = "Push";
            this.Push.Padding = new System.Windows.Forms.Padding(3);
            this.Push.Size = new System.Drawing.Size(654, 291);
            this.Push.TabIndex = 2;
            this.Push.Tag = "";
            this.Push.Text = "tabPage2";
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
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.White;
            this.panel2.Controls.Add(this.customLabel6);
            this.panel2.Controls.Add(this.logger2);
            this.panel2.Controls.Add(this.PushFileSelectButton);
            this.panel2.Controls.Add(this.status2panel);
            this.panel2.Controls.Add(this.separator1);
            this.panel2.Controls.Add(this.PushButton);
            this.panel2.Controls.Add(this.materialButton2);
            this.panel2.Location = new System.Drawing.Point(0, 19);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(654, 215);
            this.panel2.TabIndex = 13;
            // 
            // customLabel6
            // 
            this.customLabel6.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.customLabel6.ForeColor = System.Drawing.Color.Black;
            this.customLabel6.Location = new System.Drawing.Point(6, 6);
            this.customLabel6.Name = "customLabel6";
            this.customLabel6.Size = new System.Drawing.Size(186, 31);
            this.customLabel6.TabIndex = 23;
            this.customLabel6.Tag = "Left";
            this.customLabel6.Text = "Push\r\nWireless .cia and .tik installer";
            this.customLabel6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // logger2
            // 
            this.logger2.ForeColor = System.Drawing.Color.Black;
            this.logger2.Location = new System.Drawing.Point(0, 46);
            this.logger2.Name = "logger2";
            this.logger2.Size = new System.Drawing.Size(654, 165);
            this.logger2.TabIndex = 9;
            this.logger2.Text = "Loading IP Address...";
            // 
            // PushFileSelectButton
            // 
            this.PushFileSelectButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(182)))), ((int)(((byte)(218)))), ((int)(((byte)(248)))));
            this.PushFileSelectButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.PushFileSelectButton.Font = new System.Drawing.Font("Segoe UI Symbol", 48F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PushFileSelectButton.ForeColor = System.Drawing.Color.White;
            this.PushFileSelectButton.Location = new System.Drawing.Point(377, 6);
            this.PushFileSelectButton.Margin = new System.Windows.Forms.Padding(0);
            this.PushFileSelectButton.Name = "PushFileSelectButton";
            this.PushFileSelectButton.Size = new System.Drawing.Size(31, 31);
            this.PushFileSelectButton.TabIndex = 11;
            this.PushFileSelectButton.Tag = "BrightBlue";
            this.PushFileSelectButton.Text = "+";
            this.PushFileSelectButton.UseVisualStyleBackColor = false;
            this.PushFileSelectButton.Click += new System.EventHandler(this.PushFileSelectButton_Click);
            // 
            // status2panel
            // 
            this.status2panel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(184)))), ((int)(((byte)(255)))));
            this.status2panel.Location = new System.Drawing.Point(0, 211);
            this.status2panel.Margin = new System.Windows.Forms.Padding(0);
            this.status2panel.Name = "status2panel";
            this.status2panel.Size = new System.Drawing.Size(654, 4);
            this.status2panel.TabIndex = 7;
            // 
            // separator1
            // 
            this.separator1.Location = new System.Drawing.Point(0, 43);
            this.separator1.Name = "separator1";
            this.separator1.Size = new System.Drawing.Size(654, 6);
            this.separator1.TabIndex = 6;
            this.separator1.Text = "separator1";
            // 
            // PushButton
            // 
            this.PushButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(144)))), ((int)(((byte)(202)))), ((int)(((byte)(249)))));
            this.PushButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.PushButton.Enabled = false;
            this.PushButton.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PushButton.ForeColor = System.Drawing.Color.White;
            this.PushButton.Location = new System.Drawing.Point(246, 6);
            this.PushButton.Name = "PushButton";
            this.PushButton.Size = new System.Drawing.Size(131, 31);
            this.PushButton.TabIndex = 7;
            this.PushButton.Text = "PUSH";
            this.PushButton.UseVisualStyleBackColor = false;
            this.PushButton.Click += new System.EventHandler(this.PushButton_Click);
            // 
            // materialButton2
            // 
            this.materialButton2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(144)))), ((int)(((byte)(202)))), ((int)(((byte)(249)))));
            this.materialButton2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.materialButton2.Font = new System.Drawing.Font("Segoe UI Symbol", 48F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.materialButton2.ForeColor = System.Drawing.Color.White;
            this.materialButton2.Location = new System.Drawing.Point(493, 6);
            this.materialButton2.Margin = new System.Windows.Forms.Padding(0);
            this.materialButton2.Name = "materialButton2";
            this.materialButton2.Size = new System.Drawing.Size(154, 31);
            this.materialButton2.TabIndex = 8;
            this.materialButton2.Tag = "";
            this.materialButton2.Text = "Tutorial";
            this.materialButton2.UseVisualStyleBackColor = false;
            this.materialButton2.Click += new System.EventHandler(this.materialButton2_Click);
            // 
            // IR
            // 
            this.IR.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(244)))), ((int)(((byte)(249)))));
            this.IR.Controls.Add(this.customLabel3);
            this.IR.Controls.Add(this.panel3);
            this.IR.Font = new System.Drawing.Font("Kozuka Gothic Pro B", 9F);
            this.IR.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.IR.Location = new System.Drawing.Point(38, 2);
            this.IR.Name = "IR";
            this.IR.Padding = new System.Windows.Forms.Padding(3);
            this.IR.Size = new System.Drawing.Size(654, 291);
            this.IR.TabIndex = 3;
            this.IR.Tag = "";
            this.IR.Text = "tabPage3";
            // 
            // customLabel3
            // 
            this.customLabel3.Font = new System.Drawing.Font("Kozuka Gothic Pro B", 7F);
            this.customLabel3.ForeColor = System.Drawing.Color.Black;
            this.customLabel3.Location = new System.Drawing.Point(0, 277);
            this.customLabel3.Name = "customLabel3";
            this.customLabel3.Size = new System.Drawing.Size(654, 14);
            this.customLabel3.TabIndex = 24;
            this.customLabel3.Text = "credit: kazo";
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.White;
            this.panel3.Controls.Add(this.status3panel);
            this.panel3.Controls.Add(this.customLabel5);
            this.panel3.Controls.Add(this.pctSurface);
            this.panel3.Controls.Add(this.customLabel8);
            this.panel3.Controls.Add(this.customLabel4);
            this.panel3.Controls.Add(this.separator4);
            this.panel3.Controls.Add(this.materialButton5);
            this.panel3.Location = new System.Drawing.Point(0, 5);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(654, 244);
            this.panel3.TabIndex = 27;
            // 
            // status3panel
            // 
            this.status3panel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(184)))), ((int)(((byte)(255)))));
            this.status3panel.Location = new System.Drawing.Point(0, 240);
            this.status3panel.Name = "status3panel";
            this.status3panel.Size = new System.Drawing.Size(654, 4);
            this.status3panel.TabIndex = 7;
            // 
            // customLabel5
            // 
            this.customLabel5.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.customLabel5.ForeColor = System.Drawing.Color.Black;
            this.customLabel5.Location = new System.Drawing.Point(498, 45);
            this.customLabel5.Margin = new System.Windows.Forms.Padding(0);
            this.customLabel5.Name = "customLabel5";
            this.customLabel5.Size = new System.Drawing.Size(145, 195);
            this.customLabel5.TabIndex = 26;
            this.customLabel5.Text = "Usage:\r\nF1 - IP Address\r\nF2 - Keyboard Controls\r\nF3 - Gamepad Controls\r\nF4 - Debu" +
    "g Mode\r\n\r\nMake sure you enter the IP Address before trying to use touch.";
            // 
            // pctSurface
            // 
            this.pctSurface.BackColor = System.Drawing.Color.Gainsboro;
            this.pctSurface.Location = new System.Drawing.Point(167, 0);
            this.pctSurface.Margin = new System.Windows.Forms.Padding(0);
            this.pctSurface.Name = "pctSurface";
            this.pctSurface.Size = new System.Drawing.Size(320, 240);
            this.pctSurface.TabIndex = 10;
            this.pctSurface.TabStop = false;
            this.pctSurface.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pctSurface_MouseDown);
            this.pctSurface.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pctSurface_MouseMove);
            this.pctSurface.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pctSurface_MouseUp);
            // 
            // customLabel8
            // 
            this.customLabel8.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.customLabel8.ForeColor = System.Drawing.Color.Black;
            this.customLabel8.Location = new System.Drawing.Point(6, 7);
            this.customLabel8.Name = "customLabel8";
            this.customLabel8.Size = new System.Drawing.Size(158, 31);
            this.customLabel8.TabIndex = 23;
            this.customLabel8.Tag = "Left";
            this.customLabel8.Text = "IR\r\n3DS Input Redirector";
            this.customLabel8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // customLabel4
            // 
            this.customLabel4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.customLabel4.ForeColor = System.Drawing.Color.Black;
            this.customLabel4.Location = new System.Drawing.Point(6, 45);
            this.customLabel4.Margin = new System.Windows.Forms.Padding(0);
            this.customLabel4.Name = "customLabel4";
            this.customLabel4.Size = new System.Drawing.Size(154, 195);
            this.customLabel4.TabIndex = 25;
            this.customLabel4.Text = "Installation:\r\nClick here to download the Input Redirection CIA file and install " +
    "it using FBI.";
            this.customLabel4.Click += new System.EventHandler(this.customLabel4_Click);
            // 
            // separator4
            // 
            this.separator4.Location = new System.Drawing.Point(0, 44);
            this.separator4.Name = "separator4";
            this.separator4.Size = new System.Drawing.Size(654, 6);
            this.separator4.TabIndex = 6;
            this.separator4.Text = "separator4";
            // 
            // materialButton5
            // 
            this.materialButton5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(144)))), ((int)(((byte)(202)))), ((int)(((byte)(249)))));
            this.materialButton5.Cursor = System.Windows.Forms.Cursors.Hand;
            this.materialButton5.Font = new System.Drawing.Font("Segoe UI Symbol", 48F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.materialButton5.ForeColor = System.Drawing.Color.White;
            this.materialButton5.Location = new System.Drawing.Point(493, 7);
            this.materialButton5.Margin = new System.Windows.Forms.Padding(0);
            this.materialButton5.Name = "materialButton5";
            this.materialButton5.Size = new System.Drawing.Size(154, 31);
            this.materialButton5.TabIndex = 8;
            this.materialButton5.Tag = "";
            this.materialButton5.Text = "Tutorial";
            this.materialButton5.UseVisualStyleBackColor = false;
            this.materialButton5.Click += new System.EventHandler(this.materialButton5_Click);
            // 
            // Hidden4
            // 
            this.Hidden4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(244)))), ((int)(((byte)(249)))));
            this.Hidden4.Font = new System.Drawing.Font("Kozuka Gothic Pro B", 9F);
            this.Hidden4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.Hidden4.Location = new System.Drawing.Point(38, 2);
            this.Hidden4.Name = "Hidden4";
            this.Hidden4.Size = new System.Drawing.Size(654, 291);
            this.Hidden4.TabIndex = 9;
            this.Hidden4.Tag = "Hidden";
            this.Hidden4.Text = "Hidden4";
            // 
            // Hidden5
            // 
            this.Hidden5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(244)))), ((int)(((byte)(249)))));
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
            this.Hidden6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(244)))), ((int)(((byte)(249)))));
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
            // Hidden7
            // 
            this.Hidden7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(244)))), ((int)(((byte)(249)))));
            this.Hidden7.Font = new System.Drawing.Font("Kozuka Gothic Pro B", 9F);
            this.Hidden7.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.Hidden7.Location = new System.Drawing.Point(38, 2);
            this.Hidden7.Name = "Hidden7";
            this.Hidden7.Size = new System.Drawing.Size(654, 291);
            this.Hidden7.TabIndex = 8;
            this.Hidden7.Tag = "Hidden";
            this.Hidden7.Text = "Hidden7";
            // 
            // tabPage8
            // 
            this.tabPage8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(244)))), ((int)(((byte)(249)))));
            this.tabPage8.Controls.Add(this.panel5);
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
            // panel5
            // 
            this.panel5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(98)))), ((int)(((byte)(114)))));
            this.panel5.Controls.Add(this.cTabControl1);
            this.panel5.Location = new System.Drawing.Point(0, 0);
            this.panel5.Margin = new System.Windows.Forms.Padding(0);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(654, 288);
            this.panel5.TabIndex = 1;
            // 
            // cTabControl1
            // 
            this.cTabControl1.Alignment = System.Windows.Forms.TabAlignment.Left;
            this.cTabControl1.Controls.Add(this.tabPage2);
            this.cTabControl1.Controls.Add(this.tabPage1);
            this.cTabControl1.Controls.Add(this.tabPage3);
            this.cTabControl1.ItemSize = new System.Drawing.Size(36, 127);
            this.cTabControl1.Location = new System.Drawing.Point(-2, -2);
            this.cTabControl1.Multiline = true;
            this.cTabControl1.Name = "cTabControl1";
            this.cTabControl1.SelectedIndex = 0;
            this.cTabControl1.Size = new System.Drawing.Size(656, 290);
            this.cTabControl1.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.cTabControl1.TabIndex = 0;
            this.cTabControl1.Tag = "TextOnly";
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(244)))), ((int)(((byte)(249)))));
            this.tabPage2.Controls.Add(this.panel6);
            this.tabPage2.Font = new System.Drawing.Font("Kozuka Gothic Pro B", 9F);
            this.tabPage2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.tabPage2.Location = new System.Drawing.Point(127, 2);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(529, 288);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Viewer";
            // 
            // panel6
            // 
            this.panel6.BackColor = System.Drawing.Color.White;
            this.panel6.Controls.Add(this.separator2);
            this.panel6.Controls.Add(this.label9);
            this.panel6.Controls.Add(this.ShowConsole);
            this.panel6.Controls.Add(this.Quality);
            this.panel6.Controls.Add(this.label1);
            this.panel6.Controls.Add(this.PriorityFactor);
            this.panel6.Controls.Add(this.tScale);
            this.panel6.Controls.Add(this.label6);
            this.panel6.Controls.Add(this.label10);
            this.panel6.Controls.Add(this.label5);
            this.panel6.Controls.Add(this.bScale);
            this.panel6.Controls.Add(this.QOSValue);
            this.panel6.Controls.Add(this.AutoConnect);
            this.panel6.Controls.Add(this.label3);
            this.panel6.Controls.Add(this.ViewMode);
            this.panel6.Controls.Add(this.label4);
            this.panel6.Controls.Add(this.ScreenPriority);
            this.panel6.Controls.Add(this.label7);
            this.panel6.Controls.Add(this.label2);
            this.panel6.Location = new System.Drawing.Point(0, 8);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(529, 238);
            this.panel6.TabIndex = 77;
            // 
            // separator2
            // 
            this.separator2.Location = new System.Drawing.Point(13, 130);
            this.separator2.Name = "separator2";
            this.separator2.Size = new System.Drawing.Size(508, 6);
            this.separator2.TabIndex = 77;
            this.separator2.Text = "separator2";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.ForeColor = System.Drawing.Color.Black;
            this.label9.Location = new System.Drawing.Point(10, 10);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(145, 17);
            this.label9.TabIndex = 70;
            this.label9.Text = "Auto-Connect on Launch";
            // 
            // ShowConsole
            // 
            this.ShowConsole.AutoSize = true;
            this.ShowConsole.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.ShowConsole.Location = new System.Drawing.Point(508, 140);
            this.ShowConsole.Name = "ShowConsole";
            this.ShowConsole.Size = new System.Drawing.Size(15, 14);
            this.ShowConsole.TabIndex = 76;
            this.ShowConsole.UseVisualStyleBackColor = true;
            this.ShowConsole.CheckedChanged += new System.EventHandler(this.ShowConsole_CheckedChanged);
            // 
            // Quality
            // 
            this.Quality.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.Quality.Location = new System.Drawing.Point(473, 103);
            this.Quality.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.Quality.Name = "Quality";
            this.Quality.Size = new System.Drawing.Size(48, 21);
            this.Quality.TabIndex = 68;
            this.Quality.Value = new decimal(new int[] {
            90,
            0,
            0,
            0});
            this.Quality.ValueChanged += new System.EventHandler(this.Quality_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(10, 139);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(151, 17);
            this.label1.TabIndex = 75;
            this.label1.Text = "Show NTRViewer Console";
            // 
            // PriorityFactor
            // 
            this.PriorityFactor.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.PriorityFactor.Location = new System.Drawing.Point(473, 160);
            this.PriorityFactor.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.PriorityFactor.Name = "PriorityFactor";
            this.PriorityFactor.Size = new System.Drawing.Size(48, 21);
            this.PriorityFactor.TabIndex = 64;
            this.PriorityFactor.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.PriorityFactor.ValueChanged += new System.EventHandler(this.PriorityFactor_ValueChanged);
            // 
            // tScale
            // 
            this.tScale.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tScale.Location = new System.Drawing.Point(473, 30);
            this.tScale.Name = "tScale";
            this.tScale.Size = new System.Drawing.Size(48, 21);
            this.tScale.TabIndex = 74;
            this.tScale.Text = "1.0";
            this.tScale.TextChanged += new System.EventHandler(this.tScale_TextChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.ForeColor = System.Drawing.Color.Black;
            this.label6.Location = new System.Drawing.Point(10, 211);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(214, 17);
            this.label6.TabIndex = 63;
            this.label6.Text = "Screen Priority (1 = Top, 0 = Bottom)";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.ForeColor = System.Drawing.Color.Black;
            this.label10.Location = new System.Drawing.Point(10, 34);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(183, 17);
            this.label10.TabIndex = 73;
            this.label10.Text = "Top Screen Scale (0 = Disabled)";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.Color.Black;
            this.label5.Location = new System.Drawing.Point(9, 187);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(141, 17);
            this.label5.TabIndex = 62;
            this.label5.Text = "Quality of Service Value";
            // 
            // bScale
            // 
            this.bScale.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.bScale.Location = new System.Drawing.Point(473, 54);
            this.bScale.Name = "bScale";
            this.bScale.Size = new System.Drawing.Size(48, 21);
            this.bScale.TabIndex = 72;
            this.bScale.Text = "1.0";
            this.bScale.TextChanged += new System.EventHandler(this.bScale_TextChanged);
            // 
            // QOSValue
            // 
            this.QOSValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.QOSValue.Location = new System.Drawing.Point(473, 184);
            this.QOSValue.Maximum = new decimal(new int[] {
            101,
            0,
            0,
            0});
            this.QOSValue.Name = "QOSValue";
            this.QOSValue.Size = new System.Drawing.Size(48, 21);
            this.QOSValue.TabIndex = 65;
            this.QOSValue.Value = new decimal(new int[] {
            101,
            0,
            0,
            0});
            this.QOSValue.ValueChanged += new System.EventHandler(this.QOSValue_ValueChanged);
            // 
            // AutoConnect
            // 
            this.AutoConnect.AutoSize = true;
            this.AutoConnect.Location = new System.Drawing.Point(508, 11);
            this.AutoConnect.Name = "AutoConnect";
            this.AutoConnect.Size = new System.Drawing.Size(15, 14);
            this.AutoConnect.TabIndex = 71;
            this.AutoConnect.UseVisualStyleBackColor = true;
            this.AutoConnect.CheckedChanged += new System.EventHandler(this.AutoConnect_CheckedChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Location = new System.Drawing.Point(10, 82);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(231, 17);
            this.label3.TabIndex = 59;
            this.label3.Text = "View Mode (1 = Vertical, 0 = Horizontal)";
            // 
            // ViewMode
            // 
            this.ViewMode.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.ViewMode.Location = new System.Drawing.Point(473, 79);
            this.ViewMode.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.ViewMode.Name = "ViewMode";
            this.ViewMode.Size = new System.Drawing.Size(48, 21);
            this.ViewMode.TabIndex = 60;
            this.ViewMode.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.ViewMode.ValueChanged += new System.EventHandler(this.ViewMode_ValueChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.Color.Black;
            this.label4.Location = new System.Drawing.Point(9, 163);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(88, 17);
            this.label4.TabIndex = 61;
            this.label4.Text = "Priority Factor";
            // 
            // ScreenPriority
            // 
            this.ScreenPriority.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.ScreenPriority.Location = new System.Drawing.Point(473, 208);
            this.ScreenPriority.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.ScreenPriority.Name = "ScreenPriority";
            this.ScreenPriority.Size = new System.Drawing.Size(48, 21);
            this.ScreenPriority.TabIndex = 66;
            this.ScreenPriority.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.ScreenPriority.ValueChanged += new System.EventHandler(this.ScreenPriority_ValueChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.ForeColor = System.Drawing.Color.Black;
            this.label7.Location = new System.Drawing.Point(10, 106);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(48, 17);
            this.label7.TabIndex = 67;
            this.label7.Text = "Quality";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(10, 58);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(205, 17);
            this.label2.TabIndex = 58;
            this.label2.Text = "Bottom Screen Scale (0 = Disabled)";
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(244)))), ((int)(((byte)(249)))));
            this.tabPage1.Controls.Add(this.customLabel9);
            this.tabPage1.Font = new System.Drawing.Font("Kozuka Gothic Pro B", 9F);
            this.tabPage1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.tabPage1.Location = new System.Drawing.Point(127, 2);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(529, 288);
            this.tabPage1.TabIndex = 2;
            this.tabPage1.Text = "Push";
            // 
            // customLabel9
            // 
            this.customLabel9.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.customLabel9.ForeColor = System.Drawing.Color.Silver;
            this.customLabel9.Location = new System.Drawing.Point(0, 0);
            this.customLabel9.Name = "customLabel9";
            this.customLabel9.Size = new System.Drawing.Size(529, 253);
            this.customLabel9.TabIndex = 37;
            this.customLabel9.Text = "No configurable settings...";
            this.customLabel9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tabPage3
            // 
            this.tabPage3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(244)))), ((int)(((byte)(249)))));
            this.tabPage3.Controls.Add(this.customLabel10);
            this.tabPage3.Font = new System.Drawing.Font("Kozuka Gothic Pro B", 9F);
            this.tabPage3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.tabPage3.Location = new System.Drawing.Point(127, 2);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(529, 288);
            this.tabPage3.TabIndex = 3;
            this.tabPage3.Text = "Input Redirecter";
            // 
            // customLabel10
            // 
            this.customLabel10.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.customLabel10.ForeColor = System.Drawing.Color.Silver;
            this.customLabel10.Location = new System.Drawing.Point(0, 0);
            this.customLabel10.Name = "customLabel10";
            this.customLabel10.Size = new System.Drawing.Size(529, 253);
            this.customLabel10.TabIndex = 38;
            this.customLabel10.Text = "No configurable settings...";
            this.customLabel10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(104)))), ((int)(((byte)(118)))), ((int)(((byte)(138)))));
            this.ClientSize = new System.Drawing.Size(690, 317);
            this.Controls.Add(this.controlBox1);
            this.Controls.Add(this.BatchLinkButton);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.customLabel1);
            this.Controls.Add(this.TabFixer);
            this.Controls.Add(this.customTabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(690, 317);
            this.MinimumSize = new System.Drawing.Size(690, 317);
            this.Name = "MainForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "kit-kat";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.FormDrag);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.customTabControl1.ResumeLayout(false);
            this.CaptureCard.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.Push.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.IR.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pctSurface)).EndInit();
            this.tabPage8.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.cTabControl1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Quality)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PriorityFactor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.QOSValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ViewMode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ScreenPriority)).EndInit();
            this.tabPage1.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Timer DisconnectTimeout;
        private ControlBox controlBox1;
        private System.Windows.Forms.Panel TabFixer;
        private System.Windows.Forms.Timer Heartbeat;
        private CustomLabel customLabel1;
        private System.Windows.Forms.Timer IPDetecter;
        private System.Windows.Forms.TabPage tabPage8;
        private System.Windows.Forms.Panel panel5;
        private cTabControl cTabControl1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.CheckBox ShowConsole;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tScale;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox bScale;
        private System.Windows.Forms.CheckBox AutoConnect;
        private System.Windows.Forms.NumericUpDown ViewMode;
        private System.Windows.Forms.NumericUpDown ScreenPriority;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown QOSValue;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown PriorityFactor;
        private System.Windows.Forms.NumericUpDown Quality;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TabPage Hidden7;
        private System.Windows.Forms.TabPage Hidden6;
        private System.Windows.Forms.TabPage Hidden5;
        private System.Windows.Forms.TabPage IR;
        private CustomLabel customLabel3;
        private System.Windows.Forms.Panel panel3;
        private CustomLabel customLabel5;
        public System.Windows.Forms.PictureBox pctSurface;
        private CustomLabel customLabel8;
        private CustomLabel customLabel4;
        private System.Windows.Forms.Panel status3panel;
        private Separator separator4;
        private MaterialButton materialButton5;
        private System.Windows.Forms.TabPage Push;
        private CustomLabel customLabel2;
        private System.Windows.Forms.Panel panel2;
        private CustomLabel customLabel6;
        private CustomLabel logger2;
        private MaterialButton PushFileSelectButton;
        private System.Windows.Forms.Panel status2panel;
        private Separator separator1;
        private MaterialButton PushButton;
        private MaterialButton materialButton2;
        private System.Windows.Forms.TabPage CaptureCard;
        private System.Windows.Forms.Panel panel1;
        private CustomLabel customLabel7;
        private CustomLabel logger;
        private System.Windows.Forms.Panel status1panel;
        private Separator separator5;
        private MaterialButton MemPatchButton;
        private MaterialButton ConnectButton;
        private MaterialButton materialButton1;
        private MaterialButton BatchLinkButton;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.TextBox ipaddress;
        private cTabControl customTabControl1;
        private System.Windows.Forms.TabPage Hidden4;
        private System.Windows.Forms.Panel panel6;
        private Separator separator2;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage3;
        private CustomLabel customLabel9;
        private CustomLabel customLabel10;
    }
}

