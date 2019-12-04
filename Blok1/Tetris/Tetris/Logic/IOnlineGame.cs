using System;
using System.Collections.Generic;
using System.Text;

namespace Tetris.Logic
{
    public interface IOnlineGame
    {
        void SendData(object o);
        object GetLastGameState();
    }
}
