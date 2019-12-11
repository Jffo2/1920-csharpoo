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

        /// <summary>
        /// Start listening for disconnects and incoming messages
        /// </summary>
        public void StartListening()
        {
            listenForIncoming = true;
            Task.Run(() =>
            {
                int counter = 0;
                while (listenForIncoming)
                {
                    if (counter > 100)
                    {
                        // About a second has passed, let's ping the other person to see if we're still connected
                        Ping();
                        counter = 0;
                    }
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
                            MessageReceived?.Invoke(this, new SocketEventArgs(Socket));
                        }
                    }
                    System.Threading.Thread.Sleep(10);
                    counter++;
                }
            });
        }

        private void Ping()
        {
            WriteData("PING", null);
        }

        /// <summary>
        /// Stop listening for incoming messages
        /// </summary>
        public void StopListening()
        {
            listenForIncoming = false;
        }

        /// <summary>
        /// Send data through the socket
        /// </summary>
        /// <param name="command">the command for the receiver</param>
        /// <param name="o">the data for the receiver</param>
        public void WriteData(string command, object o)
        {
            try
            {
                string stringified = (o == null) ? "" : JsonConvert.SerializeObject(o);
                Socket.Send((command + "\a\a\a" + stringified + (char)4).ToCharArray().Select((val) => (byte)val).ToArray());
            }
            catch (Exception) { /* Connection was closed */}
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