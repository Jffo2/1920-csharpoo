using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Tetris.Logic
{
    public class ConnectionHandler
    {
        public event EventHandler<SocketEventArgs> MessageReceived;
        public event EventHandler<SocketEventArgs> OnDisconnect;

        public Socket Socket { get; set; }


        private bool listenForIncoming = false;
        public ConnectionHandler(Socket socket)
        {
            Socket = socket;
            StartListening();
        }

        public void StartListening()
        {
            listenForIncoming = true;
            Task.Run(() =>
            {
                while (listenForIncoming)
                {
                    lock (Socket)
                    {
                        if (!Socket.Connected)
                        {
                            System.Diagnostics.Debug.WriteLine("Detecting disconnect");
                            OnDisconnect?.Invoke(this, new SocketEventArgs(Socket));
                            break;
                        }
                        if (Socket.Available > 0)
                        {
                            System.Diagnostics.Debug.WriteLine("Data is available for reading!");
                            MessageReceived?.Invoke(this, new SocketEventArgs(Socket));
                        }
                    }
                    System.Threading.Thread.Sleep(10);
                }
            });
        }

        public void StopListening()
        {
            listenForIncoming = false;
        }

        public void WriteData(object o)
        {
            try
            {
                Socket.Send(JsonConvert.SerializeObject(o).ToCharArray().Select((val) => (byte)val).ToArray());
            } catch (Exception) { /* Connection was closed */}
        }

        public class SocketEventArgs : EventArgs
        {
            public Socket Socket { get; }
            public SocketEventArgs(Socket s)
            {
                Socket = s;
            }
        }
    }
}