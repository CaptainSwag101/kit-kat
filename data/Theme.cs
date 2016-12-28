// LICENCE: This file is NOT to be used ANYWHERE! This cannot be shared, modified or used without my consent! Want to use it? Ask me on twitter: @initPRAGMA
// Created with HUGE <3 by initPRAGMA.

using kitkat.Properties;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using static Helpers;

#region Helpers
static internal class Helpers
{

    #region ColorFromHex
    public static Color ColorFromHex(string Hex)
    {
        return Color.FromArgb(Convert.ToInt32(long.Parse(string.Format("FFFFFFFFFF{0}", Hex.Substring(1)), System.Globalization.NumberStyles.HexNumber)));
    }
    #endregion
    #region ColorContrast
    public static Color ColorContrast(Color color__1)
    {
        
        double a = 1 - (0.299 * color__1.R + 0.587 * color__1.G + 0.114 * color__1.B) / 255;

        if (a < 0.5)
        {
            return Color.White;
        }
        else
        {
            return Color.Black;
        }

    }
    #endregion
    #region GetTabIcon
    public static Image GetTabIcon(int i)
    {
        if (i == 0)
        {
            return Resources.Monitor;
        }
        else if (i == 1)
        {
            return Resources.WiFi;
        }
        else if (i == 2)
        {
            return Resources.Upload;
        }
        else if (i == 3)
        {
            return Resources.Controller;
        }
        else
        {
            return Resources.Settings;
        }
    }
    #endregion
    #region RoundRect
    public enum RoundingStyle : byte
    {
        All = 0,
        Top = 1,
        Bottom = 2,
        Left = 3,
        Right = 4,
        TopRight = 5,
        BottomRight = 6
    }
    public static GraphicsPath RoundRect(Rectangle Rect, int Rounding, RoundingStyle Style = RoundingStyle.All)
    {

        GraphicsPath GP = new GraphicsPath();
        int AW = Rounding * 2;

        GP.StartFigure();

        if (Rounding == 0)
        {
            GP.AddRectangle(Rect);
            GP.CloseAllFigures();
            return GP;
        }

        switch (Style)
        {
            case RoundingStyle.All:
                GP.AddArc(new Rectangle(Rect.X, Rect.Y, AW, AW), -180, 90);
                GP.AddArc(new Rectangle(Rect.Width - AW + Rect.X, Rect.Y, AW, AW), -90, 90);
                GP.AddArc(new Rectangle(Rect.Width - AW + Rect.X, Rect.Height - AW + Rect.Y, AW, AW), 0, 90);
                GP.AddArc(new Rectangle(Rect.X, Rect.Height - AW + Rect.Y, AW, AW), 90, 90);
                break;
            case RoundingStyle.Top:
                GP.AddArc(new Rectangle(Rect.X, Rect.Y, AW, AW), -180, 90);
                GP.AddArc(new Rectangle(Rect.Width - AW + Rect.X, Rect.Y, AW, AW), -90, 90);
                GP.AddLine(new Point(Rect.X + Rect.Width, Rect.Y + Rect.Height), new Point(Rect.X, Rect.Y + Rect.Height));
                break;
            case RoundingStyle.Bottom:
                GP.AddLine(new Point(Rect.X, Rect.Y), new Point(Rect.X + Rect.Width, Rect.Y));
                GP.AddArc(new Rectangle(Rect.Width - AW + Rect.X, Rect.Height - AW + Rect.Y, AW, AW), 0, 90);
                GP.AddArc(new Rectangle(Rect.X, Rect.Height - AW + Rect.Y, AW, AW), 90, 90);
                break;
            case RoundingStyle.Left:
                GP.AddArc(new Rectangle(Rect.X, Rect.Y, AW, AW), -180, 90);
                GP.AddLine(new Point(Rect.X + Rect.Width, Rect.Y), new Point(Rect.X + Rect.Width, Rect.Y + Rect.Height));
                GP.AddArc(new Rectangle(Rect.X, Rect.Height - AW + Rect.Y, AW, AW), 90, 90);
                break;
            case RoundingStyle.Right:
                GP.AddLine(new Point(Rect.X, Rect.Y + Rect.Height), new Point(Rect.X, Rect.Y));
                GP.AddArc(new Rectangle(Rect.Width - AW + Rect.X, Rect.Y, AW, AW), -90, 90);
                GP.AddArc(new Rectangle(Rect.Width - AW + Rect.X, Rect.Height - AW + Rect.Y, AW, AW), 0, 90);
                break;
            case RoundingStyle.TopRight:
                GP.AddLine(new Point(Rect.X, Rect.Y + 1), new Point(Rect.X, Rect.Y));
                GP.AddArc(new Rectangle(Rect.Width - AW + Rect.X, Rect.Y, AW, AW), -90, 90);
                GP.AddLine(new Point(Rect.X + Rect.Width, Rect.Y + Rect.Height - 1), new Point(Rect.X + Rect.Width, Rect.Y + Rect.Height));
                GP.AddLine(new Point(Rect.X + 1, Rect.Y + Rect.Height), new Point(Rect.X, Rect.Y + Rect.Height));
                break;
            case RoundingStyle.BottomRight:
                GP.AddLine(new Point(Rect.X, Rect.Y + 1), new Point(Rect.X, Rect.Y));
                GP.AddLine(new Point(Rect.X + Rect.Width - 1, Rect.Y), new Point(Rect.X + Rect.Width, Rect.Y));
                GP.AddArc(new Rectangle(Rect.Width - AW + Rect.X, Rect.Height - AW + Rect.Y, AW, AW), 0, 90);
                GP.AddLine(new Point(Rect.X + 1, Rect.Y + Rect.Height), new Point(Rect.X, Rect.Y + Rect.Height));
                break;
        }

        GP.CloseAllFigures();

        return GP;

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

}
#endregion

namespace kitkat
{
    
