using System;
using System.Collections.Generic;
using System.Timers;
using Tetris.Data;
using Tetris.Logic;
using Tetris.Util;

public class TetrisGame
{
    #region Private Variables
    private readonly IHighscoreFetcher highscoreFetcher;
    private readonly ITetrisDrawer tetrisDrawer;
    private readonly IInputManager inputManager;
    private readonly bool[,] gameBoard;
    private Timer gameLoop;
    private Timer inputTimer;
    private const int INTERVAL = 400;
    #endregion

    /// <summary>
    /// The amount of rows of the game field
    /// </summary>
    public int Rows { get; }

    /// <summary>
    /// The amount of columns of the game field
    /// </summary>
    public int Columns { get; }

    /// <summary>
    /// The block that is currently falling
    /// </summary>
    public Block CurrentBlock { get; private set; }

    /// <summary>
    /// The next block
    /// </summary>
    public Block NextBlock { get; private set; }

    /// <summary>
    /// Constructor for the tetris game
    /// </summary>
    /// <param name="highscoreFetcher">A class that can provide highscore data</param>
    /// <param name="tetrisDrawer">An object that will be used to draw the game</param>
    /// <param name="inputManager">An object that will listen for input</param>
    /// <param name="columns">The amount of columns for the actual game</param>
    /// <param name="rows">The amount of rows for the actual game</param>
	public TetrisGame(IHighscoreFetcher highscoreFetcher, ITetrisDrawer tetrisDrawer, IInputManager inputManager, int columns, int rows)
	{
        // TODO: check for null objects
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

    /// <summary>
    /// Event handler from the input class that get's triggered when input is available
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
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

    /// <summary>
    /// Start all timers and prepare for the start of the game
    /// </summary>
    public void Start()
    {
        // TODO: attach handlers in constructor
        CurrentBlock = SpawnBlock();
        NextBlock = SpawnBlock();
        gameLoop.Elapsed += new ElapsedEventHandler(OnTimerTick);
        gameLoop.Start();
        inputTimer.Elapsed += new ElapsedEventHandler(OnInputTimerTick);
        inputTimer.Start();
    }

    /// <summary>
    /// This tells the input class to check for available input
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OnInputTimerTick(object sender, ElapsedEventArgs e)
    {
        inputManager.CheckInput();
    }

    /// <summary>
    /// Generates a random block centered at the top of the screen
    /// </summary>
    /// <returns>A block ready for use</returns>
    private Block SpawnBlock()
    {
        var b = new Block(0, Columns / 2);
        b.Column -= b.Shape.GetLength(1)/2;
        return b;
    }

    /// <summary>
    /// Displays the game over text and stops all timers
    /// </summary>
    private void GameOver()
    {
        // TODO: implement this
        System.Diagnostics.Debug.WriteLine("Game Over");
        gameLoop.Stop();
        inputTimer.Stop();
    }

    /// <summary>
    /// Persists a block onto the game board
    /// </summary>
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

    /// <summary>
    /// This is our main gameloop and get's called every INTERVAL
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <see cref="INTERVAL"/>
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

    /// <summary>
    /// Loops over all rows and clears them if they are full
    /// </summary>
    private void ProcessFullRows()
    {
        // TODO: Add scoring here
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
                row++;
            }
        }
    }

    /// <summary>
    /// Clears the row and moves all row's above it down
    /// </summary>
    /// <param name="row">the row to clear</param>
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

    /// <summary>
    /// Draws the current state to the screen
    /// </summary>
    private void Draw()
    {
        // Unlike what is expected, lock does not lock the object. 
        // Lock just ensures that whatever is inside it's body does not execute while the object between the brackets is in use somewhere else.
        lock (gameBoard)
        {
            lock (CurrentBlock)
            {
                tetrisDrawer.Draw(Cloner.DeepClone(gameBoard), new Block(CurrentBlock), new Block(NextBlock));
            }
        }

    }
}
