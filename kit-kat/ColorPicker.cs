using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace kit_kat
{
	/// <summary>
	/// Summary description for frmColorPicker.
	/// </summary>
	public class ColorPicker : Form
    {
		public ColorPicker(Color starting_color)
		{
			InitializeComponent();

			_rgb = starting_color;
			_hsl = AdobeColors.RGB_to_HSB(_rgb);
			_cmyk = AdobeColors.RGB_to_CMYK(_rgb);

            UpdateTextBoxes();

			m_ctrl_BigBox.HSB = _hsl;
			m_ctrl_ThinBox.HSB = _hsl;

			m_lbl_Primary_Color.BackColor = starting_color;
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

        private AdobeColors.HSB		_hsl;
		private Color				_rgb;
		private AdobeColors.CMYK	_cmyk;

		public Color PrimaryColor
		{
			get
			{
				return _rgb;
			}
			set
			{
				_rgb = value;
				_hsl = AdobeColors.RGB_to_HSB(_rgb);

                UpdateTextBoxes();

				m_ctrl_BigBox.HSB = _hsl;
				m_ctrl_ThinBox.HSB = _hsl;

				m_lbl_Primary_Color.BackColor = _rgb;
			}
		}

		public ColorComponent DrawStyle
		{
			get
			{
		        return ColorComponent.Hue;
			}
		}

		#region Designer Generated Variables
		private System.Windows.Forms.TextBox m_txt_Hex;
		private System.Windows.Forms.Label m_lbl_HexPound;
		private System.Windows.Forms.Label m_lbl_Primary_Color;
		private ctrlVerticalColorSlider m_ctrl_ThinBox;
		private ctrl2DColorBox m_ctrl_BigBox;
        private Panel panel1;
        private ControlBox controlBox1;
        private CustomLabel AppTitle;

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

		#endregion

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            kit_kat.AdobeColors.HSB hsb1 = new kit_kat.AdobeColors.HSB();
            kit_kat.AdobeColors.HSB hsb2 = new kit_kat.AdobeColors.HSB();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ColorPicker));
            this.m_txt_Hex = new System.Windows.Forms.TextBox();
            this.m_lbl_HexPound = new System.Windows.Forms.Label();
            this.m_lbl_Primary_Color = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.controlBox1 = new kit_kat.ControlBox();
            this.AppTitle = new kit_kat.CustomLabel();
            this.m_ctrl_BigBox = new kit_kat.ctrl2DColorBox();
            this.m_ctrl_ThinBox = new kit_kat.ctrlVerticalColorSlider();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // m_txt_Hex
            // 
            this.m_txt_Hex.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.m_txt_Hex.Location = new System.Drawing.Point(307, 40);
            this.m_txt_Hex.Name = "m_txt_Hex";
            this.m_txt_Hex.Size = new System.Drawing.Size(55, 20);
            this.m_txt_Hex.TabIndex = 19;
            this.m_txt_Hex.Leave += new System.EventHandler(this.m_txt_Hex_Leave);
            // 
            // m_lbl_HexPound
            // 
            this.m_lbl_HexPound.Location = new System.Drawing.Point(290, 42);
            this.m_lbl_HexPound.Name = "m_lbl_HexPound";
            this.m_lbl_HexPound.Size = new System.Drawing.Size(19, 15);
            this.m_lbl_HexPound.TabIndex = 27;
            this.m_lbl_HexPound.Text = "#";
            // 
            // m_lbl_Primary_Color
            // 
            this.m_lbl_Primary_Color.BackColor = System.Drawing.Color.White;
            this.m_lbl_Primary_Color.Location = new System.Drawing.Point(288, 5);
            this.m_lbl_Primary_Color.Name = "m_lbl_Primary_Color";
            this.m_lbl_Primary_Color.Size = new System.Drawing.Size(74, 29);
            this.m_lbl_Primary_Color.TabIndex = 36;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.m_ctrl_BigBox);
            this.panel1.Controls.Add(this.m_lbl_Primary_Color);
            this.panel1.Controls.Add(this.m_ctrl_ThinBox);
            this.panel1.Controls.Add(this.m_txt_Hex);
            this.panel1.Controls.Add(this.m_lbl_HexPound);
            this.panel1.Location = new System.Drawing.Point(0, 29);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(367, 254);
            this.panel1.TabIndex = 40;
            // 
            // controlBox1
            // 
            this.controlBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.controlBox1.Font = new System.Drawing.Font("Verdana", 8F);
            this.controlBox1.Location = new System.Drawing.Point(299, 0);
            this.controlBox1.Name = "controlBox1";
            this.controlBox1.Size = new System.Drawing.Size(68, 29);
            this.controlBox1.TabIndex = 41;
            this.controlBox1.Text = "controlBox1";
            // 
            // AppTitle
            // 
            this.AppTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.AppTitle.ForeColor = System.Drawing.Color.White;
            this.AppTitle.Location = new System.Drawing.Point(0, 0);
            this.AppTitle.Name = "AppTitle";
            this.AppTitle.Size = new System.Drawing.Size(144, 29);
            this.AppTitle.TabIndex = 42;
            this.AppTitle.Tag = "Overide";
            this.AppTitle.Text = "kit-kat - Color Picker";
            this.AppTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // m_ctrl_BigBox
            // 
            this.m_ctrl_BigBox.BaseColorComponent = kit_kat.ColorComponent.Hue;
            hsb1.B = 1D;
            hsb1.H = 0D;
            hsb1.S = 1D;
            this.m_ctrl_BigBox.HSB = hsb1;
            this.m_ctrl_BigBox.Location = new System.Drawing.Point(-2, -2);
            this.m_ctrl_BigBox.Name = "m_ctrl_BigBox";
            this.m_ctrl_BigBox.RGB = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.m_ctrl_BigBox.Size = new System.Drawing.Size(256, 256);
            this.m_ctrl_BigBox.TabIndex = 39;
            this.m_ctrl_BigBox.WebSafeColorsOnly = false;
            this.m_ctrl_BigBox.SelectionChanged += new System.EventHandler(this.m_ctrl_BigBox_SelectionChanged);
            // 
            // m_ctrl_ThinBox
            // 
            this.m_ctrl_ThinBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(108)))), ((int)(((byte)(128)))));
            this.m_ctrl_ThinBox.BaseColorComponent = kit_kat.ColorComponent.Hue;
            hsb2.B = 1D;
            hsb2.H = 0D;
            hsb2.S = 1D;
            this.m_ctrl_ThinBox.HSB = hsb2;
            this.m_ctrl_ThinBox.Location = new System.Drawing.Point(244, -4);
            this.m_ctrl_ThinBox.Name = "m_ctrl_ThinBox";
            this.m_ctrl_ThinBox.RGB = System.Drawing.Color.Red;
            this.m_ctrl_ThinBox.Size = new System.Drawing.Size(39, 262);
            this.m_ctrl_ThinBox.TabIndex = 38;
            this.m_ctrl_ThinBox.WebSafeColorsOnly = false;
            this.m_ctrl_ThinBox.SelectionChanged += new System.EventHandler(this.m_ctrl_ThinBox_SelectionChanged);
            // 
            // ColorPicker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(104)))), ((int)(((byte)(118)))), ((int)(((byte)(138)))));
            this.ClientSize = new System.Drawing.Size(367, 283);
            this.Controls.Add(this.controlBox1);
            this.Controls.Add(this.AppTitle);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "ColorPicker";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "kit-kat - Color Picker";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ColorPicker_FormClosed);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.FormDrag);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

		}


		#endregion

		#region Events Handlers
        
		#region Primary Picture Box (m_ctrl_BigBox)

		private void m_ctrl_BigBox_SelectionChanged(object sender, System.EventArgs e)
		{
			_hsl = m_ctrl_BigBox.HSB;
			_rgb = AdobeColors.HSB_to_RGB(_hsl);
			_cmyk = AdobeColors.RGB_to_CMYK(_rgb);

            UpdateTextBoxes();

			m_ctrl_ThinBox.HSB = _hsl;

			m_lbl_Primary_Color.BackColor = _rgb;
			m_lbl_Primary_Color.Update();
		}

		#endregion

		#region Secondary Picture Box (m_ctrl_ThinBox)

		private void m_ctrl_ThinBox_SelectionChanged(object sender, System.EventArgs e)
		{
			_hsl = m_ctrl_ThinBox.HSB;
			_rgb = AdobeColors.HSB_to_RGB(_hsl);
			_cmyk = AdobeColors.RGB_to_CMYK(_rgb);

            UpdateTextBoxes();

			m_ctrl_BigBox.HSB = _hsl;

			m_lbl_Primary_Color.BackColor = _rgb;
			m_lbl_Primary_Color.Update();
		}

		#endregion

		#region Hex Box (m_txt_Hex)

		private void m_txt_Hex_Leave(object sender, System.EventArgs e)
		{
			string text = m_txt_Hex.Text.ToUpper();
			bool has_illegal_chars = false;

			if ( text.Length <= 0 )
				has_illegal_chars = true;
			foreach ( char letter in text )
			{
				if ( !char.IsNumber(letter) )
				{
					if ( letter >= 'A' && letter <= 'F' )
						continue;
					has_illegal_chars = true;
					break;
				}
			}

			if ( has_illegal_chars )
			{
				MessageBox.Show("Hex must be a hex value between 0x000000 and 0xFFFFFF");
				WriteHexData(_rgb);
				return;
			}

			_rgb = ParseHexData(text);
			_hsl = AdobeColors.RGB_to_HSB(_rgb);
			_cmyk = AdobeColors.RGB_to_CMYK(_rgb);

			m_ctrl_BigBox.HSB = _hsl;
			m_ctrl_ThinBox.HSB = _hsl;
			m_lbl_Primary_Color.BackColor = _rgb;

			UpdateTextBoxes();
		}


		#endregion

		#endregion

		#region Private Functions

		private void WriteHexData(Color rgb)
		{
			string red = Convert.ToString(rgb.R, 16);
			if ( red.Length < 2 ) red = "0" + red;
			string green = Convert.ToString(rgb.G, 16);
			if ( green.Length < 2 ) green = "0" + green;
			string blue = Convert.ToString(rgb.B, 16);
			if ( blue.Length < 2 ) blue = "0" + blue;

			m_txt_Hex.Text = red.ToUpper() + green.ToUpper() + blue.ToUpper();
			m_txt_Hex.Update();
		}

		private Color ParseHexData(string hex_data)
		{
            hex_data = "000000" + hex_data;
            hex_data = hex_data.Remove(0, hex_data.Length - 6);

			string r_text, g_text, b_text;
			int r, g, b;

			r_text = hex_data.Substring(0, 2);
			g_text = hex_data.Substring(2, 2);
			b_text = hex_data.Substring(4, 2);

			r = int.Parse(r_text, System.Globalization.NumberStyles.HexNumber);
			g = int.Parse(g_text, System.Globalization.NumberStyles.HexNumber);
			b = int.Parse(b_text, System.Globalization.NumberStyles.HexNumber);

			return Color.FromArgb(r, g, b);
		}

        private void UpdateTextBoxes()
        {
            WriteHexData(_rgb);
        }

		#endregion

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }

            base.Dispose(disposing);
        }

        private void m_txt_Hue_TextChanged(object sender, EventArgs e)
        {

        }

        private void ColorPicker_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }
    }
}
