using System;
using System.Collections.Generic;

namespace TetrisConsole.Interfaces
{
    class TetrisDrawerConsole : ITetrisDrawer
    {

        public void Draw(bool[,] gameBoard, int rows, int columns, bool[,] block, List<HighscoreModel> highScores)
        {
            Console.CursorVisible = false;
            Console.SetWindowSize(columns*4, rows+2);
            Console.Clear();
            Console.SetCursorPosition(0, 0);
            for (int row = 0; row < rows; row++)
            {
                Console.Write("=");
                for (int column = 0; column < columns; column++)
                {
                    if (row == 0 || row==rows-1) Console.Write("=");
                    else
                    {
                        Console.Write(gameBoard[row, column] ? (char)219 : ' ');
                    }
                }
                Console.Write("=");
                Console.WriteLine();
            }
        }
    }
}
