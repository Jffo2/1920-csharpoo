using System;
using System.Collections.Generic;
using Tetris.Logic;

public interface ITetrisDrawer
{ 
    void SetHighscores(List<HighscoreModel> highscores);
    void DisplayGameOver(int finalScore);
    string PromptUsername();
    void Draw(bool[,] gameBoard, Block block, Block NextBlock, int Score);
    void DrawOnlineGame(bool[,] gameBoard);
}