    #region ControlBox

    class ControlBox : Control
    {

        //Variables;
        public MouseState s;
        Bitmap minbmp = new Bitmap(68, 29);
        Bitmap closebmp = new Bitmap(68, 29);
        dynamic x = -1000; //Mouse X-Position;
        public int l = 2; //Mouse Hover State;

        //Default Settings;
        public ControlBox()
        {

            //Graphical Style Settings;
            SetStyle((ControlStyles)139270 | ControlStyles.ResizeRedraw | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint | ControlStyles.SupportsTransparentBackColor, true);
            Graphics MeasureGraphics = default(Graphics);
            MeasureGraphics = Graphics.FromImage(new Bitmap(1, 1));
            dynamic DrawRadialPath = new GraphicsPath();

            //Default Settings;
            DoubleBuffered = true;
            Anchor = AnchorStyles.Top | AnchorStyles.Right;
            Width = 68;
            Height = 29;
            Font = new Font("Verdana", 8);
            
        }
        
        //Make sure it's always above everything;
        protected override sealed void OnHandleCreated(EventArgs e)
        {
            
            //Make sure it's ALWAYS above everything;
            BringToFront();

            //Raise flag;
            base.OnHandleCreated(e);

        }

        //Render the Icon BMP's;
        protected override void OnCreateControl()
        {

            //Raise flag;
            base.OnCreateControl();

            //Set the Graphics Variable;
            Graphics G = default(Graphics);

            #region Minimize Icon

            //Make a Graphics Variable from the Minimize BMP;
            G = Graphics.FromImage(minbmp);

            //Quality Settings;
            G.CompositingQuality = CompositingQuality.HighQuality;
            G.InterpolationMode = InterpolationMode.HighQualityBicubic;

            //Draw the Dropshadow into the BMP;
            G.DrawLine(new Pen(Color.FromArgb(68, Color.Black)), 11, 20, 21, 20);
            G.DrawLine(new Pen(Color.FromArgb(40, Color.Black)), 11, 17, 21, 17);

            //Draw the Icon into the BMP;
            G.DrawLine(new Pen(Color.White), 11, 18, 21, 18);
            G.DrawLine(new Pen(Color.White), 11, 19, 21, 19);

            #endregion
            #region Close Icon

            //Convert Base64PNG to a BMP;
            Bitmap icon = new Bitmap(Image.FromStream(new System.IO.MemoryStream(Convert.FromBase64String("iVBORw0KGgoAAAANSUhEUgAAAA0AAAANCAYAAABy6+R8AAAACXBIWXMAAA7AAAAOwAFq1okJAAA4JGlUWHRYTUw6Y29tLmFkb2JlLnhtcAAAAAAAPD94cGFja2V0IGJlZ2luPSLvu78iIGlkPSJXNU0wTXBDZWhpSHpyZVN6TlRjemtjOWQiPz4KPHg6eG1wbWV0YSB4bWxuczp4PSJhZG9iZTpuczptZXRhLyIgeDp4bXB0az0iQWRvYmUgWE1QIENvcmUgNS42LWMwNjcgNzkuMTU3NzQ3LCAyMDE1LzAzLzMwLTIzOjQwOjQyICAgICAgICAiPgogICA8cmRmOlJERiB4bWxuczpyZGY9Imh0dHA6Ly93d3cudzMub3JnLzE5OTkvMDIvMjItcmRmLXN5bnRheC1ucyMiPgogICAgICA8cmRmOkRlc2NyaXB0aW9uIHJkZjphYm91dD0iIgogICAgICAgICAgICB4bWxuczp4bXA9Imh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC8iCiAgICAgICAgICAgIHhtbG5zOmRjPSJodHRwOi8vcHVybC5vcmcvZGMvZWxlbWVudHMvMS4xLyIKICAgICAgICAgICAgeG1sbnM6cGhvdG9zaG9wPSJodHRwOi8vbnMuYWRvYmUuY29tL3Bob3Rvc2hvcC8xLjAvIgogICAgICAgICAgICB4bWxuczp4bXBNTT0iaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wL21tLyIKICAgICAgICAgICAgeG1sbnM6c3RFdnQ9Imh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC9zVHlwZS9SZXNvdXJjZUV2ZW50IyIKICAgICAgICAgICAgeG1sbnM6dGlmZj0iaHR0cDovL25zLmFkb2JlLmNvbS90aWZmLzEuMC8iCiAgICAgICAgICAgIHhtbG5zOmV4aWY9Imh0dHA6Ly9ucy5hZG9iZS5jb20vZXhpZi8xLjAvIj4KICAgICAgICAgPHhtcDpDcmVhdG9yVG9vbD5BZG9iZSBQaG90b3Nob3AgQ0MgMjAxNSAoV2luZG93cyk8L3htcDpDcmVhdG9yVG9vbD4KICAgICAgICAgPHhtcDpDcmVhdGVEYXRlPjIwMTYtMDYtMDRUMTM6MjY6MDQrMDE6MDA8L3htcDpDcmVhdGVEYXRlPgogICAgICAgICA8eG1wOk1vZGlmeURhdGU+MjAxNi0wNi0wNFQxMzozMjo1MSswMTowMDwveG1wOk1vZGlmeURhdGU+CiAgICAgICAgIDx4bXA6TWV0YWRhdGFEYXRlPjIwMTYtMDYtMDRUMTM6MzI6NTErMDE6MDA8L3htcDpNZXRhZGF0YURhdGU+CiAgICAgICAgIDxkYzpmb3JtYXQ+aW1hZ2UvcG5nPC9kYzpmb3JtYXQ+CiAgICAgICAgIDxwaG90b3Nob3A6Q29sb3JNb2RlPjM8L3Bob3Rvc2hvcDpDb2xvck1vZGU+CiAgICAgICAgIDx4bXBNTTpJbnN0YW5jZUlEPnhtcC5paWQ6ZTExODJmMmUtNWQ3NS0yOTQyLWI2ZTQtNDk5ZDU4YzQwZjEwPC94bXBNTTpJbnN0YW5jZUlEPgogICAgICAgICA8eG1wTU06RG9jdW1lbnRJRD54bXAuZGlkOmUxMTgyZjJlLTVkNzUtMjk0Mi1iNmU0LTQ5OWQ1OGM0MGYxMDwveG1wTU06RG9jdW1lbnRJRD4KICAgICAgICAgPHhtcE1NOk9yaWdpbmFsRG9jdW1lbnRJRD54bXAuZGlkOmUxMTgyZjJlLTVkNzUtMjk0Mi1iNmU0LTQ5OWQ1OGM0MGYxMDwveG1wTU06T3JpZ2luYWxEb2N1bWVudElEPgogICAgICAgICA8eG1wTU06SGlzdG9yeT4KICAgICAgICAgICAgPHJkZjpTZXE+CiAgICAgICAgICAgICAgIDxyZGY6bGkgcmRmOnBhcnNlVHlwZT0iUmVzb3VyY2UiPgogICAgICAgICAgICAgICAgICA8c3RFdnQ6YWN0aW9uPmNyZWF0ZWQ8L3N0RXZ0OmFjdGlvbj4KICAgICAgICAgICAgICAgICAgPHN0RXZ0Omluc3RhbmNlSUQ+eG1wLmlpZDplMTE4MmYyZS01ZDc1LTI5NDItYjZlNC00OTlkNThjNDBmMTA8L3N0RXZ0Omluc3RhbmNlSUQ+CiAgICAgICAgICAgICAgICAgIDxzdEV2dDp3aGVuPjIwMTYtMDYtMDRUMTM6MjY6MDQrMDE6MDA8L3N0RXZ0OndoZW4+CiAgICAgICAgICAgICAgICAgIDxzdEV2dDpzb2Z0d2FyZUFnZW50PkFkb2JlIFBob3Rvc2hvcCBDQyAyMDE1IChXaW5kb3dzKTwvc3RFdnQ6c29mdHdhcmVBZ2VudD4KICAgICAgICAgICAgICAgPC9yZGY6bGk+CiAgICAgICAgICAgIDwvcmRmOlNlcT4KICAgICAgICAgPC94bXBNTTpIaXN0b3J5PgogICAgICAgICA8dGlmZjpPcmllbnRhdGlvbj4xPC90aWZmOk9yaWVudGF0aW9uPgogICAgICAgICA8dGlmZjpYUmVzb2x1dGlvbj45NTkxMDQvMTAwMDA8L3RpZmY6WFJlc29sdXRpb24+CiAgICAgICAgIDx0aWZmOllSZXNvbHV0aW9uPjk1OTEwNC8xMDAwMDwvdGlmZjpZUmVzb2x1dGlvbj4KICAgICAgICAgPHRpZmY6UmVzb2x1dGlvblVuaXQ+MjwvdGlmZjpSZXNvbHV0aW9uVW5pdD4KICAgICAgICAgPGV4aWY6Q29sb3JTcGFjZT42NTUzNTwvZXhpZjpDb2xvclNwYWNlPgogICAgICAgICA8ZXhpZjpQaXhlbFhEaW1lbnNpb24+MTM8L2V4aWY6UGl4ZWxYRGltZW5zaW9uPgogICAgICAgICA8ZXhpZjpQaXhlbFlEaW1lbnNpb24+MTM8L2V4aWY6UGl4ZWxZRGltZW5zaW9uPgogICAgICA8L3JkZjpEZXNjcmlwdGlvbj4KICAgPC9yZGY6UkRGPgo8L3g6eG1wbWV0YT4KICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAKPD94cGFja2V0IGVuZD0idyI/PmHZOLgAAAAgY0hSTQAAeiUAAICDAAD5/wAAgOkAAHUwAADqYAAAOpgAABdvkl/FRgAAAEhJREFUeNpi+P//PwOpGJnzn4Di/+iakAEuDXB5nBL4xBkIKMBqECGnYHUyAzF+IEYTyTaR7CeSQ4+ieCIpRZCEAQAAAP//AwCCbC3vkjidPQAAAABJRU5ErkJggg=="))), 13, 13);

            #region Dropshadow

            //Create a new BMP for the Dropshadow
            Bitmap ds = new Bitmap(23, 23);

            //Make a Graphics Variable from the DropShadow BMP;
            G = Graphics.FromImage(ds);

            //Quality Settings;
            G.CompositingQuality = CompositingQuality.HighQuality;
            G.InterpolationMode = InterpolationMode.HighQualityBicubic;

            #region Draw Dropshadow into the BMP
            G.FillRectangle(new SolidBrush(Color.FromArgb(16, Color.Black)), 2, 0, 1, 1);
            G.FillRectangle(new SolidBrush(Color.FromArgb(16, Color.Black)), 1, 1, 1, 1);
            G.FillRectangle(new SolidBrush(Color.FromArgb(16, Color.Black)), 9, 1, 1, 1);
            G.FillRectangle(new SolidBrush(Color.FromArgb(16, Color.Black)), 10, 0, 1, 1);
            G.FillRectangle(new SolidBrush(Color.FromArgb(16, Color.Black)), 11, 1, 1, 1);

            G.FillRectangle(new SolidBrush(Color.FromArgb(14, Color.Black)), 4, 2, 1, 1);
            G.FillRectangle(new SolidBrush(Color.FromArgb(14, Color.Black)), 5, 3, 1, 1);
            G.FillRectangle(new SolidBrush(Color.FromArgb(14, Color.Black)), 6, 4, 1, 1);
            G.FillRectangle(new SolidBrush(Color.FromArgb(14, Color.Black)), 7, 3, 1, 1);
            G.FillRectangle(new SolidBrush(Color.FromArgb(14, Color.Black)), 8, 2, 1, 1);
            G.FillRectangle(new SolidBrush(Color.FromArgb(14, Color.Black)), 3, 10, 1, 1);
            G.FillRectangle(new SolidBrush(Color.FromArgb(14, Color.Black)), 9, 10, 1, 1);

            G.FillRectangle(new SolidBrush(Color.FromArgb(12, Color.Black)), 2, 8, 1, 1);
            G.FillRectangle(new SolidBrush(Color.FromArgb(12, Color.Black)), 1, 9, 1, 1);
            G.FillRectangle(new SolidBrush(Color.FromArgb(12, Color.Black)), 0, 10, 1, 1);
            G.FillRectangle(new SolidBrush(Color.FromArgb(12, Color.Black)), 10, 8, 1, 1);
            G.FillRectangle(new SolidBrush(Color.FromArgb(12, Color.Black)), 11, 9, 1, 1);
            G.FillRectangle(new SolidBrush(Color.FromArgb(12, Color.Black)), 12, 10, 1, 1);

            G.FillRectangle(new SolidBrush(Color.FromArgb(38, Color.Black)), 1, 11, 1, 1);
            G.FillRectangle(new SolidBrush(Color.FromArgb(38, Color.Black)), 2, 12, 1, 1);
            G.FillRectangle(new SolidBrush(Color.FromArgb(38, Color.Black)), 3, 11, 1, 1);
            G.FillRectangle(new SolidBrush(Color.FromArgb(38, Color.Black)), 9, 11, 1, 1);
            G.FillRectangle(new SolidBrush(Color.FromArgb(38, Color.Black)), 10, 12, 1, 1);
            G.FillRectangle(new SolidBrush(Color.FromArgb(38, Color.Black)), 11, 11, 1, 1);

            G.FillRectangle(new SolidBrush(Color.FromArgb(40, Color.Black)), 6, 8, 1, 1);
            G.FillRectangle(new SolidBrush(Color.FromArgb(40, Color.Black)), 5, 9, 1, 1);
            G.FillRectangle(new SolidBrush(Color.FromArgb(40, Color.Black)), 4, 10, 1, 1);
            G.FillRectangle(new SolidBrush(Color.FromArgb(40, Color.Black)), 7, 9, 1, 1);
            G.FillRectangle(new SolidBrush(Color.FromArgb(40, Color.Black)), 8, 10, 1, 1);

            G.FillRectangle(new SolidBrush(Color.FromArgb(46, Color.Black)), 0, 2, 1, 1);
            G.FillRectangle(new SolidBrush(Color.FromArgb(46, Color.Black)), 1, 3, 1, 1);
            G.FillRectangle(new SolidBrush(Color.FromArgb(46, Color.Black)), 12, 2, 1, 1);
            G.FillRectangle(new SolidBrush(Color.FromArgb(46, Color.Black)), 11, 3, 1, 1);

            G.FillRectangle(new SolidBrush(Color.FromArgb(44, Color.Black)), 2, 4, 1, 1);
            G.FillRectangle(new SolidBrush(Color.FromArgb(44, Color.Black)), 3, 5, 1, 1);
            G.FillRectangle(new SolidBrush(Color.FromArgb(44, Color.Black)), 10, 4, 1, 1);
            G.FillRectangle(new SolidBrush(Color.FromArgb(44, Color.Black)), 9, 5, 1, 1);

            G.FillRectangle(new SolidBrush(Color.FromArgb(36, Color.Black)), 4, 6, 1, 1);
            G.FillRectangle(new SolidBrush(Color.FromArgb(36, Color.Black)), 8, 6, 1, 1);
            #endregion

            #endregion

            //Make a Graphics Variable from the Close BMP;
            G = Graphics.FromImage(closebmp);

            //Quality Settings;
            G.CompositingQuality = CompositingQuality.HighQuality;
            G.InterpolationMode = InterpolationMode.HighQualityBicubic;

            //Draw the Icon and DropShadow BMP's into the main Close BMP;
            G.DrawImageUnscaled(icon, 10, 8); //Icon;
            G.DrawImageUnscaled(ds, 10, 8); //Dropshadow;

            #endregion

        }

