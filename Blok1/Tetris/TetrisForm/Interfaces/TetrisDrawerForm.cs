using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Tetris.Logic;

namespace TetrisForm.Interfaces
{
    class TetrisDrawerForm : ITetrisDrawer
    {
        #region Private Variables
        private string infoString;
        #endregion

        /// <summary>
        /// The picturebox to which we will draw the game
        /// </summary>
        public PictureBox MainGamePictureBox { get; }

        /// <summary>
        /// The picturebox in which we will draw the next block
        /// </summary>
        public PictureBox NextBlockPictureBox { get; }

        /// <summary>
        /// The picturebox in which we will draw the online game
        /// </summary>
        public PictureBox OnlineGamePictureBox { get; }

        /// <summary>
        /// The label with the score from the remote game
        /// </summary>
        public Label OnlineScoreLabel { get; }

        /// <summary>
        /// The label containing score and highscore details
        /// </summary>
        public Label InfoLabel { get; }

        /// <summary>
        /// The constructor containing the elements to which we will draw the information
        /// </summary>
        /// <param name="mainGamePictureBox">the picturebox to which we will draw the game</param>
        /// <param name="nextBlockPictureBox">the picturebox in which we will draw the next block</param>
        /// <param name="infoLabel">the label containing score and highscore details</param>
        public TetrisDrawerForm(PictureBox mainGamePictureBox, PictureBox nextBlockPictureBox, PictureBox onlineGamePictureBox, Label infoLabel, Label onlineScoreLabel)
        {
            MainGamePictureBox = mainGamePictureBox;
            NextBlockPictureBox = nextBlockPictureBox;
            OnlineGamePictureBox = onlineGamePictureBox;
            InfoLabel = infoLabel;
            OnlineScoreLabel = onlineScoreLabel;
        }

        /// <summary>
        /// Display the final score to the user and present him with the game over screen
        /// </summary>
        /// <param name="finalScore">the final score of the user</param>
        public void DisplayGameOver(int finalScore)
        {
            MessageBox.Show("Game over, you scored " + finalScore, "Game Over", MessageBoxButtons.OK, MessageBoxIcon.None);
            DisplayScore(finalScore);
        }

        /// <summary>
        /// The main drawing method that's called from the game loop
        /// </summary>
        /// <param name="gameBoard">the game board</param>
        /// <param name="block">the block that's falling</param>
        /// <param name="NextBlock">the next block that'll fall</param>
        /// <param name="Score">the score of the user</param>
        public void Draw(bool[,] gameBoard, Block block, Block NextBlock, int Score)
        {
            DrawMainGame(gameBoard, block);
            DrawNextBlock(NextBlock);
            DisplayScore(Score);
        }

        /// <summary>
        /// Display the current score to the screen
        /// </summary>
        /// <param name="score">the current score</param>
        private void DisplayScore(int score)
        {
            var action = new Action<int>((s) => InfoLabel.Text = string.Format(infoString, s, TetrisGame.GetLevelFromScore(s)));
            InfoLabel.Invoke(action, score);
        }

        /// <summary>
        /// Draw the next block to the screen
        /// </summary>
        /// <param name="nextBlock">the next block that will fall</param>
        private void DrawNextBlock(Block nextBlock)
        {
            var width = NextBlockPictureBox.Width;
            var height = NextBlockPictureBox.Height;
            var bmp = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                Brush b = new SolidBrush(Color.Gray);
                g.FillRectangle(b, new Rectangle(0, 0, width - 2, height - 2));
                nextBlock.Column = 1;
                nextBlock.Row = 1;
                DrawBlock(g, 5, 6, nextBlock, NextBlockPictureBox);
            }
            NextBlockPictureBox.Image = bmp;
        }

