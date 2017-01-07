using kit_kat;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Newtonsoft.Json;
using System;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Net;

namespace InputRedirection
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D Font;
        Texture2D Cursor;
        IPAddress ipAddress;
        string IPAddress = kit_kat.Properties.Settings.Default.IPAddress;
        byte[] data = new byte[12];
        uint oldbuttons = 0xFFF;
        uint newbuttons = 0xFFF;
        uint oldtouch = 0x2000000;
        uint newtouch = 0x2000000;
        uint oldcpad = 0x800800;
        uint newcpad = 0x800800;
        uint touchclick = 0x00;
        uint cpadclick = 0x00;
        int Mode = 0;
        Keys[] ipKeysToCheck = { Keys.D0, Keys.D1, Keys.D2, Keys.D3, Keys.D4, Keys.D5, Keys.D6, Keys.D7, Keys.D8, Keys.D9, Keys.NumPad0, Keys.NumPad1, Keys.NumPad2, Keys.NumPad3, Keys.NumPad4, Keys.NumPad5, Keys.NumPad6, Keys.NumPad7, Keys.NumPad8, Keys.NumPad9, Keys.Decimal, Keys.OemPeriod, Keys.Back, Keys.Delete, Keys.Escape };
        Keys[] buttonKeysToCheck = { Keys.A, Keys.B, Keys.RightShift, Keys.LeftShift, Keys.Enter, Keys.Right, Keys.Left, Keys.Up, Keys.Down, Keys.R, Keys.L, Keys.X, Keys.Y, Keys.Escape };
        Keys[] KeyboardInput = { Keys.A, Keys.S, Keys.N, Keys.M, Keys.H, Keys.F, Keys.T, Keys.G, Keys.W, Keys.Q, Keys.Z, Keys.X, Keys.Right, Keys.Left, Keys.Up, Keys.Down };
        uint[] GamePadInput = { 0x01, 0x02, 0x04, 0x08, 0x10, 0x020, 0x40, 0x80, 0x100, 0x200, 0x400, 0x800 };
        string[] ButtonNames = { "A", "B", "Select", "Start", "DPad Right", "DPad Left", "DPad Up", "DPad Down", "R", "L", "X", "Y" };
        Keys UpKey;
        bool WaitForKeyUp;
        bool debug = false;
        KeyboardState keyboardState;
        GamePadState gamePadState;
        uint KeyIndex;
        Keys OldKey;
        uint OldButton;
        uint seconds = 0;
        bool useGamePad = true;

        void graphics_PreparingDeviceSettings(object sender, PreparingDeviceSettingsEventArgs e)
        {
            e.GraphicsDeviceInformation.PresentationParameters.DeviceWindowHandle = drawSurface;
        }
        private void Game1_VisibleChanged(object sender, EventArgs e)
        {
            if (System.Windows.Forms.Control.FromHandle((Window.Handle)).Visible == true)
                System.Windows.Forms.Control.FromHandle((Window.Handle)).Visible = false;
        }

        private IntPtr drawSurface;
        public Game1(IntPtr drawSurface)
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = 240;
            graphics.PreferredBackBufferWidth = 320;
            this.drawSurface = drawSurface;
            graphics.PreparingDeviceSettings +=
            new EventHandler<PreparingDeviceSettingsEventArgs>(graphics_PreparingDeviceSettings);
            System.Windows.Forms.Control.FromHandle((Window.Handle)).VisibleChanged +=
            new EventHandler(Game1_VisibleChanged);
        }

        protected override void Initialize()
        {
            IsMouseVisible = true;
            base.Initialize();
        }

        protected override void LoadContent()
        {

            ReadConfig();

            spriteBatch = new SpriteBatch(GraphicsDevice);

            #region Font
            byte[] fontbyte;
            using (var stream = new MemoryStream())
            {
                kit_kat.Properties.Resources.Font.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                fontbyte = stream.ToArray();
            }
            using (Stream strm = new MemoryStream(fontbyte))
            {
                Font = Texture2D.FromStream(GraphicsDevice, strm);
            }
            #endregion
            #region Cursor
            byte[] cursorbyte;
            using (var stream = new MemoryStream())
            {
                kit_kat.Properties.Resources.Cursor.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                cursorbyte = stream.ToArray();
            }
            using (Stream strm = new MemoryStream(cursorbyte))
            {
                Cursor = Texture2D.FromStream(GraphicsDevice, strm);
            }
            #endregion

        }

        protected override void UnloadContent()
        {

        }


        protected override void Update(GameTime gameTime)
        {
            if (gameTime.TotalGameTime.TotalSeconds != seconds)
            {
                seconds = (uint)gameTime.TotalGameTime.TotalSeconds;
                Program.ir.sendHeartbeatPacket();
            }

            switch (Mode)
            {
                case 0:
                    {
                        IsMouseVisible = !debug;
                        ReadMain();
                    }
                    break;

                case 1:
                    {
                        IsMouseVisible = true;
                        ReadIPInput();
                    }
                    break;

                case 2:
                    {
                        IsMouseVisible = true;
                        ReadKeyboardInput();
                    }
                    break;

                case 3:
                    {
                        IsMouseVisible = true;
                        ReadGamePadInput();
                    }
                    break;

                case 4:
                    {
                        IsMouseVisible = true;
                        ReadNewKey();
                    }
                    break;

                case 5:
                    {
                        IsMouseVisible = true;
                        ReadNewButton();
                    }
                    break;

            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Microsoft.Xna.Framework.Color.Black);
            spriteBatch.Begin();
            {
                switch (Mode)
                {
                    case 0:
                        {
                            ShowMain();
                        }
                        break;

                    case 1:
                        {
                            ShowIPInput();
                        }
                        break;

                    case 2:
                    case 4:
                        {
                            ShowKeyboardInput();
                        }
                        break;

                    case 3:
                    case 5:
                        {
                            ShowGamePadInput();
                        }
                        break;
                }
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }

        protected override void OnExiting(Object sender, EventArgs args)
        {
            Program.ir.disconnect();
        }

        private void ReadConfig()
        {
            // IP Address
            IPAddress = kit_kat.Properties.Settings.Default.IPAddress;

            // Debug Mode
            if (kit_kat.Properties.Settings.Default.IRDebug == true) { debug = true; IsMouseVisible = false; }

            // Gamepad Mode
            if (kit_kat.Properties.Settings.Default.IRGamepad == false) { useGamePad = false; }

            // Keyboard Input
            string[] IRKB = kit_kat.Properties.Settings.Default.IRKB.Split(',');
            for (int i = 0; i < KeyboardInput.Length; i++)
            {
                KeyboardInput[i] = (Keys)Enum.Parse(typeof(Keys), IRKB[i]);
            }

            // Gamepad Input
            string[] IRGP = kit_kat.Properties.Settings.Default.IRGP.Split(',');
            for (int i = 0; i < IRGP.Length; i++)
            {
                GamePadInput[i] = Convert.ToUInt32(IRGP[i]);
            }
        }

        private void SaveConfig()
        {

            string KB = "";
            for (int i = 0; i < KeyboardInput.Length; i++) {
                KB += KeyboardInput[i].ToString();
                if (i != (KeyboardInput.Length - 1)) { KB += ","; }
            }
            System.Windows.Forms.MessageBox.Show(KB.ToString());
            kit_kat.Properties.Settings.Default["IRKB"] = KB;
            kit_kat.Properties.Settings.Default.Save();

            string GP = "";
            for (int i = 0; i < GamePadInput.Length; i++) {
                GP += GamePadInput[i].ToString();
                if (i != (GamePadInput.Length - 1)) { GP += ","; }
            }
            kit_kat.Properties.Settings.Default["IRGP"] = GP;
            kit_kat.Properties.Settings.Default.Save();

        }

        private void CheckConnection()
        {
            if (!Program.ir.isConnected)
            {
                if(IPAddress != "3DS IP Address")
                {
                    try
                    {
                        Program.ir.setServer(IPAddress, 8000);
                        Program.ir.connectToServer(false);
                    }
                    catch (Exception)
                    {
                        log("", "logger3", "Failed to connect!");
                    }
                } else
                {
                    log("", "logger3", "Failed to connect!");
                }
            }
        }

        private void ReadNewKey()
        {
            if (!WaitForKeyUp)
            {
                keyboardState = Keyboard.GetState();

                if (keyboardState.GetPressedKeys().Length > 0)
                {
                    switch (keyboardState.GetPressedKeys()[0])
                    {
                        case Keys.Escape:
                            {
                                KeyboardInput[KeyIndex] = OldKey;
                                Mode = 2;
                            }
                            break;

                        case Keys.F1:
                        case Keys.F2:
                        case Keys.F3:
                        case Keys.F4:
                        case Keys.F5:
                            {
                                break;
                            }

                        default:
                            {
                                for (int i = 0; i < KeyboardInput.Length; i++)
                                {
                                    if (keyboardState.GetPressedKeys()[0] == KeyboardInput[i])
                                    {
                                        break;
                                    }

                                    if (i == (KeyboardInput.Length - 1))
                                    {
                                        KeyboardInput[KeyIndex] = keyboardState.GetPressedKeys()[0];
                                        Mode = 2;
                                        WaitForKeyUp = true;
                                        UpKey = keyboardState.GetPressedKeys()[0];
                                    }
                                }
                            }
                            break;
                    }
                }
            }
            else
            {
                if (Keyboard.GetState().IsKeyUp(UpKey))
                {
                    WaitForKeyUp = false;
                }
            }
        }

        private void ReadNewButton()
        {
            if (!WaitForKeyUp)
            {
                for (int i = 0; i < buttonKeysToCheck.Length; i++)
                {
                    if (Keyboard.GetState().IsKeyDown(buttonKeysToCheck[i]))
                    {
                        WaitForKeyUp = true;
                        UpKey = buttonKeysToCheck[i];
                        switch (buttonKeysToCheck[i])
                        {
                            case Keys.Escape:
                                {
                                    if (System.Net.IPAddress.TryParse(IPAddress, out ipAddress))
                                    {
                                        GamePadInput[KeyIndex] = OldButton;
                                        Mode = 3;
                                    }
                                }
                                break;

                            default:
                                {
                                    switch (buttonKeysToCheck[i])
                                    {
                                        case Keys.A:
                                            {
                                                GamePadInput[KeyIndex] = 0x01;
                                                Mode = 3;
                                            }
                                            break;

                                        case Keys.B:
                                            {
                                                GamePadInput[KeyIndex] = 0x02;
                                                Mode = 3;
                                            }
                                            break;

                                        case Keys.RightShift:
                                        case Keys.LeftShift:
                                            {
                                                GamePadInput[KeyIndex] = 0x04;
                                                Mode = 3;
                                            }
                                            break;

                                        case Keys.Enter:
                                            {
                                                GamePadInput[KeyIndex] = 0x08;
                                                Mode = 3;
                                            }
                                            break;

                                        case Keys.Right:
                                            {
                                                GamePadInput[KeyIndex] = 0x10;
                                                Mode = 3;
                                            }
                                            break;

                                        case Keys.Left:
                                            {
                                                GamePadInput[KeyIndex] = 0x20;
                                                Mode = 3;
                                            }
                                            break;

                                        case Keys.Up:
                                            {
                                                GamePadInput[KeyIndex] = 0x40;
                                                Mode = 3;
                                            }
                                            break;

                                        case Keys.Down:
                                            {
                                                GamePadInput[KeyIndex] = 0x80;
                                                Mode = 3;
                                            }
                                            break;

                                        case Keys.R:
                                            {
                                                GamePadInput[KeyIndex] = 0x100;
                                                Mode = 3;
                                            }
                                            break;

                                        case Keys.L:
                                            {
                                                GamePadInput[KeyIndex] = 0x200;
                                                Mode = 3;
                                            }
                                            break;

                                        case Keys.X:
                                            {
                                                GamePadInput[KeyIndex] = 0x400;
                                                Mode = 3;
                                            }
                                            break;

                                        case Keys.Y:
                                            {
                                                GamePadInput[KeyIndex] = 0x800;
                                                Mode = 3;
                                            }
                                            break;
                                    }
                                }
                                break;
                        }
                    }
                }
            }
            else
            {
                if (Keyboard.GetState().IsKeyUp(UpKey))
                {
                    WaitForKeyUp = false;
                }
            }
        }

        private void ReadMain()
        {
            if (!WaitForKeyUp)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.F1))
                {
                    WaitForKeyUp = true;
                    UpKey = Keys.F1;
                    Mode = 1;
                }

                if (Keyboard.GetState().IsKeyDown(Keys.F2))
                {
                    WaitForKeyUp = true;
                    UpKey = Keys.F2;
                    Mode = 2;
                }

                if (Keyboard.GetState().IsKeyDown(Keys.F3))
                {
                    WaitForKeyUp = true;
                    UpKey = Keys.F3;
                    Mode = 3;
                }

                if (Keyboard.GetState().IsKeyDown(Keys.F4))
                {
                    WaitForKeyUp = true;
                    UpKey = Keys.F4;
                    debug = !debug;
                    this.IsMouseVisible = !this.IsMouseVisible;
                    SaveConfig();
                }

                if (Keyboard.GetState().IsKeyDown(Keys.F5))
                {
                    WaitForKeyUp = true;
                    UpKey = Keys.F5;
                    useGamePad = !useGamePad;
                    SaveConfig();
                }
            }
            else
            {
                if (Keyboard.GetState().IsKeyUp(UpKey))
                {
                    WaitForKeyUp = false;
                }
            }

            keyboardState = Keyboard.GetState();
            gamePadState = GamePad.GetState(PlayerIndex.One);
            newbuttons = 0x00;
            //Keyboard
            for (int i = 0; i < GamePadInput.Length; i++)
            {
                if (keyboardState.IsKeyDown(KeyboardInput[i]))
                {
                    newbuttons += (uint)(0x01 << i);
                }
            }

            //GamePad
            if (useGamePad)
            {
                if (GamePad.GetState(PlayerIndex.One).Buttons.B == ButtonState.Pressed)
                {
                    if ((newbuttons & GamePadInput[0]) != GamePadInput[0])
                    {
                        newbuttons += GamePadInput[0];
                    }
                }

                if (GamePad.GetState(PlayerIndex.One).Buttons.A == ButtonState.Pressed)
                {
                    if ((newbuttons & GamePadInput[1]) != GamePadInput[1])
                    {
                        newbuttons += GamePadInput[1];
                    }
                }


                if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                {
                    if ((newbuttons & GamePadInput[2]) != GamePadInput[2])
                    {
                        newbuttons += GamePadInput[2];
                    }
                }

                if (GamePad.GetState(PlayerIndex.One).Buttons.Start == ButtonState.Pressed)
                {
                    if ((newbuttons & GamePadInput[3]) != GamePadInput[3])
                    {
                        newbuttons += GamePadInput[3];
                    }
                }

                if (GamePad.GetState(PlayerIndex.One).DPad.Right == ButtonState.Pressed)
                {
                    if ((newbuttons & GamePadInput[4]) != GamePadInput[4])
                    {
                        newbuttons += GamePadInput[4];
                    }
                }

                if (GamePad.GetState(PlayerIndex.One).DPad.Left == ButtonState.Pressed)
                {
                    if ((newbuttons & GamePadInput[5]) != GamePadInput[5])
                    {
                        newbuttons += GamePadInput[5];
                    }
                }

                if (GamePad.GetState(PlayerIndex.One).DPad.Up == ButtonState.Pressed)
                {
                    if ((newbuttons & GamePadInput[6]) != GamePadInput[6])
                    {
                        newbuttons += GamePadInput[6];
                    }
                }

                if (GamePad.GetState(PlayerIndex.One).DPad.Down == ButtonState.Pressed)
                {
                    if ((newbuttons & GamePadInput[7]) != GamePadInput[7])
                    {
                        newbuttons += GamePadInput[7];
                    }
                }

                if (GamePad.GetState(PlayerIndex.One).Buttons.RightShoulder == ButtonState.Pressed)
                {
                    if ((newbuttons & GamePadInput[8]) != GamePadInput[8])
                    {
                        newbuttons += GamePadInput[8];
                    }
                }

                if (GamePad.GetState(PlayerIndex.One).Buttons.LeftShoulder == ButtonState.Pressed)
                {
                    if ((newbuttons & GamePadInput[9]) != GamePadInput[9])
                    {
                        newbuttons += GamePadInput[9];
                    }
                }

                if (GamePad.GetState(PlayerIndex.One).Buttons.Y == ButtonState.Pressed)
                {
                    if ((newbuttons & GamePadInput[10]) != GamePadInput[10])
                    {
                        newbuttons += GamePadInput[10];
                    }
                }

                if (GamePad.GetState(PlayerIndex.One).Buttons.X == ButtonState.Pressed)
                {
                    if ((newbuttons & GamePadInput[11]) != GamePadInput[11])
                    {
                        newbuttons += GamePadInput[11];
                    }
                }
            }

            newbuttons ^= 0xFFF;

            //Touch
            if (MainForm.isMouseDown == true)
            {
                TouchInput(ref newtouch, ref touchclick, false);
            }
            else
            {
                touchclick = 0x00;
                if (useGamePad)
                {
                    if (GamePad.GetState(PlayerIndex.One).Buttons.RightStick == ButtonState.Pressed)
                    {
                        newtouch = (uint)Math.Round(2047.5 + (GamePad.GetState(PlayerIndex.One).ThumbSticks.Right.X * 2047.5));
                        newtouch += (uint)Math.Round((2047.5 + (GamePad.GetState(PlayerIndex.One).ThumbSticks.Right.Y * 2047.5)) + 4095) << 0x0C;
                        newtouch += 0x1000000;
                    }
                    else
                    {
                        newtouch = 0x2000000;
                    }
                }
                else
                {
                    newtouch = 0x2000000;
                }
            }

            //Circle Pad
            if (Mouse.GetState().RightButton == ButtonState.Pressed)
            {
                TouchInput(ref newcpad, ref cpadclick, true);
            }
            else
            {
                cpadclick = 0x00;
                newcpad = (uint)Math.Round(2047.5 + (GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X * 2047.5));
                newcpad += (uint)Math.Round(2047.5 + (GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y * 2047.5)) << 0x0C;

                if (newcpad == 0x800800)
                {

                    if (Keyboard.GetState().IsKeyDown(KeyboardInput[12]))
                    {
                        newcpad = 0xFFF + (((newcpad >> 0x0C) & 0xFFF) << 0x0C);
                    }

                    if (Keyboard.GetState().IsKeyDown(KeyboardInput[13]))
                    {
                        newcpad = (((newcpad >> 0x0C) & 0xFFF) << 0x0C);
                    }

                    if (Keyboard.GetState().IsKeyDown(KeyboardInput[15]))
                    {
                        newcpad = (newcpad & 0xFFF) + (0x00 << 0x0C);
                    }

                    if (Keyboard.GetState().IsKeyDown(KeyboardInput[14]))
                    {
                        newcpad = (newcpad & 0xFFF) + (0xFFF << 0x0C);
                    }
                }

                if (newcpad != 0x800800)
                {
                    newcpad += 0x1000000;
                }
            }

            SendInput();
        }

        private void ReadIPInput()
        {
            if (!WaitForKeyUp)
            {
                for (int i = 0; i < ipKeysToCheck.Length; i++)
                {
                    if (Keyboard.GetState().IsKeyDown(ipKeysToCheck[i]))
                    {
                        WaitForKeyUp = true;
                        UpKey = ipKeysToCheck[i];
                        switch (ipKeysToCheck[i])
                        {
                            case Keys.Back:
                            case Keys.Delete:
                                {
                                    if (IPAddress.Length != 0)
                                    {
                                        IPAddress = IPAddress.Substring(0, IPAddress.Length - 1);
                                    }
                                }
                                break;

                            case Keys.Escape:
                                {
                                    if (System.Net.IPAddress.TryParse(IPAddress, out ipAddress))
                                    {
                                        Mode = 0;
                                        IPAddress = ipAddress.ToString();
                                        SaveConfig();
                                        Program.ir.disconnect();
                                    }
                                }
                                break;

                            default:
                                {
                                    if (IPAddress.Length < 15)
                                    {
                                        IPAddress += KeytoText(ipKeysToCheck[i]);
                                    }
                                }
                                break;
                        }
                    }
                }
            }
            else
            {
                if (Keyboard.GetState().IsKeyUp(UpKey))
                {
                    WaitForKeyUp = false;
                }
            }
        }

        private void ReadKeyboardInput()
        {
            if (!WaitForKeyUp)
            {
                for (int i = 0; i < KeyboardInput.Length; i++)
                {
                    if (Keyboard.GetState().IsKeyDown(KeyboardInput[i]))
                    {
                        WaitForKeyUp = true;
                        UpKey = KeyboardInput[i];
                        OldKey = KeyboardInput[i];
                        KeyboardInput[i] = Keys.None;
                        KeyIndex = (uint)i;
                        Mode = 4;
                    }
                }

                if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                {
                    Mode = 0;
                    WaitForKeyUp = true;
                    UpKey = Keys.Escape;
                    SaveConfig();
                }
            }
            else
            {
                if (Keyboard.GetState().IsKeyUp(UpKey))
                {
                    WaitForKeyUp = false;
                }
            }
        }

        private void ReadGamePadInput()
        {
            if (!WaitForKeyUp)
            {
                if (GamePad.GetState(PlayerIndex.One).Buttons.B == ButtonState.Pressed)
                {
                    Mode = 5;
                    KeyIndex = 0;
                    OldButton = GamePadInput[KeyIndex];
                    GamePadInput[KeyIndex] = 0;
                }

                if (GamePad.GetState(PlayerIndex.One).Buttons.A == ButtonState.Pressed)
                {
                    Mode = 5;
                    KeyIndex = 1;
                    OldButton = GamePadInput[KeyIndex];
                    GamePadInput[KeyIndex] = 0;
                }


                if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                {
                    Mode = 5;
                    KeyIndex = 2;
                    OldButton = GamePadInput[KeyIndex];
                    GamePadInput[KeyIndex] = 0;
                }

                if (GamePad.GetState(PlayerIndex.One).Buttons.Start == ButtonState.Pressed)
                {
                    Mode = 5;
                    KeyIndex = 3;
                    OldButton = GamePadInput[KeyIndex];
                    GamePadInput[KeyIndex] = 0;
                }

                if (GamePad.GetState(PlayerIndex.One).DPad.Right == ButtonState.Pressed)
                {
                    Mode = 5;
                    KeyIndex = 4;
                    OldButton = GamePadInput[KeyIndex];
                    GamePadInput[KeyIndex] = 0;
                }

                if (GamePad.GetState(PlayerIndex.One).DPad.Left == ButtonState.Pressed)
                {
                    Mode = 5;
                    KeyIndex = 5;
                    OldButton = GamePadInput[KeyIndex];
                    GamePadInput[KeyIndex] = 0;
                }

                if (GamePad.GetState(PlayerIndex.One).DPad.Up == ButtonState.Pressed)
                {
                    Mode = 5;
                    KeyIndex = 6;
                    OldButton = GamePadInput[KeyIndex];
                    GamePadInput[KeyIndex] = 0;
                }

                if (GamePad.GetState(PlayerIndex.One).DPad.Down == ButtonState.Pressed)
                {
                    Mode = 5;
                    KeyIndex = 7;
                    OldButton = GamePadInput[KeyIndex];
                    GamePadInput[KeyIndex] = 0;
                }

                if (GamePad.GetState(PlayerIndex.One).Buttons.RightShoulder == ButtonState.Pressed)
                {
                    Mode = 5;
                    KeyIndex = 8;
                    OldButton = GamePadInput[KeyIndex];
                    GamePadInput[KeyIndex] = 0;
                }

                if (GamePad.GetState(PlayerIndex.One).Buttons.LeftShoulder == ButtonState.Pressed)
                {
                    Mode = 5;
                    KeyIndex = 9;
                    OldButton = GamePadInput[KeyIndex];
                    GamePadInput[KeyIndex] = 0;
                }

                if (GamePad.GetState(PlayerIndex.One).Buttons.Y == ButtonState.Pressed)
                {
                    Mode = 5;
                    KeyIndex = 10;
                    OldButton = GamePadInput[KeyIndex];
                    GamePadInput[KeyIndex] = 0;
                }

                if (GamePad.GetState(PlayerIndex.One).Buttons.X == ButtonState.Pressed)
                {
                    Mode = 5;
                    KeyIndex = 11;
                    OldButton = GamePadInput[KeyIndex];
                    GamePadInput[KeyIndex] = 0;
                }

                if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                {
                    Mode = 0;
                    WaitForKeyUp = true;
                    UpKey = Keys.Escape;
                    SaveConfig();
                }
            }
            else
            {
                if (Keyboard.GetState().IsKeyUp(UpKey))
                {
                    WaitForKeyUp = false;
                }
            }
        }

        private void ShowMain()
        {
            if (debug)
            {
                DrawString(8, 8, "GamePad   : " + useGamePad, Microsoft.Xna.Framework.Color.White);
                DrawString(8, 16, "IPAddress : " + IPAddress, Microsoft.Xna.Framework.Color.White);
                DrawString(8, 24, "Buttons   : " + oldbuttons.ToString("X8"), Microsoft.Xna.Framework.Color.White);
                DrawString(8, 32, "Touch     : " + oldtouch.ToString("X8"), Microsoft.Xna.Framework.Color.White);
                DrawString(8, 40, "CPad      : " + oldcpad.ToString("X8"), Microsoft.Xna.Framework.Color.White);

                //int mousex = Mouse.GetState().Position.X;
                //int mousey = Mouse.GetState().Position.Y;
                int mousex = MainForm.MX;
                int mousey = MainForm.MY;
                if (oldtouch == 0x2000000)
                {
                    if ((GamePad.GetState(PlayerIndex.One).ThumbSticks.Right.X == 0.0) && (GamePad.GetState(PlayerIndex.One).ThumbSticks.Right.Y == 0.0))
                    {
                        if (MainForm.IsUsingMouse)
                        {
                            spriteBatch.Draw(Cursor, new Microsoft.Xna.Framework.Rectangle(mousex - 1, mousey - 1, 3, 3), Microsoft.Xna.Framework.Color.Red);
                        }
                    }
                    else
                    {
                        int stickx = (int)Math.Round(159.5 + (GamePad.GetState(PlayerIndex.One).ThumbSticks.Right.X * 159.5));
                        int sticky = (int)Math.Round(119.5 + (GamePad.GetState(PlayerIndex.One).ThumbSticks.Right.Y * 119.5));
                        spriteBatch.Draw(Cursor, new Microsoft.Xna.Framework.Rectangle(stickx - 1, sticky - 1, 3, 3), Microsoft.Xna.Framework.Color.Red);
                    }
                }
                else
                {
                    int touchx = (int)Math.Round(((double)(oldtouch & 0xFFF) / 0xFFF) * 319);
                    int touchy = (int)Math.Round(((double)((oldtouch >> 0x0C) & 0xFFF) / 0xFFF) * 239);
                    spriteBatch.Draw(Cursor, new Microsoft.Xna.Framework.Rectangle(touchx - 1, touchy - 1, 3, 3), Microsoft.Xna.Framework.Color.Green);
                }

                int cpadx = (int)Math.Round(((double)(oldcpad & 0xFFF) / 0xFFF) * 319);
                int cpady = (int)Math.Round(239 - (((double)((oldcpad >> 0x0C) & 0xFFF) / 0xFFF) * 239));
                spriteBatch.Draw(Cursor, new Microsoft.Xna.Framework.Rectangle(cpadx - 1, cpady - 1, 3, 3), Microsoft.Xna.Framework.Color.Blue);
            }
        }

        private void ShowIPInput()
        {
            DrawString(0, 0, "IP Address: " + IPAddress, Microsoft.Xna.Framework.Color.White);
        }

        private void ShowKeyboardInput()
        {
            DrawString(68, 28, "3DS        : Keyboard", Microsoft.Xna.Framework.Color.White);

            DrawString(68, 44, "DPad Up    : " + KeyboardInput[6], Microsoft.Xna.Framework.Color.White);
            DrawString(68, 52, "DPad Down  : " + KeyboardInput[7], Microsoft.Xna.Framework.Color.White);
            DrawString(68, 60, "DPad Left  : " + KeyboardInput[5], Microsoft.Xna.Framework.Color.White);
            DrawString(68, 68, "DPad Right : " + KeyboardInput[4], Microsoft.Xna.Framework.Color.White);

            DrawString(68, 84, "CPad Up    : " + KeyboardInput[14], Microsoft.Xna.Framework.Color.White);
            DrawString(68, 92, "CPad Down  : " + KeyboardInput[15], Microsoft.Xna.Framework.Color.White);
            DrawString(68, 100, "CPad Left  : " + KeyboardInput[13], Microsoft.Xna.Framework.Color.White);
            DrawString(68, 108, "CPad Right : " + KeyboardInput[12], Microsoft.Xna.Framework.Color.White);


            DrawString(68, 124, "A          : " + KeyboardInput[0], Microsoft.Xna.Framework.Color.White);
            DrawString(68, 132, "B          : " + KeyboardInput[1], Microsoft.Xna.Framework.Color.White);
            DrawString(68, 140, "Y          : " + KeyboardInput[11], Microsoft.Xna.Framework.Color.White);
            DrawString(68, 148, "X          : " + KeyboardInput[10], Microsoft.Xna.Framework.Color.White);

            DrawString(68, 164, "L          : " + KeyboardInput[9], Microsoft.Xna.Framework.Color.White);
            DrawString(68, 172, "R          : " + KeyboardInput[8], Microsoft.Xna.Framework.Color.White);
            DrawString(68, 180, "Start      : " + KeyboardInput[3], Microsoft.Xna.Framework.Color.White);
            DrawString(68, 188, "Select     : " + KeyboardInput[2], Microsoft.Xna.Framework.Color.White);
        }

        private void ShowGamePadInput()
        {
            DrawString(68, 28, "Controller : 3DS", Microsoft.Xna.Framework.Color.White);
            DrawString(68, 44, "DPad Up    : " + GetButtonNameFromValue(GamePadInput[6]), Microsoft.Xna.Framework.Color.White);
            DrawString(68, 52, "DPad Down  : " + GetButtonNameFromValue(GamePadInput[7]), Microsoft.Xna.Framework.Color.White);
            DrawString(68, 60, "DPad Left  : " + GetButtonNameFromValue(GamePadInput[5]), Microsoft.Xna.Framework.Color.White);
            DrawString(68, 68, "DPad Right : " + GetButtonNameFromValue(GamePadInput[4]), Microsoft.Xna.Framework.Color.White);

            DrawString(68, 84, "Y Axis+    : CPad Up", Microsoft.Xna.Framework.Color.Gray);
            DrawString(68, 92, "Y Axis-    : CPad Down", Microsoft.Xna.Framework.Color.Gray);
            DrawString(68, 100, "X Axis+    : CPad Left", Microsoft.Xna.Framework.Color.Gray);
            DrawString(68, 108, "X Axis-    : CPad Right", Microsoft.Xna.Framework.Color.Gray);


            DrawString(68, 124, "B          : " + GetButtonNameFromValue(GamePadInput[0]), Microsoft.Xna.Framework.Color.White);
            DrawString(68, 132, "A          : " + GetButtonNameFromValue(GamePadInput[1]), Microsoft.Xna.Framework.Color.White);
            DrawString(68, 140, "X          : " + GetButtonNameFromValue(GamePadInput[11]), Microsoft.Xna.Framework.Color.White);
            DrawString(68, 148, "Y          : " + GetButtonNameFromValue(GamePadInput[10]), Microsoft.Xna.Framework.Color.White);

            DrawString(68, 164, "LB         : " + GetButtonNameFromValue(GamePadInput[9]), Microsoft.Xna.Framework.Color.White);
            DrawString(68, 172, "RB         : " + GetButtonNameFromValue(GamePadInput[8]), Microsoft.Xna.Framework.Color.White);
            DrawString(68, 180, "Start      : " + GetButtonNameFromValue(GamePadInput[3]), Microsoft.Xna.Framework.Color.White);
            DrawString(68, 188, "Back       : " + GetButtonNameFromValue(GamePadInput[2]), Microsoft.Xna.Framework.Color.White);
        }

        private string GetButtonNameFromValue(uint value)
        {
            string result = "None";

            for (int i = 0; i < ButtonNames.Length; i++)
            {
                if ((value >> i) == 0x01)
                {
                    result = ButtonNames[i];
                    break;
                }
            }

            return result;
        }

        private string KeytoText(Keys key)
        {
            string result = "";

            switch (key)
            {
                case Keys.NumPad0:
                case Keys.NumPad1:
                case Keys.NumPad2:
                case Keys.NumPad3:
                case Keys.NumPad4:
                case Keys.NumPad5:
                case Keys.NumPad6:
                case Keys.NumPad7:
                case Keys.NumPad8:
                case Keys.NumPad9:
                    {
                        result = key.ToString().Substring(6);
                    }
                    break;

                case Keys.D0:
                case Keys.D1:
                case Keys.D2:
                case Keys.D3:
                case Keys.D4:
                case Keys.D5:
                case Keys.D6:
                case Keys.D7:
                case Keys.D8:
                case Keys.D9:
                    {
                        result = key.ToString().Substring(1);
                    }
                    break;

                case Keys.Decimal:
                case Keys.OemPeriod:
                    {
                        result = ".";
                    }
                    break;
            }
            return result;
        }

        private void DrawString(int X, int Y, string data, Microsoft.Xna.Framework.Color color)
        {
            int TexX = 0;
            int TexY = 0;
            for (int i = 0; i < data.Length; i++)
            {
                TexX = ((data[i]) & 0x0F) * 0x08;
                TexY = (((data[i]) & 0xF0) >> 0x04) * 0x08;
                spriteBatch.Draw(Font, new Microsoft.Xna.Framework.Rectangle(X, Y, 8, 8), new Microsoft.Xna.Framework.Rectangle(TexX, TexY, 8, 8), color);
                X += 8;
            }
        }

        private bool MouseInWindow(int x, int y)
        {
            bool result = false;
            if (((x >= 0) && (x < 320)) && ((y >= 0) && (y < 240)))
            {
                result = true;
            }
            return result;
        }

        private void ClampValues(ref int x, ref int y)
        {
            if (x < 0)
            {
                x = 0;
            }

            if (y < 0)
            {
                y = 0;
            }

            if (x > 319)
            {
                x = 319;
            }

            if (y > 239)
            {
                y = 239;
            }
        }

        private void TouchInput(ref uint value, ref uint mouseclick, bool cpad)
        {
            int X = MainForm.MX;
            int Y = MainForm.MY;

            if (mouseclick == 0x00)
            {
                if (MouseInWindow(X, Y))
                {
                    mouseclick = 0x01;
                }
                else
                {
                    mouseclick = 0x02;
                }
            }

            if (mouseclick == 0x01)
            {
                ClampValues(ref X, ref Y);
                X = (int)Math.Round(((double)X / 319) * 4095);
                if (cpad)
                {
                    Y = (int)(4095 - Math.Round(((double)Y / 239) * 4095));
                }
                else
                {
                    Y = (int)Math.Round(((double)Y / 239) * 4095);
                }
                value = (uint)X + ((uint)Y << 0x0C) + 0x01000000;
            }
        }
        
        private void SendInput()
        {
            if ((newbuttons != oldbuttons) || (newtouch != oldtouch) || (newcpad != oldcpad))
            {
                oldbuttons = newbuttons;
                oldtouch = newtouch;
                oldcpad = newcpad;

                //Buttons
                data[0x00] = (byte)(oldbuttons & 0xFF);
                data[0x01] = (byte)((oldbuttons >> 0x08) & 0xFF);
                data[0x02] = (byte)((oldbuttons >> 0x10) & 0xFF);
                data[0x03] = (byte)((oldbuttons >> 0x18) & 0xFF);

                //Touch
                data[0x04] = (byte)(oldtouch & 0xFF);
                data[0x05] = (byte)((oldtouch >> 0x08) & 0xFF);
                data[0x06] = (byte)((oldtouch >> 0x10) & 0xFF);
                data[0x07] = (byte)((oldtouch >> 0x18) & 0xFF);

                //CPad
                data[0x08] = (byte)(oldcpad & 0xFF);
                data[0x09] = (byte)((oldcpad >> 0x08) & 0xFF);
                data[0x0A] = (byte)((oldcpad >> 0x10) & 0xFF);
                data[0x0B] = (byte)((oldcpad >> 0x18) & 0xFF);

                CheckConnection();
                if (Program.ir.isConnected)
                {
                    Program.ir.sendWriteMemPacket(0x10DF20, (uint)0x10, data);
                }
            }
        }

        public delegate void logHandler(string msg, string c);
        public event logHandler onLogArrival;
        public void log(string msg, string c = "logger", string s = "")
        {
            if (onLogArrival != null)
            {
                onLogArrival.Invoke(msg, c);
            }
            try
            {
                Program.mainform.BeginInvoke(Program.mainform.delLog, msg, c, s);
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }
    }
}
