using System;

public class TetrisGame
{
    private readonly IHighscoreFetcher highscoreFetcher;
    private readonly ITetrisDrawer tetrisDrawer;

	public TetrisGame(IHighscoreFetcher highscoreFetcher, ITetrisDrawer tetrisDrawer)
	{
        this.highscoreFetcher = highscoreFetcher;
        this.tetrisDrawer = tetrisDrawer;
	}
}
