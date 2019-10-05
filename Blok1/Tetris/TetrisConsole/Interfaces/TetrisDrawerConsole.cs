using System;
using System.Collections.Generic;
using Tetris.Logic;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Tetris.Util;

namespace TetrisConsole.Interfaces
{
    class TetrisDrawerConsole : ITetrisDrawer
    {
        private readonly int rows;
        private readonly int columns;

        private Block previousBlock;
        private bool[,] previousGameBoard;

        public TetrisDrawerConsole(int rows, int columns, List<HighscoreModel> highScores)
        {
            this.rows = rows;
            this.columns = columns;
            Init();
        }

        private void Init()
        {
            Console.SetWindowSize(columns * 4, rows + 3);
            Console.OutputEncoding = Encoding.Unicode;
            Console.CursorVisible = false;
            Console.Clear();
            Console.SetCursorPosition(0, 0);
            // Draw box around game area
            for (int row = 0; row < rows+2; row++)
            {
                Console.Write("|");
                for (int column = 0; column < columns; column++)
                {
                    if (row == 0 || row == rows +1) Console.Write("=");
                    else
                    {
                        Console.Write(' ');
                    }
                }
                Console.Write("|");
                Console.WriteLine();
            }
            // Draw a box where we'll display the next block
            Console.SetCursorPosition(columns + 2,0);
            Console.Write("=====");
            Console.SetCursorPosition(columns + 2,1);
            Console.Write("    |");
            Console.SetCursorPosition(columns + 2,2);
            Console.Write("    |");
            Console.SetCursorPosition(columns + 2,3);
            Console.Write("    |");
            Console.SetCursorPosition(columns + 2,4);
            Console.Write("=====");

            previousGameBoard = new bool[rows, columns];
            previousBlock = new Block(0, 0);
        }

        public void Draw(bool[,] gameBoard, Block block, Block nextBlock)
        {
            // Clear previous block
            DrawBlock(previousBlock, ConsoleColor.Black);
            DrawBoard(gameBoard);
            DrawBlock(block);
            DrawNextBlock(nextBlock);
            previousGameBoard = Cloner.DeepClone(gameBoard);
            previousBlock = new Block(block);
        }

        private void DrawNextBlock(Block block)
        {
            Console.SetCursorPosition(columns + 2, 1);
            Console.Write("    |");
            Console.SetCursorPosition(columns + 2, 2);
            Console.Write("    |");
            Console.SetCursorPosition(columns + 2, 3);
            Console.Write("    |");
            block.Column = columns + 1;
            block.Row = 1;
            DrawBlock(block, ConsoleColor.Cyan);
        }

        private void DrawBlock(Block block, ConsoleColor c = ConsoleColor.White ,char character = '\u2588')
        {
            Console.ForegroundColor = c;
            for (int row = 0; row < block.Shape.GetLength(0); row++)
            {
                for (int column = 0; column < block.Shape.GetLength(1); column++)
                {
                    
                    if (block.Shape[row, column])
                    {
                        Console.SetCursorPosition(block.Column + column + 1, block.Row + row + 1);
                        Console.Write(character);
                    }
                }
            }
        }

        private void DrawBoard(bool[,] gameBoard)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < columns; c++)
                {
                    if (gameBoard[r, c] ^ previousGameBoard[r, c])
                    {
                        Console.SetCursorPosition(c + 1, r + 1);
                        Console.Write(gameBoard[r, c] ? '\u2588' : ' ');
                    }
                }
            }
        }

       
    }
}
