using System;
using System.Collections.Generic;
using Tetris.Logic;
using System.Text;

namespace TetrisConsole.Interfaces
{
    class TetrisDrawerConsole : ITetrisDrawer
    {
        private readonly int rows;
        private readonly int columns;

        public TetrisDrawerConsole(int rows, int columns, List<HighscoreModel> highScores)
        {
            this.rows = rows;
            this.columns = columns;
            Init();
        }

        private void Init()
        {
            Console.OutputEncoding = System.Text.Encoding.Unicode;
            Console.CursorVisible = false;
            Console.SetWindowSize(columns * 4, rows + 3);
            Console.Clear();
            Console.SetCursorPosition(0, 0);
            for (int row = 0; row < rows+2; row++)
            {
                Console.Write("=");
                for (int column = 0; column < columns; column++)
                {
                    if (row == 0 || row == rows +1) Console.Write("=");
                    else
                    {
                        Console.Write(' ');
                    }
                }
                Console.Write("=");
                Console.WriteLine();
            }
        }

        public void Draw(bool[,] gameBoard, Block block)
        {
            DrawBoard(gameBoard);
            DrawBlock(block);
        }

        private void DrawBlock(Block block)
        {
            for (int row = 0; row < block.Shape.GetLength(0); row++)
            {
                Console.SetCursorPosition(block.Column + 1, block.Row + row + 1);
                for (int column = 0; column < block.Shape.GetLength(1); column++)
                {
                    Console.Write(block.Shape[row, column] ? '\u2588' : ' ');
                }

            }
        }

        private void DrawBoard(bool[,] gameBoard)
        {
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < columns; c++)
                {
                    Console.SetCursorPosition(c+1, r+1);
                    Console.Write(gameBoard[r, c] ? '\u2588' : ' ');
                }
            }
        }

        private void Clear()
        {
            for (int r=1;r<rows+1;r++)
            {
                for (int c=1;c<columns+1;c++)
                {
                    Console.SetCursorPosition(c, r);
                    Console.Write(' ');
                }
            }
        }
    }
}
