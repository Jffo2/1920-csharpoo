using System;
using TetrisConsole.Interfaces;

namespace TetrisConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            TetrisGame game = new TetrisGame(null, new TetrisDrawerConsole(20,10,null), new ConsoleInputManager(), 10, 20);
            game.Start();
            while(1==1) { System.Threading.Thread.Sleep(100); }
        }
    }
}
