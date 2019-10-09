using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Threading;
using Tetris.Data;
using TetrisForm.Interfaces;

namespace TetrisForm
{
    public partial class Form1 : Form
    {

        private TetrisGame tetrisGame;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            tetrisGame = new TetrisGame(new HighscoreTextfileReader(), new TetrisDrawerForm(MainGamePictureBox, NextBlockPictureBox, InfoLabel), new FormInputManager(Dispatcher.CurrentDispatcher), 10, 20);
            tetrisGame.Start();
            StartButton.Enabled = false;
            StopButton.Enabled = true;
            PauseButton.Enabled = true;
        }

        private void StopButton_Click(object sender, EventArgs e)
        {
            tetrisGame?.Stop();
            tetrisGame = null;
            StartButton.Enabled = true;
            StopButton.Enabled = false;
            PauseButton.Enabled = false;
        }

        private void PauseButton_Click(object sender, EventArgs e)
        {
            if (PauseButton.Text=="Pause")
            {
                tetrisGame.Stop();
                PauseButton.Text = "Resume";
            }
            else
            {
                tetrisGame.Resume();
                PauseButton.Text = "Pause";
            }
        }
    }
}
