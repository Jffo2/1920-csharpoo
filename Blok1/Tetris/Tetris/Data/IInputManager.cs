﻿using System;

namespace Tetris.Data
{
    public interface IInputManager
    {
        event EventHandler<KeyEventArgs> KeyPressed;
        void CheckInput();
    }
    public class KeyEventArgs : EventArgs
    {
        public enum Keys { RotateL, RotateR, Left, Right, Down, Exit };
        public Keys Key { get; set; }

        public KeyEventArgs(Keys k)
        {
            Key = k;
        }
    }
}
