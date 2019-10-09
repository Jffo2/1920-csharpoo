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
            tetrisGame = new TetrisGame(new HighscoreTextfileReader(), new TetrisDrawerForm(MainGamePictureBox, NextBlockPictureBox, InfoLabel), new FormInputManager(Dispatcher.CurrentDispatcher), 10, 20);
            tetrisGame.Start();
        }
    }
}
