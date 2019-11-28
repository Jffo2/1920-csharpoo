using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Tetris.Logic
{
    public abstract class OnlineGame : IOnlineGame
    { 
        public Socket Socket { get; protected set; }

        protected OnlineGame()
        {
            Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            Socket.Bind(new IPEndPoint(IPAddress.Any, 8000));
            Socket.Listen(1);
        }

        public object ReceiveData()
        {
            return null;
        }

        public void SendData(object o)
        {
            throw new NotImplementedException();
        }

        public void BecomeClient(String ip, int port)
        {

        }

        public void Close()
        {
            Socket.Close();
        }
    }
}
