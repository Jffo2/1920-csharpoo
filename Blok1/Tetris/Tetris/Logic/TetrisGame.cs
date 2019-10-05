using System;
using System.Collections.Generic;
using System.Timers;
using Tetris.Data;
using Tetris.Logic;
using Tetris.Util;

public class TetrisGame
{
    private readonly IHighscoreFetcher highscoreFetcher;
    private readonly ITetrisDrawer tetrisDrawer;
    private readonly IInputManager inputManager;
    private readonly bool[,] gameBoard;
    private Timer gameLoop;
    private Timer inputTimer;
    private const int INTERVAL = 400;

    public int Rows { get; }
    public int Columns { get; }

    public Block CurrentBlock { get; private set; }
    public Block NextBlock { get; private set; }


	public TetrisGame(IHighscoreFetcher highscoreFetcher, ITetrisDrawer tetrisDrawer, IInputManager inputManager, int columns, int rows)
	{
        this.highscoreFetcher = highscoreFetcher;
        this.tetrisDrawer = tetrisDrawer;
        this.inputManager = inputManager;
        inputManager.KeyPressed += OnKeyPressed;
        Rows = rows;
        Columns = columns;
        // TODO: Check rows and columns and if invalid throw an exception
        gameBoard = new bool[rows,columns];
        gameLoop = new Timer
        {
            Interval = INTERVAL
        };
        inputTimer = new Timer
        {
            Interval = 1
        };
    }

    private void OnKeyPressed(object sender, KeyEventArgs args)
    {
        if (args.Key == KeyEventArgs.Keys.RotateL)
        {
            CurrentBlock.SafeRotate(gameBoard);
        }
        if (args.Key == KeyEventArgs.Keys.Left)
        {
            CurrentBlock.SafeMoveLeft(gameBoard);
        }
        if (args.Key == KeyEventArgs.Keys.Right)
        {
            CurrentBlock.SafeMoveRight(gameBoard);
        }
        if (args.Key == KeyEventArgs.Keys.Down)
        {
            CurrentBlock.SafeFall(gameBoard);
        }
        if (args.Key == KeyEventArgs.Keys.Exit)
        {
            GameOver();
        }
        Draw();
    }

    public List<HighscoreModel> LoadHighscores()
    {
        return highscoreFetcher.LoadHighscores();
    }

    public void Start()
    {
        CurrentBlock = SpawnBlock();
        NextBlock = SpawnBlock();
        gameLoop.Elapsed += new ElapsedEventHandler(OnTimerTick);
        gameLoop.Start();
        inputTimer.Elapsed += new ElapsedEventHandler(OnInputTimerTick);
        inputTimer.Start();
    }

    private void OnInputTimerTick(object sender, ElapsedEventArgs e)
    {
        inputManager.CheckInput();
    }

    private Block SpawnBlock()
    {
        var b = new Block(0, Columns / 2);
        b.Column -= b.Shape.GetLength(1)/2;
        return b;
    }

    private void GameOver()
    {
        System.Diagnostics.Debug.WriteLine("Game Over");
        gameLoop.Stop();
        inputTimer.Stop();
    }

    private void AddBlockToBoard()
    {
        for (int row = CurrentBlock.Row; row < CurrentBlock.Row + CurrentBlock.Shape.GetLength(0); row++)
        {
            for (int column = CurrentBlock.Column; column < CurrentBlock.Column + CurrentBlock.Shape.GetLength(1); column++)
            {
                gameBoard[row, column] |= CurrentBlock.Shape[row - CurrentBlock.Row, column - CurrentBlock.Column];
            }
        }
    }

    private void OnTimerTick(object sender, ElapsedEventArgs e)
    {
        if (!CurrentBlock.SafeFall(gameBoard))
        {
            AddBlockToBoard();
            CurrentBlock = NextBlock;
            if (!CurrentBlock.CanFall(gameBoard))
            {
                GameOver();
            }
            NextBlock = SpawnBlock();
        }
        ProcessFullRows();
        Draw();
    }

    private void ProcessFullRows()
    {
        for (int row = Rows-1; row>=0; row--)
        {
            bool rowFull = true;
            for (int column = 0; column<Columns; column++)
            {
                if (!gameBoard[row,column])
                {
                    rowFull = false;
                    break;
                }
            }
            if (rowFull)
            {
                ClearRow(row);
            }
        }
    }

    private void ClearRow(int row)
    {
        for (int r = row; r>=0; r--)
        {
            for (int c = 0; c<Columns; c++)
            {
                if (r==0)
                {
                    gameBoard[r, c] = false;
                }
                else
                {
                    gameBoard[r, c] = gameBoard[r - 1, c];
                }
            }
        }
        
    }

    private void Draw()
    {
        lock (gameBoard)
        {
            lock (CurrentBlock)
            {
                tetrisDrawer.Draw(Cloner.DeepClone(gameBoard), new Block(CurrentBlock), new Block(NextBlock));
            }
        }

    }
}
