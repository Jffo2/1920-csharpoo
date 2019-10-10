using System;
using System.Collections.Generic;
using System.Windows.Input;
using System.Windows.Threading;
using Tetris.Data;

namespace TetrisForm.Interfaces
{
    class FormInputManager : IInputManager
    {
        private readonly Dispatcher dispatcher;
        private Key? previousKey;
        private long millis;
        private const int DELAY = 200;
        private readonly Dictionary<Key, Tetris.Data.KeyEventArgs.Keys> keyMap = new Dictionary<Key, Tetris.Data.KeyEventArgs.Keys>()
        {
            {Key.Up, Tetris.Data.KeyEventArgs.Keys.RotateL },
            {Key.Down, Tetris.Data.KeyEventArgs.Keys.Down },
            {Key.Left, Tetris.Data.KeyEventArgs.Keys.Left },
            {Key.Right, Tetris.Data.KeyEventArgs.Keys.Right },
            {Key.Escape, Tetris.Data.KeyEventArgs.Keys.Exit }
        };

        public FormInputManager(Dispatcher d)
        {
            dispatcher = d;
        }

        public event EventHandler<Tetris.Data.KeyEventArgs> KeyPressed;
        public void CheckInput()
        {
            if (previousKey != null)
            {
                dispatcher.Invoke(DispatcherPriority.Normal, (Action)delegate
                {
                    if (Keyboard.IsKeyUp((Key)previousKey))
                    {
                        previousKey = null;
                        millis = 0;
                    }
                });
            }
            if (DateTimeOffset.Now.ToUnixTimeMilliseconds() - millis > DELAY)
            {
                Key? key = null;
                dispatcher.Invoke(DispatcherPriority.Normal, (Action)delegate
               {
                   if (Keyboard.IsKeyDown(Key.Up))
                       key = Key.Up;
                   else if (Keyboard.IsKeyDown(Key.Down))
                       key = Key.Down;
                   else if (Keyboard.IsKeyDown(Key.Left))
                       key = Key.Left;
                   else if (Keyboard.IsKeyDown(Key.Right))
                       key = Key.Right;
                   else if (Keyboard.IsKeyDown(Key.Escape))
                       key = Key.Escape;
               });
                if (key != null)
                {
                    previousKey = key;
                    millis = DateTimeOffset.Now.ToUnixTimeMilliseconds();
                    KeyPressed?.Invoke(this, new Tetris.Data.KeyEventArgs(keyMap[(Key)key]));
                }
            }
        }
    }
}
