using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using static Tetris.Logic.ConnectionHandler;

namespace Tetris.Logic
{
    public abstract class OnlineGame : IOnlineGame
    {
        public Socket Socket { get; protected set; }
        public bool IsClient { get; private set; }

        public virtual event EventHandler<SocketEventArgs> OnDisconnect;
        public virtual event EventHandler<SocketEventArgs> OnConnect;

        private ConnectionHandler handler;

        /// <summary>
        /// Constructor
        /// </summary>
        public OnlineGame()
        {
            StartServer();
        }

        /// <summary>
        /// Start listening for incoming connections
        /// </summary>
        private void StartServer()
        {
            InitializeSocket();
            // Create a task that will run and search for connections
            Task.Run(() =>
            {
                if (handler == null && IsClient == false)
                {
                    try
                    {
                        handler = new ConnectionHandler(Socket.Accept());

                        handler.MessageReceived += ReceiveData;

                        handler.OnDisconnect += ClientDisconnected;

                        System.Diagnostics.Debug.WriteLine("Triggering client connect");
                        OnConnect?.Invoke(this, new SocketEventArgs(handler.Socket));
                    }
                    catch {
                        Close();
                    }
                }
            });
        }

        /// <summary>
        /// Initialize the socket
        /// </summary>
        private void InitializeSocket()
        {
            int port = 8000;
            // Find a free port
            while (Socket == null)
            {

                try
                {
                    Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    Socket.Bind(new IPEndPoint(IPAddress.Any, port));
                    System.Diagnostics.Debug.WriteLine("Bound to port: " + port);
                    Socket.Listen(1);
                    IsClient = false;
                }
                catch
                {
                    port++;
                    Socket = null;
                }
            }
        }

        /// <summary>
        /// Event listener for the disconnected method
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClientDisconnected(object sender, SocketEventArgs e)
        {
            Close();
            Socket = null;
            handler = null;
            IsClient = false;
            StartServer();
            System.Diagnostics.Debug.WriteLine("Triggering client disconnect");
            OnDisconnect?.Invoke(sender, e);
        }

        /// <summary>
        /// Receive data event listener
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private async void ReceiveData(object sender, SocketEventArgs args)
        {
            if (args.Socket.Available > 0)
            {
                string buffer = await ReadToBuffer(args.Socket);
                if (buffer != "")
                {
                    // Sometimes two messages have been sent before the buffer is read, so we split it
                    string[] bursts = buffer.Split((char)4);
                    foreach (string singleBurst in bursts)
                    {
                        try
                        {
                            string[] tuple = singleBurst.Split(new string[] { "\a\a\a" }, StringSplitOptions.None);
                            string command = tuple[0];
                            string obj = tuple[1];
                            ProcessData(command, obj);
                        }
                        catch {/* If only half of the next message was received this will be triggered, however no action is required */ }
                    }
                }
            }
        }

        /// <summary>
        /// An abstract method for processing the command
        /// </summary>
        /// <param name="command">the command as a string</param>
        /// <param name="obj">the object as a string, usually in json notation</param>
        protected abstract void ProcessData(string command, string obj);

        /// <summary>
        /// Read a buffer async from the socket
        /// </summary>
        /// <param name="socket">the socket to read from</param>
        /// <returns>a string with the contents of the buffer</returns>
        private Task<string> ReadToBuffer(Socket socket)
        {
            return Task<string>.Run(() =>
            {
                try
                {
                    byte[] buffer = new byte[socket.Available];
                    socket.Receive(buffer, 0, socket.Available, SocketFlags.None);
                    return string.Join("", buffer.Select((bb) => (char)bb));
                }
                catch { 
                    /* this can happen when another thread reads the buffer first so  it is empty when this one reads it */ 
                    return ""; 
                }
            });
        }

        /// <summary>
        /// Send data
        /// </summary>
        /// <param name="command">the command for the data</param>
        /// <param name="o">the data as an object, it must be serializable</param>
        public void SendData(string command, object o)
        {
            if (handler != null)
                handler.WriteData(command, o);
        }

        /// <summary>
        /// Have the game become the client
        /// </summary>
        /// <param name="ip">the ip of the game that will be server</param>
        /// <param name="port">the port of the game that will be server</param>
        public void BecomeClient(string ip, int port)
        {
            Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            // This is necessary because locale changes a dot to a comma sometimes
            Socket.Connect(ip.Replace(",", "."), port);
            handler = new ConnectionHandler(Socket);
            handler.MessageReceived += ReceiveData;
            handler.OnDisconnect += ClientDisconnected;

            IsClient = true;
        }

        /// <summary>
        /// Close all connections and sockets
        /// </summary>
        public void Close()
        {
            if (Socket != null)
                Socket.Close();
            if (handler!=null && handler.Socket != null)
                handler.Socket.Close();
        }
    }
}
