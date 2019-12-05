using Newtonsoft.Json;
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
        private readonly ITetrisDrawer tetrisDrawer;

        public OnlineGame(ITetrisDrawer tetrisDrawer)
        {
            this.tetrisDrawer = tetrisDrawer;
            StartServer();
        }

        private void StartServer()
        {
            int port = 8000;
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
            Task.Run(() =>
            {
                while (handler == null && IsClient == false)
                {
                    handler = new ConnectionHandler(Socket.Accept());

                    handler.MessageReceived += ReceiveData;

                    handler.OnDisconnect += ClientDisconnected;

                    System.Diagnostics.Debug.WriteLine("Triggering client connect");
                    OnConnect?.Invoke(this, new SocketEventArgs(handler.Socket));
                }
            });
        }

        private void ClientDisconnected(object sender, SocketEventArgs e)
        {
            handler = null;
            IsClient = false;
            StartServer();
            System.Diagnostics.Debug.WriteLine("Triggering client disconnect");
            OnDisconnect?.Invoke(sender, e);
        }

        private void ReceiveData(object sender, SocketEventArgs args)
        {
            if (args.Socket.Available > 0)
            {
                byte[] buffer = new byte[args.Socket.Available];
                args.Socket.Receive(buffer, 0, args.Socket.Available, SocketFlags.None);
                System.Diagnostics.Debug.WriteLine(string.Join("", buffer.Select((bb) => (char)bb)));
                bool[,] gameBoard = JsonConvert.DeserializeObject<bool[,]>(string.Join("", buffer.Select((bb) => (char)bb)).Split(new string []{ "]]" }, StringSplitOptions.None)[0] + "]]");
                tetrisDrawer.DrawOnlineGame(gameBoard);
            }
        }

        public object GetLastGameState()
        {
            return null;
        }

        public void SendData(object o)
        {
            if (handler != null)
                handler.WriteData(o);
        }

        public void BecomeClient(string ip, int port)
        {
            Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            // This is necessary because locale changes a dot to a comma sometimes
            Socket.Connect(ip.Replace(",","."), port);
            handler = new ConnectionHandler(Socket);
            handler.MessageReceived += ReceiveData;
            handler.OnDisconnect += ClientDisconnected;

            IsClient = true;
        }

        public void Close()
        {
            Socket.Close();
        }
    }
}
