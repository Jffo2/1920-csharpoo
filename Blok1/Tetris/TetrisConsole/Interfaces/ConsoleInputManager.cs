using System;
using System.Collections.Generic;
using System.Text;
using Tetris.Data;
using System.Windows.Input;
using System.Threading.Tasks;

namespace TetrisConsole.Interfaces
{
    class ConsoleInputManager : IInputManager
    {
        public event EventHandler<KeyEventArgs> KeyPressed;

        public void CheckInput()
        {
            if (Console.KeyAvailable)
            {
                ConsoleKeyInfo k = Console.ReadKey(true);
                if (k.Key==ConsoleKey.UpArrow)
                {
                    KeyPressed?.Invoke(this, new KeyEventArgs(KeyEventArgs.Keys.RotateL));
                }
                else if (k.Key==ConsoleKey.DownArrow)
                {
                    KeyPressed?.Invoke(this, new KeyEventArgs(KeyEventArgs.Keys.Down));
                }
                else if (k.Key == ConsoleKey.LeftArrow)
                {
                    KeyPressed?.Invoke(this, new KeyEventArgs(KeyEventArgs.Keys.Left));
                }
                else if (k.Key == ConsoleKey.RightArrow)
                {
                    KeyPressed?.Invoke(this, new KeyEventArgs(KeyEventArgs.Keys.Right));
                }
                else if (k.Key==ConsoleKey.Escape)
                {
                    KeyPressed?.Invoke(this, new KeyEventArgs(KeyEventArgs.Keys.Exit));
                }
            }
        }
    }
}
