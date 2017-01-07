using System;
using System.IO;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using kit_kat;
using System.Threading.Tasks;

namespace ntrbase
{
    class readMemRequest
    {
        public string fileName;
        public bool isCallback;

        public readMemRequest(string fileName_)
        {
            fileName = fileName_;
            isCallback = false;
        }

        public readMemRequest()
        {
            fileName = null;
            isCallback = true;
        }
    };

    public class DataReadyEventArgs : EventArgs
    {
        public uint seq;
        public byte[] data;

        public DataReadyEventArgs(uint seq_, byte[] data_)
        {
            seq = seq_;
            data = data_;
        }
    }

    public class InfoReadyEventArgs : EventArgs
    {
        public string info;

        public InfoReadyEventArgs(string info_)
        {
            info = info_;
        }
    }

    class NTR
    {
        public string host;
        public int port;
        public TcpClient tcp;
        public NetworkStream netStream;
        public Thread packetRecvThread;
        private object syncLock = new object();
        public int heartbeatSendable;
        public event EventHandler<DataReadyEventArgs> DataReady;
        public event EventHandler Connected;
        public bool isConnected = false;
        public event EventHandler<InfoReadyEventArgs> InfoReady;

        protected virtual void OnDataReady(DataReadyEventArgs e)
        {
            DataReady?.Invoke(this, e);
        }

        protected virtual void OnConnected(EventArgs e, bool Stream)
        {
            if(Stream == true)
            {
                Connected?.Invoke(this, e);
                isConnected = true;
            }
        }

        protected virtual void OnInfoReady(InfoReadyEventArgs e)
        {
            InfoReady?.Invoke(this, e);
        }


        uint currentSeq;
        public Dictionary<uint, readMemRequest> pendingReadMem = new Dictionary<uint, readMemRequest>();
        public volatile int progress = -1;


        int readNetworkStream(NetworkStream stream, byte[] buf, int length)
        {
            int index = 0;
            bool useProgress = false;

            if (length > 100000)
            {
                useProgress = true;
            }
            do
            {
                if (useProgress)
                {
                    progress = (int)(((double)(index) / length) * 100);
                }
                int len = stream.Read(buf, index, length - index);
                if (len == 0)
                {
                    return 0;
                }
                index += len;
            }
            while (index < length);
            progress = -1;
            return length;
        }

