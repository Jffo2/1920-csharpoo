using System;
using System.Collections.Generic;
using System.Timers;
using Tetris.Data;

public class TetrisGame
{
    private readonly IHighscoreFetcher highscoreFetcher;
    private readonly ITetrisDrawer tetrisDrawer;
    private readonly IInputManager inputManager;
    private readonly bool[,] gameBoard;
    private Timer gameLoop;
    private const int INTERVAL = 400;

    public int Rows { get; }
    public int Columns { get; }


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
        gameLoop = new Timer();
        gameLoop.Interval = INTERVAL;
	}

    public void OnKeyPressed(object sender, KeyEventArgs args)
    {
        Console.WriteLine(args.Key.ToString());
    }

    public List<HighscoreModel> LoadHighscores()
    {
        return highscoreFetcher.LoadHighscores();
    }

    public void Start()
    {
        gameLoop.Elapsed += new ElapsedEventHandler(OnTimerTick);
        gameLoop.Start();
    }

    public void OnTimerTick(object sender, ElapsedEventArgs e)
    {
        inputManager.CheckInput();
        tetrisDrawer.Draw(gameBoard, new Tetris.Logic.Block(5,2));
    }
}
