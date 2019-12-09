using System;
using System.Collections.Generic;
using System.Text;

namespace Tetris.Logic
{
    public interface IOnlineGame
    {
        void SendData(string command, object o);
    }
}
