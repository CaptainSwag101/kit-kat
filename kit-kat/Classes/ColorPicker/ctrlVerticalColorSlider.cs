using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Threading.Tasks;

namespace kit_kat
{
	/// <summary>
	/// A vertical slider control that shows a range for a color property (a.k.a. Hue, Saturation, Brightness,
	/// Red, Green, Blue) and sends an event when the slider is changed.
	/// </summary>
	public class ctrlVerticalColorSlider : UserControl
    {
		public ctrlVerticalColorSlider()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			//	Initialize Colors
			_hsb = new AdobeColors.HSB();
			_hsb.H = 1.0;
			_hsb.S = 1.0;
			_hsb.B = 1.0;
			_rgb = AdobeColors.HSB_to_RGB(_hsb);
			_baseColorComponent = ColorComponent.Hue;
		}

        private ColorComponent _baseColorComponent = ColorComponent.Hue;
		/// <summary>
        /// Gets or sets the base color component which is fixed.
        /// </summary>
		public ColorComponent BaseColorComponent
		{
			get
			{
				return _baseColorComponent;
			}
			set
			{
				_baseColorComponent = value;

				//	Redraw the control based on the new ColorComponent
				ResetSlider(true);
				RedrawAll();
			}
		}

        private AdobeColors.HSB	_hsb;
		/// <summary>
        /// Gets or sets the color in HSB mode. <see cref="RGB"/> property will be accordingly updated.
        /// </summary>
		public AdobeColors.HSB HSB
		{
			get
			{
				return _hsb;
			}
			set
			{
				_hsb = value;
				_rgb = AdobeColors.HSB_to_RGB(_hsb);

				//	Redraw the control based on the new color.
				ResetSlider(true);
				DrawContent();
			}
		}

        private Color _rgb;
		/// <summary>
        /// Gets or sets the color in RGB mode. <see cref="HSB"/> property will be accordingly updated.
        /// </summary>
        public Color RGB
        {
            get
            {
                return _rgb;
            }
            set
            {
                _rgb = value;
                _hsb = AdobeColors.RGB_to_HSB(_rgb);

                //	Redraw the control based on the new color.
                ResetSlider(true);
                DrawContent();
            }
        }

        private bool _webSafeColorsOnly = false;
        /// <summary>
        /// Gets or sets a boolean value that indicates where only the web colors are available.
        /// </summary>
        public bool WebSafeColorsOnly
        {
            get
            {
                return _webSafeColorsOnly;
            }
            set
            {
                _webSafeColorsOnly = value;
                RedrawAll();
            }
        }

        #region Events

        /// <summary>
        /// Occurs when the selected color has been changed.
        /// </summary>
        public event EventHandler SelectionChanged;

        /// <summary>
        /// Raises the <see cref="SelectionChanged"/> event.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnSelectionChanged(EventArgs e)
        {
            if (SelectionChanged != null)
            {
                EventHandler handler = SelectionChanged;
                handler(this, e);
            }
        }