        /// <summary>
        /// This will draw the gameboard to the screen as well as the block that's falling
        /// </summary>
        /// <param name="gameBoard">the gameboard</param>
        /// <param name="block">the block that's falling</param>
        private void DrawMainGame(bool[,] gameBoard, Block block)
        {
            var width = MainGamePictureBox.Width;
            var height = MainGamePictureBox.Height;
            var bmp = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                Brush b = new SolidBrush(Color.White);
                g.FillRectangle(b, new Rectangle(0, 0, width - 2, height - 2));
                DrawGameBoard(g, gameBoard);
                DrawBlock(g, gameBoard.GetLength(0), gameBoard.GetLength(1), block, MainGamePictureBox);
            }
            MainGamePictureBox.Image = bmp;
        }

        /// <summary>
        /// Draw a block to the screen
        /// </summary>
        /// <param name="g">the graphics to draw</param>
        /// <param name="rows">the amount of rows the canvas should be divided into</param>
        /// <param name="columns">the amount of columns the canvas should be divided into</param>
        /// <param name="b">the block to draw</param>
        /// <param name="p">the picturebox to calculate the canvas size from</param>
        private void DrawBlock(Graphics g, int rows, int columns, Block b, PictureBox p)
        {
            var columnOffset = b.Column;
            var rowOffset = b.Row;
            var rowHeight = p.Height / rows;
            var columnWidth = p.Width / columns;
            for (int r = 0; r < b.Shape.GetLength(0); r++)
            {
                for (int c = 0; c < b.Shape.GetLength(1); c++)
                {
                    if (b.Shape[r, c])
                        g.FillRectangle(new SolidBrush(Color.Black), new Rectangle(columnWidth * (c + columnOffset), rowHeight * (r + rowOffset), columnWidth, rowHeight));
                }
            }
        }

        /// <summary>
        /// Draw the game board to the screen
        /// </summary>
        /// <param name="g">the graphics used to draw</param>
        /// <param name="gameBoard">the game board</param>
        private void DrawGameBoard(Graphics g, bool[,] gameBoard)
        {
            var rows = gameBoard.GetLength(0);
            var columns = gameBoard.GetLength(1);
            var rowHeight = MainGamePictureBox.Height / rows;
            var columnWidth = MainGamePictureBox.Width / columns;
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < columns; c++)
                {
                    if (gameBoard[r, c])
                        g.FillRectangle(new SolidBrush(Color.Red), new Rectangle(columnWidth * c, rowHeight * r, columnWidth, rowHeight));
                }
            }
        }

        /// <summary>
        /// Prompt for the user name used in the highscores
        /// </summary>
        /// <returns></returns>
        public string PromptUsername()
        {
            string name = "";
            InputBox.Show("Highscore", "Enter your name:", ref name);
            return name;
        }

        /// <summary>
        /// Set the highscores to be drawn to the screen
        /// </summary>
        /// <param name="highscores">the highscores to be drawn</param>
        public void SetHighscores(List<HighscoreModel> highscores)
        {
            infoString = "Score: {0}\r\nLevel: {1}\r\n\r\n\r\n";
            int index = 1;
            foreach (HighscoreModel model in highscores)
            {
                infoString += $"{index}. {model.Name} scored {model.Score}\r\n";
                index++;
            }
        }

        /// <summary>
        /// Draw the game received from online data
        /// </summary>
        /// <param name="gameBoard">the gameboard of the remote game</param>
        /// <param name="score">the score of the remote game</param>
        public void DrawOnlineGame(bool[,] gameBoard, int score)
        {
            var width = OnlineGamePictureBox.Width;
            var height = OnlineGamePictureBox.Height;

            var bmp = new Bitmap(width, height);

            using (Graphics g = Graphics.FromImage(bmp))
            {
                Brush b = new SolidBrush(Color.White);
                g.FillRectangle(b, new Rectangle(0, 0, width - 2, height - 2));
                DrawGameBoard(g, gameBoard);
            }

            OnlineGamePictureBox.Image = bmp;
            OnlineScoreLabel.Invoke(new Action(() =>
            {
                OnlineScoreLabel.Text = "" + score;
            }));
        }
    }
}