        void packetRecvThreadStart()
        {
            byte[] buf = new byte[84];
            uint[] args = new uint[16];
            int ret;
            NetworkStream stream = netStream;

            while (true)
            {
                try
                {
                    ret = readNetworkStream(stream, buf, buf.Length);
                    if (ret == 0)
                    {
                        break;
                    }
                    int t = 0;
                    uint magic = BitConverter.ToUInt32(buf, t);
                    t += 4;
                    uint seq = BitConverter.ToUInt32(buf, t);
                    t += 4;
                    uint type = BitConverter.ToUInt32(buf, t);
                    t += 4;
                    uint cmd = BitConverter.ToUInt32(buf, t);
                    for (int i = 0; i < args.Length; i++)
                    {
                        t += 4;
                        args[i] = BitConverter.ToUInt32(buf, t);
                    }
                    t += 4;
                    uint dataLen = BitConverter.ToUInt32(buf, t);
                    if (cmd != 0)
                    {
                        log(string.Format("packet: cmd = {0}, dataLen = {1}", cmd, dataLen));
                    }

                    if (magic != 0x12345678)
                    {
                        log(string.Format("broken protocol: magic = {0}, seq = {1}", magic, seq));
                        break;
                    }

                    if (cmd == 0)
                    {
                        if (dataLen != 0)
                        {
                            byte[] dataBuf = new byte[dataLen];
                            readNetworkStream(stream, dataBuf, dataBuf.Length);
                            string logMsg = Encoding.UTF8.GetString(dataBuf);
                            OnInfoReady(new InfoReadyEventArgs(Encoding.UTF8.GetString(dataBuf)));
                        }
                        lock (syncLock)
                        {
                            heartbeatSendable = 1;
                        }
                        continue;
                    }
                    if (dataLen != 0)
                    {
                        byte[] dataBuf = new byte[dataLen];
                        readNetworkStream(stream, dataBuf, dataBuf.Length);
                        handlePacket(cmd, seq, dataBuf);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    break;
                }
            }
            disconnect(false);
        }

        string byteToHex(byte[] datBuf, int type)
        {
            string r = "";
            for (int i = 0; i < datBuf.Length; i++)
            {
                r += datBuf[i].ToString("X2") + " ";
            }
            return r;
        }

        void handleReadMem(uint seq, byte[] dataBuf)
        {
            readMemRequest requestDetails;
            if (!pendingReadMem.TryGetValue(seq, out requestDetails))
            {
                log("seq not in pending readmems, ignored");
                return;
            }
            pendingReadMem.Remove(seq);

            if (requestDetails.fileName != null)
            {
                string fileName = requestDetails.fileName;
                FileStream fs = new FileStream(fileName, FileMode.Create);
                fs.Write(dataBuf, 0, dataBuf.Length);
                fs.Close();
                log("dump saved into " + fileName + " successfully");
                return;
            }
            else if (requestDetails.isCallback)
            {
                //Copies the data, truncates if necessary
                byte[] dataBufCopy = new byte[dataBuf.Length];
                dataBuf.CopyTo(dataBufCopy, 0);
                DataReadyEventArgs e = new DataReadyEventArgs(seq, dataBufCopy);
                OnDataReady(e);
            }
            else
            {
                log(byteToHex(dataBuf, 0));
            }

        }

        void handlePacket(uint cmd, uint seq, byte[] dataBuf)
        {
            if (cmd == 9)
            {
                handleReadMem(seq, dataBuf);
            }
        }

        public void setServer(string serverHost, int serverPort)
        {
            host = serverHost;
            port = serverPort;
        }
        
        public void connectToServer(bool Stream = true)
        {
            if (tcp != null)
            {
                disconnect();
            }
            
            tcp = new TcpClient();
            tcp.NoDelay = true;
            if (!tcp.ConnectAsync(host, port).Wait(2000))
            {
                if(Stream == true)
                {
                    log("Make sure your using Boot NTR Selector and loading 3.4\n- Wi-Fi Adapter and Router might not be getting a strong enough connection,\n- IP Address could be incorrect (It changes every now and then),\n- 3DS and PC might not be connected to the same Network,\n- 3DS might not ACTUALLY be connected to the Internet\n(Some games disable Wi-Fi when you are in it to allow NFC - Pokemon, Zelda e.t.c).", "logger", "Failed to connect!");
                } else {
                    log("", "logger3", "Failed to connect!");
                }
            }
            else
            {
                if(Stream == true)
                {
                } else
                {
                    log("", "logger3", "Successfully connected...");
                }
                currentSeq = 0;
                netStream = tcp.GetStream();
                heartbeatSendable = 1;
                packetRecvThread = new Thread(new ThreadStart(packetRecvThreadStart));
                packetRecvThread.Start();
                isConnected = true;
                OnConnected(null, Stream);
            }

        }

        public void disconnect(bool waitPacketThread = true)
        {
            try
            {
                if (tcp != null)
                {
                    tcp.Close();
                }
                if (waitPacketThread)
                {
                    if (packetRecvThread != null)
                    {
                        packetRecvThread.Join();
                    }
                }
            }
            catch (Exception ex)
            {
                log(ex.Message);
            }
            tcp = null;
            isConnected = false;
        }

        public void sendPacket(uint type, uint cmd, uint[] args, uint dataLen)
        {
            try
            {
                int t = 0;
                currentSeq += 1000;
                byte[] buf = new byte[84];
                BitConverter.GetBytes(0x12345678).CopyTo(buf, t);
                t += 4;
                BitConverter.GetBytes(currentSeq).CopyTo(buf, t);
                t += 4;
                BitConverter.GetBytes(type).CopyTo(buf, t);
                t += 4;
                BitConverter.GetBytes(cmd).CopyTo(buf, t);
                for (int i = 0; i < 16; i++)
                {
                    t += 4;
                    uint arg = 0;
                    if (args != null)
                    {
                        arg = args[i];
                    }
                    BitConverter.GetBytes(arg).CopyTo(buf, t);
                }
                t += 4;
                BitConverter.GetBytes(dataLen).CopyTo(buf, t);
                netStream.Write(buf, 0, buf.Length);
            }
            catch (Exception e)
            {
                Console.WriteLine("SendPacket: " + e.Message);
            }
        }

        public void sendReadMemPacket(uint addr, uint size, uint pid, string fileName)
        {
            sendEmptyPacket(9, pid, addr, size);
            pendingReadMem.Add(currentSeq, new readMemRequest(fileName));
        }

        public uint sendReadMemPacket(uint addr, uint size, uint pid)
        {
            sendEmptyPacket(9, pid, addr, size);
            pendingReadMem.Add(currentSeq, new readMemRequest());
            return currentSeq;
        }

        public void sendWriteMemPacket(uint addr, uint pid, byte[] buf)
        {
            uint[] args = new uint[16];
            args[0] = pid;
            args[1] = addr;
            args[2] = (uint)buf.Length;
            sendPacket(1, 10, args, args[2]);
            netStream.Write(buf, 0, buf.Length);
        }

        public void sendWriteMemPacketByte(uint addr, uint pid, byte buf)
        {
            uint[] args = new uint[16];
            args[0] = pid;
            args[1] = addr;
            args[2] = 1;
            sendPacket(1, 10, args, args[2]);
            netStream.WriteByte(buf);
        }


        public void sendHeartbeatPacket()
        {
            if (tcp != null)
            {
                lock (syncLock)
                {
                    if (heartbeatSendable == 1)
                    {
                        heartbeatSendable = 0;
                        sendPacket(0, 0, null, 0);
                    }
                }
            }

        }

        public void sendHelloPacket()
        {
            sendPacket(0, 3, null, 0);
        }

        public void sendReloadPacket()
        {
            sendPacket(0, 4, null, 0);
        }

        public void sendEmptyPacket(uint cmd, uint arg0 = 0, uint arg1 = 0, uint arg2 = 0)
        {
            uint[] args = new uint[16];

            args[0] = arg0;
            args[1] = arg1;
            args[2] = arg2;
            sendPacket(0, cmd, args, 0);
        }
        
        public void sendSaveFilePacket(string fileName, byte[] fileData)
        {
            byte[] fileNameBuf = new byte[0x200];
            Encoding.UTF8.GetBytes(fileName).CopyTo(fileNameBuf, 0);
            sendPacket(1, 1, null, (uint)(fileNameBuf.Length + fileData.Length));
            netStream.Write(fileNameBuf, 0, fileNameBuf.Length);
            netStream.Write(fileData, 0, fileData.Length);
        }

        public async Task<bool> waitNTRwrite(uint address, uint data, int pid)
        {
            byte[] command = BitConverter.GetBytes(data);
            sendWriteMemPacket(address, (uint)pid, command);
            int waittimeout;
            for (waittimeout = 0; waittimeout < 10 * 10; waittimeout++)
            {
                Program.mainform.lastlog = "";
                await Task.Delay(100);
                if (Program.mainform.lastlog.Contains("finished"))
                    break;
            }
            if (waittimeout < 10)
                return true;
            else
                return false;
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
                MessageBox.Show(ex.Message);
            }
        }

    }
}
