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
using Tetris.Logic;
using TetrisForm.Interfaces;
using static Tetris.Logic.ConnectionHandler;

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
            Setup();
        }

        private void Setup()
        {

            if (tetrisGame != null)
            {
                tetrisGame.OnConnect -= ClientConnected;
                tetrisGame.OnDisconnect -= ClientDisconnected;
                tetrisGame.Close();
            }
            tetrisGame = new TetrisGame(new HighscoreTextfileReader(), new TetrisDrawerForm(MainGamePictureBox, NextBlockPictureBox, OnlineGamePictureBox, InfoLabel, OnlineScoreLabel), new FormInputManager(Dispatcher.CurrentDispatcher), 10, 20);
            tetrisGame.OnDisconnect += ClientDisconnected;
            tetrisGame.OnConnect += ClientConnected;
        }

        private void ClientConnected(object sender, SocketEventArgs e)
        {
            ConnectButton.Invoke(new Action(() =>
            {
                ConnectButton.Enabled = false;
            }));
        }

        private void ClientDisconnected(object sender, SocketEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Re-enabling the start button");
            ConnectButton.Invoke(new Action(() =>
            {
                ConnectButton.Enabled = true;
            }));
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            tetrisGame.Start();
            StartButton.Enabled = false;
            StopButton.Enabled = true;
            PauseButton.Enabled = true;
        }

        private void StopButton_Click(object sender, EventArgs e)
        {
            tetrisGame?.Stop();
            ClientDisconnected(null, null);
            StartButton.Enabled = true;
            StopButton.Enabled = false;
            PauseButton.Enabled = false;
            Setup();
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

        private void ConnectButton_Click(object sender, EventArgs e)
        {
            try
            {
                string[] ipPortCombination = ConnectionTextBox.Text.Split(':');
                tetrisGame.BecomeClient(ipPortCombination[0], int.Parse(ipPortCombination[1]));
                ConnectButton.Enabled = false;
            } catch
            {
                MessageBox.Show("The IP adres you entered is invalid!");
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (tetrisGame != null)
                tetrisGame.Close();
        }
    }
}