        //Draw the Control;
        protected override sealed void OnPaint(PaintEventArgs e)
        {

            //Set the Variable G to e.Graphics;
            Graphics G = e.Graphics;

            //Draw the Controls BackColor;
            G.Clear(BackColor);

            //Set the Quality;
            G.SmoothingMode = SmoothingMode.Default;
            G.CompositingQuality = CompositingQuality.AssumeLinear;
            G.InterpolationMode = InterpolationMode.HighQualityBicubic;

            #region Calculate OnHover/OnClick

            //Variables;
            int cb = 0; //Colorbyte;
            Color hc = default(Color); //Hovercolor;

            //Mouse State;
            switch (State)
            {

                //If it's Hovering Over;
                case MouseState.Over:

                    if (l == 1) //Close;
                    {
                        cb = 255;
                        hc = Color.Red;
                    }
                    else //Everything else;
                    {
                        cb = 45;
                        hc = Color.Black;
                    }

                    break; //Exit Switch;

                //If the left mouse button is clicked;
                case MouseState.Down:

                    //Set the Colorbyte;
                    cb = 35;
                    
                    if (l == 1) //Close;
                    {
                        hc = Color.Red;
                        //Close;
                    }
                    else //Everything else;
                    {
                        hc = Color.Black;
                    }

                    break; //Exit Switch;

            }

            #endregion

            //Draw the Rectangle;
            G.FillRectangle(new SolidBrush(Color.FromArgb(cb, hc)), x, 0, 34, Height);

            //Draw the Icons;
            G.DrawImageUnscaled(minbmp, 0, 0); //Minimize;
            G.DrawImageUnscaled(closebmp, 35, 0); //Close;

        }
        
