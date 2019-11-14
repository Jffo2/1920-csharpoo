using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
    private readonly Timer gameLoop;
    private readonly Timer inputTimer;
    private const int INTERVAL = 400;
    private int score;
    private readonly Queue<Block> blockQueue;
    #endregion

    /// <summary>
    /// The score this game
    /// </summary>
    public int Score
    {
        get
        {
            return score;
        }
        private set
        {
            if (value <= 0) { score = 0; }
            else { score = value; }
        }
    }

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

    public bool IsGameOver { get; private set; }

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
        // Check all parameters and throw exceptions where necessary
        // Use 4 as a minimal value because that is the max width or height of one block
        // Use 48 as a maximal value because otherwise the window would not fit the screen
        if (columns < 4 || columns > 48) throw new ArgumentOutOfRangeException("columns", columns, "The value of columns must be between 4 and 48.");
        if (rows < 4 || rows > 48) throw new ArgumentOutOfRangeException("rows", rows, "The value of rows must be between 4 and 48.");
        Rows = rows;
        Columns = columns;

        // Assign parameters to variables
        this.highscoreFetcher = highscoreFetcher ?? throw new ArgumentNullException("highscoreFetcher");
        this.tetrisDrawer = tetrisDrawer ?? throw new ArgumentNullException("tetrisDrawer");
        this.inputManager = inputManager ?? throw new ArgumentNullException("inputManager");

        // Load highscores into the drawing class
        tetrisDrawer.SetHighscores(highscoreFetcher.LoadHighscores());

        // Add keypress listener
        inputManager.KeyPressed += OnKeyPressed;

        // Initialize gameboard
        gameBoard = new bool[rows, columns];

        IsGameOver = false;

        // Initialize timers
        gameLoop = new Timer
        {
            Interval = INTERVAL
        };
        gameLoop.Elapsed += new ElapsedEventHandler(OnTimerTick);

        inputTimer = new Timer
        {
            Interval = 1
        };
        inputTimer.Elapsed += new ElapsedEventHandler(OnInputTimerTick);

        // Initialize queue
        blockQueue = new Queue<Block>();
        PopulateQueue();

        // Create block objects
        CurrentBlock = SpawnBlock();
        NextBlock = SpawnBlock();


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
        // Draw again after a key is pressed
        Draw();
    }

    /// <summary>
    /// Start all timers and prepare for the start of the game
    /// </summary>
    public void Start()
    {
        gameLoop.Start();
        inputTimer.Start();
    }

    /// <summary>
    /// This tells the input class to check for available input and is called
    /// from the inputTimer
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
        return blockQueue.Dequeue();
    }

    /// <summary>
    /// Stop all timers, this can be resumed later with Resume
    /// <see cref="Resume"/>
    /// </summary>
    public void Stop()
    {
        gameLoop.Stop();
        inputTimer.Stop();
    }

    /// <summary>
    /// Used to start again after the Stop method had been called.
    /// <see cref="Stop"/>
    /// </summary>
    public void Resume()
    {
        gameLoop.Start();
        inputTimer.Start();
    }


    /// <summary>
    /// Displays the "game over" text and stops all timers
    /// </summary>
    private void GameOver()
    {
        gameLoop.Stop();
        inputTimer.Stop();

        var highscores = highscoreFetcher.LoadHighscores();
        highscores.Add(new HighscoreModel()
        {
            Name = tetrisDrawer.PromptUsername(),
            Score = Score
        });
        highscoreFetcher.SaveHighscores(highscores);

        tetrisDrawer.DisplayGameOver(Score);
        IsGameOver = true;
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
        int amountRows = 0;
        for (int row = Rows - 1; row >= 0; row--)
        {
            bool rowFull = true;
            for (int column = 0; column < Columns; column++)
            {
                if (!gameBoard[row, column])
                {
                    rowFull = false;
                    break;
                }
            }
            if (rowFull)
            {
                amountRows++;
                ClearRow(row);
                row++;
            }
        }
        // Add multipliers for score by amount of rows cleared and current level
        Score += 10 * amountRows * GetLevelFromScore(Score);
        gameLoop.Interval = INTERVAL - 20 * GetLevelFromScore(Score);
    }

    /// <summary>
    /// Clears the row and moves all row's above it down
    /// </summary>
    /// <param name="row">the row to clear</param>
    private void ClearRow(int row)
    {
        for (int r = row; r >= 0; r--)
        {
            for (int c = 0; c < Columns; c++)
            {
                // If it's the top row, add an empty one
                if (r == 0)
                {
                    gameBoard[r, c] = false;
                }
                // Shift row one place down
                else
                {
                    gameBoard[r, c] = gameBoard[r - 1, c];
                }
            }
        }

    }

    /// <summary>
    /// Calculates the current level based on the score of the player
    /// </summary>
    /// <param name="score">The score of the player</param>
    /// <returns>An integer representing the current level, the starting level is 1</returns>
    public static int GetLevelFromScore(int score)
    {
        return Math.Max((int)Math.Log10(score), 1);
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
                tetrisDrawer.Draw(Cloner.DeepClone(gameBoard), new Block(CurrentBlock), new Block(NextBlock), Score);
            }
        }
    }

    private void PopulateQueue()
    {
        while (blockQueue.Count <= 10)
        {
            var b = new Block(0, Columns / 2);
            b.Column -= b.Shape.GetLength(1) / 2;
            blockQueue.Enqueue(b);
        }

        Task t = new Task((Action)delegate
        {
            while (!IsGameOver)
            {
                while (blockQueue.Count <= 10)
                {
                    var b = new Block(0, Columns / 2);
                    b.Column -= b.Shape.GetLength(1) / 2;
                    blockQueue.Enqueue(b);
                }
            }
        });
        t.Start();

    }
}
