using System;
using System.Collections.Generic;
using Tetris.Logic;

public interface ITetrisDrawer
{
    void Draw(bool[,] gameBoard, Block block, Block NextBlock);
}
