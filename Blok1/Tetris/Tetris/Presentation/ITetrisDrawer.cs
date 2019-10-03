using System;
using System.Collections.Generic;

public interface ITetrisDrawer
{
    void Draw(bool[,] gameBoard, int rows, int columns, bool[,] block, List<HighscoreModel> highScores);
}