        #region "Calculate Mouse State"

        //Variable to Store wether the Mouse is In Position or not;
        private bool InPosition;
        protected MouseState State;
        public enum MouseState : byte
        {
            None = 0,
            Over = 1,
            Down = 2,
            Block = 3
        }

        private void SetState(MouseState e)
        {
            State = e;
            //Set the State;
            Invalidate();
            //Redraw;
        }

        #region "Mouse Action Flags"

        #region "Move"

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            s = MouseState.Over;
            if (e.X >= 0 & e.X <= 34) //Minimize;
            {
                l = 0;
                x = 0;
            }
            else if (e.X >= 35) //Close;
            {
                l = 1;
                x = 34;
            }
            else //Fallback;
            {
                l = 2;
                x = -1000;
            }
            Invalidate();
        }

        #endregion
        #region "Down/Up"

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                SetState(MouseState.Down);
            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (InPosition)
                SetState(MouseState.Over);
            base.OnMouseUp(e);
        }

        #endregion
        #region "Enter/Leave"

        protected override void OnMouseEnter(EventArgs e)
        {
            InPosition = true;
            SetState(MouseState.Over);
            base.OnMouseEnter(e);
        }
        protected override void OnMouseLeave(EventArgs e)
        {
            InPosition = false;
            SetState(MouseState.None);
            base.OnMouseLeave(e);
        }

