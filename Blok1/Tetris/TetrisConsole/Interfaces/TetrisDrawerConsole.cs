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
        #region Private Variables
        private readonly int rows;
        private readonly int columns;
        private Block previousBlock;
        private bool[,] previousGameBoard;
        #endregion

        /// <summary>
        /// A list of highscores
        /// </summary>
        public List<HighscoreModel> Highscores { get; set; }

        /// <summary>
        /// TetrisDrawerConsole constructor
        /// </summary>
        /// <param name="rows">the rows for the playing field</param>
        /// <param name="columns">the columns for the playing field</param>
        public TetrisDrawerConsole(int rows, int columns)
        {
            this.rows = rows;
            this.columns = columns;

            Init();
        }

        /// <summary>
        /// Handle initialization
        /// </summary>
        private void Init()
        {
            // This code doesn't work on MacOS
            Console.SetWindowSize(columns * 4, rows + 3);


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

        /// <summary>
        /// Draw the list of highscores on the screen
        /// </summary>
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

        /// <summary>
        /// Draw a box around the game field
        /// </summary>
        private void DrawGameContainer()
        {
            // TODO: optimize this
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

        /// <summary>
        /// Draw a box where we'll display the next block
        /// </summary>
        private void DrawNextBlockContainer()
        {
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

        /// <summary>
        /// This is the central method, this should be called in the main game loop
        /// </summary>
        /// <param name="gameBoard">the game board</param>
        /// <param name="block">the block that is currently falling</param>
        /// <param name="nextBlock">the next block that will fall</param>
        /// <param name="score">the score of the player</param>
        public void Draw(bool[,] gameBoard, Block block, Block nextBlock, int score)
        {
            // Clear previous block by painting over it with the background color
            DrawBlock(previousBlock, ConsoleColor.Black);
            DrawBoard(gameBoard);
            DrawBlock(block);
            DrawNextBlock(nextBlock);
            DrawScore(score);
            // Make a deepcopy of the gameboard otherwise it changes whenever the game changes it
            previousGameBoard = Cloner.DeepClone(gameBoard);
            // Make a copy of the block using it's copy constructor
            previousBlock = new Block(block);
        }

        /// <summary>
        /// Draw the score on the screen
        /// </summary>
        /// <param name="score">the score of the player</param>
        private void DrawScore(int score)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.SetCursorPosition(columns + 3, 6);
            Console.Write($"Score: {score}");
            Console.SetCursorPosition(columns + 3, 7);
            Console.Write($"Level: {TetrisGame.GetLevelFromScore(score)}");
        }

        /// <summary>
        /// Draw the nextblock to the side of the playing field
        /// </summary>
        /// <param name="block">the next block</param>
        private void DrawNextBlock(Block block)
        {
            // Clear the nextblockcontainer by drawing it again
            DrawNextBlockContainer();
            block.Column = columns + 1;
            block.Row = 1;
            DrawBlock(block, ConsoleColor.Cyan);
        }

        /// <summary>
        /// Standard method for drawing a block
        /// </summary>
        /// <param name="block">The block to be drawn</param>
        /// <param name="c">The color to be used to draw the block</param>
        /// <param name="character">The character used to draw the block</param>
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

        /// <summary>
        /// Draw the board, using a delta between the previous state to save time since writing to the screen is slow
        /// </summary>
        /// <param name="gameBoard">the gameboard</param>
        private void DrawBoard(bool[,] gameBoard, ConsoleColor color = ConsoleColor.Red)
        {
            Console.ForegroundColor = color;
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

        /// <summary>
        /// Interface method, used to set the highscores
        /// </summary>
        /// <param name="highscores">a list of highscores</param>
        public void SetHighscores(List<HighscoreModel> highscores)
        {
            Highscores = highscores;
            DrawHighscores();
        }

        /// <summary>
        /// Display the game over screen
        /// </summary>
        /// <param name="finalScore">the score the user got</param>
        public void DisplayGameOver(int finalScore)
        {
            Console.Clear();
            Console.SetCursorPosition(Console.WindowWidth / 2 - 4, Console.WindowHeight / 2);
            Console.Write("Game Over");
            Console.ReadLine();
            
        }

        /// <summary>
        /// Prompt the user for his username, to be used when saving his score to the highscores
        /// </summary>
        /// <returns>The username entered by the user</returns>
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