		#endregion

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.SuspendLayout();
            // 
            // ctrlVerticalColorSlider
            // 
            BackColor = Color.FromArgb(104, 118, 138);
            this.Name = "ctrlVerticalColorSlider";
            this.Size = new System.Drawing.Size(40, 264);
            this.ResumeLayout(false);

		}

		#endregion

        #region User Input

        private int _markerStartY = 0;
        private bool _isDragging = false;

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            if (e.Button.HasFlag(MouseButtons.Left))
            {
                _isDragging = true;

                SliderMoved(e.Y);
            }
		}

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (_isDragging)
            {
                SliderMoved(e.Y);
            }
		}

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);

            if (e.Button.HasFlag(MouseButtons.Left))
            {
                _isDragging = false;

                SliderMoved(e.Y);
            }
        }

        private int LimitInRange(int value, int min, int max)
        {
            if (value <= min)
            {
                return min;
            }
            else if (value >= max)
            {
                return max;
            }
            else
            {
                return value;
            }
        }

        private void SliderMoved(int y)
        {
            y -= 4;
            y = LimitInRange(y, 0, this.Height - 9);

            if (y == _markerStartY)
            {
                return;
            }

            DrawSlider(y, false);	//	Redraw the slider
            ResetHSLRGB();			//	Reset the color

            OnSelectionChanged(EventArgs.Empty);
        }

        #endregion

        #region Control Event Overrides

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            RedrawAll();
		}

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            RedrawAll();
		}

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            RedrawAll();
		}

		#endregion

		#region Rendering

		/// <summary>
		/// Redraws the background over the slider area on both sides of the control
		/// </summary>
		private void ClearSlider()
		{
			Graphics g = this.CreateGraphics();

			Brush brush = new SolidBrush(Color.FromArgb(94, 108, 128));
			g.FillRectangle(brush, 0, 0, 8, this.Height);				//	clear left hand slider
			g.FillRectangle(brush, this.Width - 8, 0, 8, this.Height);	//	clear right hand slider
		}

		/// <summary>
		/// Draws the slider arrows on both sides of the control.
		/// </summary>
		/// <param name="position">position value of the slider, lowest being at the bottom.  The range
		/// is between 0 and the controls height-9.  The values will be adjusted if too large/small</param>
		/// <param name="Unconditional">If Unconditional is true, the slider is drawn, otherwise some logic 
		/// is performed to determine is drawing is really neccessary.</param>
        private void DrawSlider(int position, bool force)
        {
            position = LimitInRange(position, 0, this.Height - 9);

            if (_markerStartY == position && !force)
            {
                return;
            }

            // Update the controls marker position.
            _markerStartY = position;	

            // Remove old slider.
            this.ClearSlider();		

            Graphics g = this.CreateGraphics();

            Pen pencil = new Pen(Color.FromArgb(116, 114, 106));
            Brush brush = Brushes.White;

            Point[] arrow = new Point[7];                       //	    GGG
            arrow[0] = new Point(this.Width - 2, position);		//	   G   G
            arrow[1] = new Point(this.Width - 4, position);		//	  G    G
            arrow[2] = new Point(this.Width - 8, position + 4);	//	 G     G
            arrow[3] = new Point(this.Width - 4, position + 8);	//	G      G
            arrow[4] = new Point(this.Width - 2, position + 8);	//	 G     G
            arrow[5] = new Point(this.Width - 1, position + 7);	//	  G    G
            arrow[6] = new Point(this.Width - 1, position + 1);	//	   G   G
            //	    GGG

            g.FillPolygon(brush, arrow);	//	Fill right arrow with white
            g.DrawPolygon(pencil, arrow);	//	Draw right arrow border with gray
        }

        /// <summary>
		/// Draws the border around the control, in this case the border around the content area between
		/// the slider arrows.
		/// </summary>
        private void DrawBorder()
        {
            Graphics g = this.CreateGraphics();

            Pen pencil;

            //	To make the control look like Adobe Photoshop's the border around the control will be a gray line
            //	on the top and left side, a white line on the bottom and right side, and a black rectangle (line) 
            //	inside the gray/white rectangle

            pencil = new Pen(Color.FromArgb(54,54,54));	//	The same gray color used by Photoshop
            g.DrawLine(pencil, 10, 2, 10, this.Height - 5);	//	Draw left hand line
        }

        /// <summary>
        /// Evaluates the DrawStyle of the control and calls the appropriate
        /// drawing function for content
        /// </summary>
        private void DrawContent()
		{
            Rectangle rect = new Rectangle(
                0,
                0,
                this.Width - 21,
                this.Height - 8);
            Bitmap map = GetColorStripBitmap(rect, (ColorComponent)_baseColorComponent);

            Graphics g = this.CreateGraphics();
            g.DrawImageUnscaled(map, 11, 4);

            map.Dispose();
        }

        private Bitmap GetColorStripBitmap(Rectangle rect, ColorComponent comp)
        {
            Bitmap map = new Bitmap(rect.Width, rect.Height, PixelFormat.Format24bppRgb);
            BitmapData mapData = map.LockBits(
                new Rectangle(0, 0, map.Width, map.Height),
                ImageLockMode.WriteOnly,
                PixelFormat.Format24bppRgb);

            int height = this.Height - 8;

            unsafe
            {
                byte* pt0 = (byte*)mapData.Scan0;

                Parallel.For(rect.Top, rect.Bottom, y =>
                {
                    int bitmapY = y - rect.Top;

                    Color color;
                    switch (comp)
                    {
                        case ColorComponent.Hue:
                            color = AdobeColors.HSB_to_RGB(new AdobeColors.HSB(
                                1.0 - (double)y / height,
                                1,
                                1));
                            break;

                        case ColorComponent.Saturation:
                            color = AdobeColors.HSB_to_RGB(new AdobeColors.HSB(
                                _hsb.H,
                                1.0 - (double)y / height,
                                _hsb.B));
                            break;

                        case ColorComponent.Brightness:
                            color = AdobeColors.HSB_to_RGB(new AdobeColors.HSB(
                                _hsb.H,
                                _hsb.S,
                                1.0 - (double)y / height));
                            break;

                        case ColorComponent.Red:
                            int red = 255 - (int)Math.Round(255 * (double)y / height);
                            color = Color.FromArgb(
                                red,
                                _rgb.G,
                                _rgb.B);
                            break;

                        case ColorComponent.Green:
                            int green = 255 - (int)Math.Round(255 * (double)y / height);
                            color = Color.FromArgb(
                                _rgb.R,
                                green,
                                _rgb.B);
                            break;

                        case ColorComponent.Blue:
                            int blue = 255 - (int)Math.Round(255 * (double)y / height);
                            color = Color.FromArgb(
                                _rgb.R,
                                _rgb.G,
                                blue);
                            break;

                        default:
                            throw new ArgumentException();
                    }

                    if (_webSafeColorsOnly)
                    {
                        color = AdobeColors.GetNearestWebSafeColor(color);
                    }

                    for (int x = rect.Left; x < rect.Right; x++)
                    {
                        int bitmapX = x - rect.Left;

                        byte* pt = pt0 + mapData.Stride * bitmapY + 3 * bitmapX;
                        pt[2] = color.R;
                        pt[1] = color.G;
                        pt[0] = color.B;
                    }
                });
            }

            map.UnlockBits(mapData);
            return map;
        }
        
		private void RedrawAll()
		{
			DrawSlider(_markerStartY, true);
            DrawBorder();
            DrawContent();
		}

		/// <summary>
		/// Resets the vertical position of the slider to match the controls color.  Gives the option of redrawing the slider.
		/// </summary>
		/// <param name="redraw">Set to true if you want the function to redraw the slider after determining the best position</param>
        private void ResetSlider(bool redraw)
        {
            int height = this.Height - 8;

            switch (_baseColorComponent)
            {
                case ColorComponent.Hue:
                    _markerStartY = height - (int)Math.Round(height * _hsb.H);
                    break;
                case ColorComponent.Saturation:
                    _markerStartY = height - (int)Math.Round(height * _hsb.S);
                    break;
                case ColorComponent.Brightness:
                    _markerStartY = height - (int)Math.Round(height * _hsb.B);
                    break;
                case ColorComponent.Red:
                    _markerStartY = height - (int)Math.Round(height * _rgb.R / 255.0);
                    break;
                case ColorComponent.Green:
                    _markerStartY = height - (int)Math.Round(height * _rgb.G / 255.0);
                    break;
                case ColorComponent.Blue:
                    _markerStartY = height - (int)Math.Round(height * _rgb.B / 255.0);
                    break;
            }

            if (redraw)
            {
                DrawSlider(_markerStartY, true);
            }
        }


		/// <summary>
		/// Resets the controls color (both HSL and RGB variables) based on the current slider position
		/// </summary>
        private void ResetHSLRGB()
        {
            int height = this.Height - 9;

            switch (_baseColorComponent)
            {
                case ColorComponent.Hue:
                    _hsb.H = 1.0 - (double)_markerStartY / height;
                    _rgb = AdobeColors.HSB_to_RGB(_hsb);
                    break;
                case ColorComponent.Saturation:
                    _hsb.S = 1.0 - (double)_markerStartY / height;
                    _rgb = AdobeColors.HSB_to_RGB(_hsb);
                    break;
                case ColorComponent.Brightness:
                    _hsb.B = 1.0 - (double)_markerStartY / height;
                    _rgb = AdobeColors.HSB_to_RGB(_hsb);
                    break;
                case ColorComponent.Red:
                    _rgb = Color.FromArgb(255 - (int)Math.Round(255 * (double)_markerStartY / height), _rgb.G, _rgb.B);
                    _hsb = AdobeColors.RGB_to_HSB(_rgb);
                    break;
                case ColorComponent.Green:
                    _rgb = Color.FromArgb(_rgb.R, 255 - (int)Math.Round(255 * (double)_markerStartY / height), _rgb.B);
                    _hsb = AdobeColors.RGB_to_HSB(_rgb);
                    break;
                case ColorComponent.Blue:
                    _rgb = Color.FromArgb(_rgb.R, _rgb.G, 255 - (int)Math.Round(255 * (double)_markerStartY / height));
                    _hsb = AdobeColors.RGB_to_HSB(_rgb);
                    break;
            }
        }

		#endregion
	}
}
