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

        public List<HighscoreModel> Highscores { get; set; }

        public TetrisDrawerConsole(int rows, int columns)
        {
            this.rows = rows;
            this.columns = columns;
            Init();
        }

        private void Init()
        {
            //Console.SetWindowSize(columns * 4, rows + 3);
            Console.OutputEncoding = Encoding.UTF8;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.CursorVisible = false;
            Console.Clear();
            Console.SetCursorPosition(0, 0);
            DrawGameContainer();
            DrawNextBlockContainer();
            DrawHighscores();
            previousGameBoard = new bool[rows, columns];
            previousBlock = new Block(0, 0);
        }

        private void DrawHighscores()
        { 
            if (Highscores!=null)
            {
                Console.SetCursorPosition(columns + 4, 9);
                Console.Write("HighScores:");
                int row = 10;
                foreach (HighscoreModel model in Highscores)
                {
                    Console.SetCursorPosition(columns + 4, row);
                    Console.Write($"{row - 9}. {model.Name} scored {model.Score}");
                    row++;
                }
            }
        }

        private void DrawGameContainer()
        {
            // Draw box around game area
            for (int row = 0; row < rows + 2; row++)
            {
                Console.Write("|");
                for (int column = 0; column < columns; column++)
                {
                    if (row == 0 || row == rows + 1) Console.Write("=");
                    else
                    {
                        Console.Write(' ');
                    }
                }
                Console.Write("|");
                Console.WriteLine();
            }
        }

        private void DrawNextBlockContainer()
        {
            // Draw a box where we'll display the next block
            Console.SetCursorPosition(columns + 2, 0);
            Console.Write("=====");
            Console.SetCursorPosition(columns + 2, 1);
            Console.Write("    |");
            Console.SetCursorPosition(columns + 2, 2);
            Console.Write("    |");
            Console.SetCursorPosition(columns + 2, 3);
            Console.Write("    |");
            Console.SetCursorPosition(columns + 2, 4);
            Console.Write("=====");
        }

        public void Draw(bool[,] gameBoard, Block block, Block nextBlock, int score)
        {
            // Clear previous block
            DrawBlock(previousBlock, ConsoleColor.Black);
            DrawBoard(gameBoard);
            DrawBlock(block);
            DrawNextBlock(nextBlock);
            DrawScore(score);
            previousGameBoard = Cloner.DeepClone(gameBoard);
            previousBlock = new Block(block);
        }

        private void DrawScore(int score)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.SetCursorPosition(columns + 3, 6);
            Console.Write($"Score: {score}");
            Console.SetCursorPosition(columns + 3, 7);
            Console.Write($"Level: {TetrisGame.GetLevelFromScore(score)}");
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

        private void DrawBlock(Block block, ConsoleColor c = ConsoleColor.White ,char character = '█')
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

        public void SetHighscores(List<HighscoreModel> highscores)
        {
            Highscores = highscores;
            DrawHighscores();
        }

        public void DisplayGameOver(int finalScore)
        {
            Console.SetCursorPosition(Console.WindowWidth / 2 / 2, Console.WindowHeight / 2);
            Console.Clear();
            Console.Write("Game Over");
        }

        public string PromptUsername()
        { 
            Console.Clear();
            var prompt = "Please enter your username: ";
            Console.SetCursorPosition(Console.WindowWidth / 2 - prompt.Length / 2, Console.WindowHeight/2);
            Console.Write(prompt);
            Console.CursorVisible = true;
            Console.SetCursorPosition(Console.WindowWidth / 2 - prompt.Length / 2, Console.WindowHeight/2+1);
            return Console.ReadLine();
        }
    }
}