        #endregion
        #region "EnabledChanged"

        protected override void OnEnabledChanged(EventArgs e)
        {
            if (Enabled)
                SetState(MouseState.None);
            else
                SetState(MouseState.Block);
            base.OnEnabledChanged(e);
        }

        #endregion

        #endregion

        #endregion
        #region "Button Click Actions"

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            s = MouseState.Down;
            switch (l)
            {
                case 0:
                    //Minimize;
                    FindForm().WindowState = FormWindowState.Minimized;
                    break;
                case 1:
                    //Close;
                    FindForm().Close();
                    break;
                default:
                    break;
            }
        }

        #endregion

    }

    #endregion
    #region Material Button

    class MaterialButton : Button
    {

        //Variables;
        private Graphics G;

        //Default Properties;
        public MaterialButton()
        {
            Cursor = Cursors.Hand;
            Height = 34;
            Width = 150;
            ForeColor = Settings.Default.FontColor;
            Font = new Font("Kozuka Gothic Pro M", 9.5f);
            if (ReferenceEquals(Tag, "Alert"))
            {
                BackColor = Settings.Default.AlertColor;
            }
            else if (ReferenceEquals(Tag, "White"))
            {
                BackColor = Color.White;
            }
            else
            {
                BackColor = Settings.Default.HighlightColor;
            }
            DoubleBuffered = true;
        }

        #region Enabled Property
        public new bool Enabled
        {
            get { return base.Enabled; }
            set
            {
                base.Enabled = value;
                if (base.Enabled == true)
                {
                    if (ReferenceEquals(Tag, "Alert"))
                    {
                        BackColor = Settings.Default.AlertColor;
                    }
                    else if (ReferenceEquals(Tag, "White"))
                    {
                        BackColor = Color.White;
                    }
                    else
                    {
                        BackColor = Settings.Default.HighlightColor;
                    }
                    //Normal;
                    ForeColor = Properties.Settings.Default.FontColor;
                }
                else
                {
                    BackColor = Properties.Settings.Default.DisabledColor;
                    //Disabled;
                    ForeColor = Color.FromArgb(55, Properties.Settings.Default.FontColor);
                }
                Invalidate();
            }
        }
        #endregion
        #region MouseDown/Up
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (base.Enabled == true)
            {
                if (ReferenceEquals(Tag, "Alert"))
                {
                    BackColor = Settings.Default.AlertColor;
                }
                else if (ReferenceEquals(Tag, "White"))
                {
                    BackColor = Color.White;
                }
                else
                {
                    BackColor = Settings.Default.HighlightColor;
                }
            }
        }
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if (base.Enabled == true)
            {
                if (ReferenceEquals(Tag, "Alert"))
                {
                    BackColor = Color.FromArgb(230, Settings.Default.AlertColor);
                }
                else if (ReferenceEquals(Tag, "White"))
                {
                    BackColor = Color.FromArgb(230, Color.White);
                }
                else
                {
                    BackColor = Color.FromArgb(230, Settings.Default.HighlightColor);
                }
            }
        }
        #endregion
        #region MouseEnter/Leave
        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            if (base.Enabled == true)
            {
                if (ReferenceEquals(Tag, "Alert"))
                {
                    BackColor = Color.FromArgb(230, Settings.Default.AlertColor);
                }
                else if (ReferenceEquals(Tag, "White"))
                {
                    BackColor = Color.FromArgb(230, Color.White);
                }
                else
                {
                    BackColor = Color.FromArgb(230, Settings.Default.HighlightColor);
                }
            }
        }
        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            if (base.Enabled == true)
            {
                if (ReferenceEquals(Tag, "Alert"))
                {
                    BackColor = Settings.Default.AlertColor;
                }
                else if (ReferenceEquals(Tag, "White"))
                {
                    BackColor = Color.White;
                }
                else
                {
                    BackColor = Settings.Default.HighlightColor;
                }
            }
        }
        #endregion
        #region Paint

        protected override void OnPaint(PaintEventArgs e)
        {
            //Link e.Graphics to a Variable;
            G = e.Graphics;

            //Set Render Quality;
            G.SmoothingMode = (SmoothingMode)2;
            G.InterpolationMode = (InterpolationMode)7;
            G.TextRenderingHint = TextRenderingHint.AntiAlias;

            //Set the BackColor;
            G.Clear(BackColor);

            //If there is an Image tied to the Button;

            if ((BackgroundImage != null))
            {
                //Set the Default X/Y/W/H values;
                int IX = DisplayRectangle.X;
                int IY = DisplayRectangle.Y;
                int IWidth = DisplayRectangle.Width;
                int IHeight = DisplayRectangle.Height;

                //Calculate Padding;
                if (!(Padding.All == 0))
                {
                    IX = Convert.ToInt32(DisplayRectangle.X + Padding.Left / 2);
                    IY = Convert.ToInt32(DisplayRectangle.Y + Padding.Top / 2);
                    IWidth = DisplayRectangle.Width - Padding.Right;
                    IHeight = DisplayRectangle.Height - Padding.Bottom;
                }

                //Render the Image;
                G.DrawImage(BackgroundImage, IX, IY, IWidth, IHeight);

            }

            //Draw the Text and make it MiddleCenter;
            StringFormat sf = new StringFormat();
            sf.LineAlignment = StringAlignment.Center;
            sf.Alignment = StringAlignment.Center;
            G.DrawString(Text, new Font(LoadFont(Resources.Roboto_Medium), 10f), new SolidBrush(ForeColor), DisplayRectangle, sf);

        }
        #endregion

    }

    #endregion
    #region Label

    class CustomLabel : Label
    {

        //Variables;
        private Graphics G;

        //Default Properties;
        public CustomLabel()
        {
            ForeColor = Settings.Default.FontColor;
            AutoSize = false;
        }

        #region Paint
        protected override void OnPaint(PaintEventArgs e)
        {
            //Link e.Graphics to a Variable;
            G = e.Graphics;

            //Set Render Quality;
            G.SmoothingMode = (SmoothingMode)2;
            G.InterpolationMode = (InterpolationMode)7;
            G.TextRenderingHint = TextRenderingHint.AntiAlias;

            //Set the BackColor;
            G.Clear(Parent.BackColor);

            //Draw the Text and make it MiddleCenter;
            StringFormat sf = new StringFormat();
            sf.LineAlignment = StringAlignment.Center;
            sf.Alignment = StringAlignment.Center;
            G.DrawString(Text, new Font(LoadFont(Resources.Roboto_Medium), Font.Size), new SolidBrush(ForeColor), DisplayRectangle, sf);

        }
        #endregion

    }

    #endregion
    #region CustomTabControl

    public class CustomTabControl : TabControl
    {
        
        #region Remove Padding
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x1300 + 40)
            {
                lparamrect rc = (lparamrect)m.GetLParam(typeof(lparamrect));
                rc.Left -= 4;
                rc.Right += 4;
                rc.Top -= 2;
                rc.Bottom += 4;
                Marshal.StructureToPtr(rc, m.LParam, true);
            }
            base.WndProc(ref m);
        }
        internal struct lparamrect
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }
        #endregion

        //Variables;
        private Rectangle Rect;
        private int _OverIndex = -1;
        private int OverIndex
        {
            get { return _OverIndex; }
            set
            {
                _OverIndex = value;
                Invalidate();
            }
        }

        //Default Settings;
        public CustomTabControl()
        {
            DoubleBuffered = true;
            Alignment = TabAlignment.Left;
            SizeMode = TabSizeMode.Fixed;
            ItemSize = new Size(30, 32);
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.UserPaint, true);
        }

        protected override void OnControlAdded(ControlEventArgs e)
        {
            base.OnControlAdded(e);
            e.Control.BackColor = ColorFromHex("#E5E5E5");
            e.Control.ForeColor = ColorFromHex("#FFFFFF");
            e.Control.Font = new Font("Kozuka Gothic Pro B", 9);
        }
        
        protected override void OnPaint(PaintEventArgs e)
        {

            //Set the Graphics Variable;
            Graphics G = e.Graphics;
            
            //Raise the Paint event;
            base.OnPaint(e);

            //Dynamically Darken the Parent's Color slightly;
            Color DarkParentColor = default(Color);
            if (ColorContrast(Parent.BackColor) == Color.White)
            {
                DarkParentColor = ControlPaint.Light(ControlPaint.Dark(Parent.BackColor, (float)0.01), (float)1.2);
            }
            else
            {
                DarkParentColor = ControlPaint.Light(ControlPaint.Dark(Parent.BackColor, (float)0.1), (float)0.35);
            }

            //Set the Tab's Background Color to the Darkened Parent's Color;
            G.Clear(DarkParentColor);

            //For Each Tab;
            for (int i = 0; i <= TabPages.Count - 1; i++)
            {

                if ((string)TabPages[i].Tag != "Hidden")
                {

                    //Get the Tab's Rectangle Size;
                    Rect = GetTabRect(i);

                    //If the user is hovering over the tab;
                    if (!(OverIndex == -1))
                    {
                        //Draw the Hover Background;
                        G.FillRectangle(new SolidBrush(Color.FromArgb(15, Color.Black)), new Rectangle(GetTabRect(OverIndex).X, GetTabRect(OverIndex).Y, GetTabRect(OverIndex).Width, GetTabRect(OverIndex).Height));


                    }
                    else if (!(SelectedIndex == i))
                    {
                        //Set the Background Color to the Darkened Parent's Color;
                        G.FillRectangle(new SolidBrush(DarkParentColor), new Rectangle(Rect.X, Rect.Y, Rect.Width + 7, Rect.Height));

                    }

                    //If its the Selected Tab;
                    if (SelectedIndex == i)
                    {
                        //Set the Selected Indicator to the Highlight Color;
                        G.FillRectangle(new SolidBrush(Properties.Settings.Default.HighlightColor), new Rectangle(Rect.X, Rect.Y + 8, 4, Rect.Height - 16));

                    }

                    float x = Convert.ToSingle((Rect.X + (Rect.Height - 10)) - (Rect.Height / 2));
                    float y = Convert.ToSingle((Rect.Y + (Rect.Height - 10)) - (Rect.Height / 2));

                    if ((!ReferenceEquals(Tag, "TextOnly")))
                    {
                        if (SelectedIndex == i)
                        {
                            G.DrawImage(GetTabIcon(i), new Rectangle(Convert.ToInt32(x) + 2, Convert.ToInt32(y), 20, 20));
                        }
                        else
                        {
                            G.DrawImage(GetTabIcon(i), new Rectangle(Convert.ToInt32(x), Convert.ToInt32(y), 20, 20));
                        }
                    }
                    else
                    {
                        dynamic TabProperties = TabPages[i];
                        StringFormat sf = new StringFormat();
                        sf.LineAlignment = StringAlignment.Center;
                        G.DrawString(TabProperties.Text, TabProperties.Font, new SolidBrush(TabProperties.ForeColor), x + 2, Convert.ToSingle(Rect.Y + (Rect.Height / 2)), sf);
                    }

                }

            }

        }

        protected override void OnSelecting(TabControlCancelEventArgs e)
        {
            base.OnSelecting(e);

            if ((e.TabPage != null))
            {
                if (!string.IsNullOrEmpty(Convert.ToString(e.TabPage.Tag)))
                {
                    e.Cancel = true;
                }
                else
                {
                    OverIndex = -1;
                }
            }

        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            for (int I = 0; I <= TabPages.Count - 1; I++)
            {
                if (GetTabRect(I).Contains(e.Location) & string.IsNullOrEmpty(Convert.ToString(TabPages[I].Tag)))
                {
                    OverIndex = I;
                    break; // TODO: might not be correct. Was : Exit For
                }
                else
                {
                    OverIndex = -1;
                }
            }

        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            OverIndex = -1;
        }

    }

    #endregion
    #region Separator

    public class Separator : Control
    {

        //Default Properties;
        public Separator()
        {
            DoubleBuffered = true;
        }

        protected override void OnPaint(PaintEventArgs e)
        {

            //Setup Graphics;
            Graphics G = e.Graphics;
            G.SmoothingMode = SmoothingMode.HighQuality;
            G.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            //Raise the OnPaint Flag;
            base.OnPaint(e);

            //Draw a 1 pixel line;
            using (Pen C = new Pen(ColorFromHex("#EBEBEC")))
            {
                G.DrawLine(C, new Point(0, 0), new Point(Width, 0));
            }

        }

        protected override void OnResize(EventArgs e)
        {

            //Raise the OnResize Flag;
            base.OnResize(e);

            //Make sure the size is always 2 pixels;
            Size = new Size(Width, 2);

        }

    }

    #endregion
    #region CustomCheckbox

    [DefaultEvent("CheckedChanged")]
    public class CustomCheckBox : Control
    {

        //Variables;
        public event CheckedChangedEventHandler CheckedChanged;
        public delegate void CheckedChangedEventHandler(object sender, EventArgs e);
        private bool _Checked;
        private bool _EnabledCalc;
        private Graphics G;
        private string B64Enabled = "iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAA00lEQVQ4T6WTwQ2CMBSG30/07Ci6gY7gxZoIiYADuAIrsIDpQQ/cHMERZBOuXHimDSWALYL01EO/L//724JmLszk6S+BCOIExFsmL50sEH4kAZxVciYuJgnacD16Plpgg8tFtYMILntQdSXiZ3aXqa1UF/yUsoDw4wKglQaZZPa4RW3JEKzO4RjEbyJaN1BL8gvWgsMp3ADeq0lRJ2FimLZNYWpmFbudUJdolXTLyG2wTmDODUiccEfgSDIIfwmMxAMStS+XHPZn7l/z6Ifk+nSzBR8zi2d9JmVXSgAAAABJRU5ErkJggg==";

        private string B64Disabled = "iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAA1UlEQVQ4T6WTzQ2CQBCF56EnLpaiXvUAJBRgB2oFtkALdEAJnoVEMIGzdEIFjNkFN4DLn+xpD/N9efMWQAsPFvL0lyBMUg8MiwzyZwuiJAuI6CyTMxezBC24EuSTBTp4xaaN6JWdqKQbge6udfB1pfbBjrMvEMZZAdCm3ilw7eO1KRmCxRyiOH0TsFUQs5KMwVLweKY7ALFKUZUTECD6qdquCxM7i9jNhLJEraQ5xZzrYJngO9crGYBbAm2SEfhHoCQGeeK+Ls1Ld+fuM0/+kPp+usWCD10idEOGa4QuAAAAAElFTkSuQmCC";
        //Default Properties;
        public CustomCheckBox()
        {
            DoubleBuffered = true;
            Enabled = true;
        }

        #region "Checked Property"
        public bool Checked
        {
            get { return _Checked; }
            set
            {
                _Checked = value;
                Invalidate();
            }
        }
        #endregion
        #region "Enabled Property"
        public new bool Enabled
        {
            get { return EnabledCalc; }
            set
            {
                _EnabledCalc = value;
                if (Enabled)
                {
                    Cursor = Cursors.Hand;
                }
                else
                {
                    Cursor = Cursors.Default;
                }
                Invalidate();
            }
        }

        [DisplayName("Enabled")]
        public bool EnabledCalc
        {
            get { return _EnabledCalc; }
            set
            {
                Enabled = value;
                Invalidate();
            }
        }
        #endregion
        #region "OnMouseUp"
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if (Enabled)
            {
                Checked = !Checked;
                if (CheckedChanged != null)
                {
                    CheckedChanged(this, e);
                }
            }
        }
        #endregion
        #region "OnResize"
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            Size = new Size(Width, 18);
        }
        #endregion

        #region "OnPaint"

        protected override void OnPaint(PaintEventArgs e)
        {
            //Set the Graphics Variable and Graphical Settings;
            G = e.Graphics;
            G.SmoothingMode = SmoothingMode.HighQuality;
            G.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            //Raise the Paint event;
            base.OnPaint(e);

            //Set the Background Color to the Parent Control's BackColor;
            G.Clear(Parent.BackColor);

            //If the CheckBox is Enabled;

            if (Enabled)
            {
                //Draw the CheckBox and Text;
                using (SolidBrush Background = new SolidBrush(ColorFromHex("#F3F4F7")))
                {
                    using (Pen Border = new Pen(ColorFromHex("#D0D5D9")))
                    {
                        using (SolidBrush TextColor = new SolidBrush(Color.Black))
                        {
                            using (Font TextFont = new Font("Segoe UI", 9))
                            {
                                G.FillPath(Background, RoundRect(new Rectangle(0, 0, 16, 16), 3));
                                G.DrawPath(Border, RoundRect(new Rectangle(0, 0, 16, 16), 3));
                                G.DrawString(Text, TextFont, TextColor, new Point(25, 0));
                            }
                        }
                    }
                }

                //If the CheckBox is Checked;

                if (Checked)
                {
                    //Draw the CheckMark over the CheckBox;
                    using (Image I = Image.FromStream(new System.IO.MemoryStream(Convert.FromBase64String(B64Enabled))))
                    {
                        G.DrawImage(I, new Rectangle(3, 3, 11, 11));
                    }

                }


            }
            else
            {
                //Draw the CheckBox and Text;
                using (SolidBrush Background = new SolidBrush(ColorFromHex("#F5F5F8")))
                {
                    using (Pen Border = new Pen(ColorFromHex("#E1E1E2")))
                    {
                        using (SolidBrush TextColor = new SolidBrush(Color.Gray))
                        {
                            using (Font TextFont = new Font("Segoe UI", 9))
                            {
                                G.FillPath(Background, RoundRect(new Rectangle(0, 0, 16, 16), 3));
                                G.DrawPath(Border, RoundRect(new Rectangle(0, 0, 16, 16), 3));
                                G.DrawString(Text, TextFont, TextColor, new Point(25, 0));
                            }
                        }
                    }
                }

                //If the CheckBox is Checked;

                if (Checked)
                {
                    //Draw the CheckMark over the CheckBox;
                    using (Image I = Image.FromStream(new System.IO.MemoryStream(Convert.FromBase64String(B64Disabled))))
                    {
                        G.DrawImage(I, new Rectangle(3, 3, 11, 11));
                    }

                }

            }

        }
        #endregion

    }

    #endregion

}
