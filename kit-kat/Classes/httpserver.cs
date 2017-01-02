// This script is heavilly edited by @initPRAGMA. The original is from rmortega77@yahoo.es
using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Collections;
using System.Web;
using System.Net;
using System.IO;

namespace kit_kat.httpserver
{

    #region httpserver

    /// <summary>
    /// Summary description for MyServer.
    /// </summary>
    public class httpserver : sRequest
    {

        // Folder Variable
        public string Folder;

        #region Main
        public httpserver() : base() {
            Folder = "c:\\www";
        }
        public httpserver(int thePort, string theFolder) : base(thePort)
        {
            Folder = theFolder;
        }
        #endregion
        #region onResponse
        public override void OnResponse(ref HTTPRequestStruct rq, ref HTTPResponseStruct rp)
        {

            // Path
            string path = Folder + rq.URL.Replace("/", @"\").Replace("%20", " ");
            
            // Create a Byte FileStream from the file;
            FileStream input = new FileStream(path, FileMode.Open);

            // Open the stream and read it;
            rp.Headers["Content-type"] = "application / octet - stream";
            rp.Headers["Content-Length"] = input.Length.ToString();
            rp.fs = input;

        }
        #endregion

    }

    #endregion
    #region sRequest

    /// <summary>
    /// Summary description for sHTTPServer.
    /// </summary>
    public abstract class sRequest
    {

        // Variables
        private int port = 8080;
        private TcpListener listener;
        Thread Thread;
        public Hashtable respStatus = new Hashtable();
        public string Name = "CsHTTPServer/1.0.*";
        #region Response Codes
        private void respStatusInit()
        {
            respStatus.Add(200, "200 Ok");
            respStatus.Add(201, "201 Created");
            respStatus.Add(202, "202 Accepted");
            respStatus.Add(204, "204 No Content");
            respStatus.Add(301, "301 Moved Permanently");
            respStatus.Add(302, "302 Redirection");
            respStatus.Add(304, "304 Not Modified");
            respStatus.Add(400, "400 Bad Request");
            respStatus.Add(401, "401 Unauthorized");
            respStatus.Add(403, "403 Forbidden");
            respStatus.Add(404, "404 Not Found");
            respStatus.Add(500, "500 Internal Server Error");
            respStatus.Add(501, "501 Not Implemented");
            respStatus.Add(502, "502 Bad Gateway");
            respStatus.Add(503, "503 Service Unavailable");
        }
        #endregion

        // Functions
        public bool IsAlive { get { return Thread.IsAlive; } }
        public sRequest() { respStatusInit(); }
        public sRequest(int port) { this.port = port; respStatusInit(); }
        public abstract void OnResponse(ref HTTPRequestStruct rq, ref HTTPResponseStruct rp);

        #region Start
        public void Start() { Thread = new Thread(new ThreadStart(Listen)); Thread.Start(); }
        #endregion
        #region Listen
        public void Listen()
        {

            // Variables
            bool done = false;
            IPAddress ipAddress = Dns.GetHostEntry(Dns.GetHostName()).AddressList[0];

            // Listener
#pragma warning disable CS0618 // Type or member is obsolete
            listener = new TcpListener(port);
#pragma warning restore CS0618 // Type or member is obsolete
            listener.Start();
            Console.WriteLine("Listening On: " + port.ToString());

            // Until "Done"...
            while (!done)
            {
                try
                {
                    // Start sHTTPRequest
                    Console.WriteLine("Waiting for connection...");
                    sProcess newRequest = new sProcess(listener.AcceptTcpClient(), this);
                    Thread Thread = new Thread(new ThreadStart(newRequest.Process));
                    Thread.Name = "HTTP Request";
                    Thread.Start();
                }
                catch (Exception)
                {
                    //from time to time this went boom boom. So a nice trycatch stops it.
                }
            }

        }
        #endregion
        #region Stop
        public void Stop() { listener.Stop(); Thread.Abort(); }
        #endregion

    }

    #endregion
    #region sProcess

