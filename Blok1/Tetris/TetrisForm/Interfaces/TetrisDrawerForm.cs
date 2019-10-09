using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Tetris.Logic;

namespace TetrisForm.Interfaces
{
    class TetrisDrawerForm : ITetrisDrawer
    {
        private string infoString;
        public PictureBox MainGamePictureBox { get; }
        public PictureBox NextBlockPictureBox { get; }

        public Label InfoLabel { get; }

        public TetrisDrawerForm(PictureBox mainGamePictureBox, PictureBox nextBlockPictureBox, Label infoLabel)
        {
            MainGamePictureBox = mainGamePictureBox;
            NextBlockPictureBox = nextBlockPictureBox;
            InfoLabel = infoLabel;
        }

        public void DisplayGameOver(int finalScore)
        {
            MessageBox.Show("Game over, you scored " + finalScore, "Game Over", MessageBoxButtons.OK, MessageBoxIcon.None);
            DisplayScore(finalScore);
        }

        public void Draw(bool[,] gameBoard, Block block, Block NextBlock, int Score)
        {
            DrawMainGame(gameBoard, block);
            DrawNextBlock(NextBlock);
            DisplayScore(Score);
        }

        private void DisplayScore(int score)
        {
            var action = new Action<int>((s) => InfoLabel.Text = string.Format(infoString, s, TetrisGame.GetLevelFromScore(s)));
            InfoLabel.Invoke(action, score);
        }

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

        public string PromptUsername()
        {
            string name = "";
            InputBox.Show("Highscore", "Enter your name:", ref name);
            return name;
        }

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
    }
}