    #region States
    enum RState { METHOD, URL, URLPARM, URLVALUE, VERSION, HEADERKEY, HEADERVALUE, BODY, OK };
    enum RespState { OK = 200, BAD_REQUEST = 400, NOT_FOUND = 404 }
    #endregion
    #region Structs
    public struct HTTPRequestStruct
    {
        public string Method;
        public string URL;
        public string Version;
        public Hashtable Args;
        public bool Execute;
        public Hashtable Headers;
        public int BodySize;
        public byte[] BodyData;
    }
    public struct HTTPResponseStruct
    {
        public int status;
        public string version;
        public Hashtable Headers;
        public int BodySize;
        public byte[] BodyData;
        public System.IO.FileStream fs;
    }
    #endregion

    /// <summary>
    /// Summary description for sHTTPRequest.
    /// </summary>
    public class sProcess
    {

        // Variables
        private TcpClient client;
        private RState ParserState;
        private HTTPRequestStruct HTTPRequest;
        private HTTPResponseStruct HTTPResponse;
        byte[] myReadBuffer;
        sRequest Parent;

        // Function
        public sProcess(TcpClient client, sRequest Parent) { this.client = client; this.Parent = Parent; HTTPResponse.BodySize = 0; }

        #region Process
        public void Process()
        {

            // Variables
            myReadBuffer = new byte[client.ReceiveBufferSize];
            string myCompleteMessage = "";
            int numberOfBytesRead = 0;
            Console.WriteLine("Connection accepted. Buffer: " + client.ReceiveBufferSize.ToString());
            NetworkStream ns = client.GetStream();
            string hValue = "";
            string hKey = "";

            try
            {

                // binary data buffer index
                int bfndx = 0;

                // Incoming message may be larger than the buffer size.
                do
                {

                    numberOfBytesRead = ns.Read(myReadBuffer, 0, myReadBuffer.Length);
                    myCompleteMessage = string.Concat(myCompleteMessage, Encoding.ASCII.GetString(myReadBuffer, 0, numberOfBytesRead));

                    // read buffer index
                    int ndx = 0;
                    do
                    {
                        switch (ParserState)
                        {
                            case RState.METHOD:
                                if (myReadBuffer[ndx] != ' ') { HTTPRequest.Method += (char)myReadBuffer[ndx++]; }
                                else { ndx++; ParserState = RState.URL; }
                                break;
                            case RState.URL:
                                if (myReadBuffer[ndx] == '?') { ndx++; hKey = ""; HTTPRequest.Execute = true; HTTPRequest.Args = new Hashtable(); ParserState = RState.URLPARM; }
                                else if (myReadBuffer[ndx] != ' ') { HTTPRequest.URL += (char)myReadBuffer[ndx++]; }
                                else { ndx++; HTTPRequest.URL = HttpUtility.UrlDecode(HTTPRequest.URL); ParserState = RState.VERSION; }
                                break;
                            case RState.URLPARM:
                                if (myReadBuffer[ndx] == '=') { ndx++; hValue = ""; ParserState = RState.URLVALUE; }
                                else if (myReadBuffer[ndx] == ' ') { ndx++; HTTPRequest.URL = HttpUtility.UrlDecode(HTTPRequest.URL); ParserState = RState.VERSION; }
                                else { hKey += (char)myReadBuffer[ndx++]; }
                                break;
                            case RState.URLVALUE:
                                if (myReadBuffer[ndx] == '&') { ndx++; hKey = HttpUtility.UrlDecode(hKey); hValue = HttpUtility.UrlDecode(hValue); HTTPRequest.Args[hKey] = HTTPRequest.Args[hKey] != null ? HTTPRequest.Args[hKey] + ", " + hValue : hValue; hKey = ""; ParserState = RState.URLPARM; }
                                else if (myReadBuffer[ndx] == ' ') { ndx++; hKey = HttpUtility.UrlDecode(hKey); hValue = HttpUtility.UrlDecode(hValue); HTTPRequest.Args[hKey] = HTTPRequest.Args[hKey] != null ? HTTPRequest.Args[hKey] + ", " + hValue : hValue; HTTPRequest.URL = HttpUtility.UrlDecode(HTTPRequest.URL); ParserState = RState.VERSION; }
                                else { hValue += (char)myReadBuffer[ndx++]; }
                                break;
                            case RState.VERSION:
                                if (myReadBuffer[ndx] == '\r') { ndx++; }
                                else if (myReadBuffer[ndx] != '\n') { HTTPRequest.Version += (char)myReadBuffer[ndx++]; }
                                else { ndx++; hKey = ""; HTTPRequest.Headers = new Hashtable(); ParserState = RState.HEADERKEY; }
                                break;
                            case RState.HEADERKEY:
                                if (myReadBuffer[ndx] == '\r') { ndx++; }
                                else if (myReadBuffer[ndx] == '\n')
                                {
                                    ndx++;
                                    if (HTTPRequest.Headers["Content-Length"] != null) { HTTPRequest.BodySize = Convert.ToInt32(HTTPRequest.Headers["Content-Length"]); HTTPRequest.BodyData = new byte[HTTPRequest.BodySize]; ParserState = RState.BODY; }
                                    else { ParserState = RState.OK; }
                                }
                                else if (myReadBuffer[ndx] == ':') { ndx++; }
                                else if (myReadBuffer[ndx] != ' ') { hKey += (char)myReadBuffer[ndx++]; }
                                else { ndx++; hValue = ""; ParserState = RState.HEADERVALUE; }
                                break;
                            case RState.HEADERVALUE:
                                if (myReadBuffer[ndx] == '\r') { ndx++; }
                                else if (myReadBuffer[ndx] != '\n') { hValue += (char)myReadBuffer[ndx++]; }
                                else { ndx++; HTTPRequest.Headers.Add(hKey, hValue); hKey = ""; ParserState = RState.HEADERKEY; }
                                break;
                            case RState.BODY:
                                Array.Copy(myReadBuffer, ndx, HTTPRequest.BodyData, bfndx, numberOfBytesRead - ndx);
                                bfndx += numberOfBytesRead - ndx;
                                ndx = numberOfBytesRead;
                                if (HTTPRequest.BodySize <= bfndx) { ParserState = RState.OK; }
                                break;
                        }
                    }
                    while (ndx < numberOfBytesRead);

                }
                while (ns.DataAvailable);

                // Print out the received message to the console.
                Console.WriteLine("You received the following message : \n" + myCompleteMessage);

                HTTPResponse.version = "HTTP/1.1";

                if (ParserState != RState.OK) { HTTPResponse.status = (int)RespState.BAD_REQUEST; }
                else { HTTPResponse.status = (int)RespState.OK; }

                HTTPResponse.Headers = new Hashtable();
                HTTPResponse.Headers.Add("Server", Parent.Name);
                HTTPResponse.Headers.Add("Date", DateTime.Now.ToString("r"));
                Parent.OnResponse(ref HTTPRequest, ref HTTPResponse);

                string HeadersString = HTTPResponse.version + " " + Parent.respStatus[HTTPResponse.status] + "\n";
                foreach (DictionaryEntry Header in HTTPResponse.Headers) { HeadersString += Header.Key + ": " + Header.Value + "\n"; }
                HeadersString += "\n";
                byte[] bHeadersString = Encoding.ASCII.GetBytes(HeadersString);

                // Send headers 
                ns.Write(bHeadersString, 0, bHeadersString.Length);

                // Send body
                if (HTTPResponse.BodyData != null) { ns.Write(HTTPResponse.BodyData, 0, HTTPResponse.BodyData.Length); }

                if (HTTPResponse.fs != null)
                {
                    using (HTTPResponse.fs)
                    {
                        byte[] b = new byte[client.SendBufferSize];
                        int bytesRead;
                        while ((bytesRead = HTTPResponse.fs.Read(b, 0, b.Length)) > 0) { ns.Write(b, 0, bytesRead); }
                        HTTPResponse.fs.Close();
                    }
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            finally
            {
                ns.Close();
                client.Close();
                if (HTTPResponse.fs != null) { HTTPResponse.fs.Close(); }
                Thread.CurrentThread.Abort();
            }

        }
        #endregion

    }

    #endregion

}