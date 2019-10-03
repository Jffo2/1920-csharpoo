using System;
using System.Collections.Generic;

namespace Tetris.Logic
{
    public class Block
    {
        // https://tetris.fandom.com/wiki/Tetromino
        public enum BlockShape { T, L, S, Z, O, I, J }
        public static Dictionary<BlockShape, bool[,]> BlockMap = new Dictionary<BlockShape, bool[,]>()
        {
            { BlockShape.T, new bool[,]{{false,true,false},{true,true,true} } },
            {BlockShape.L, new bool[,]{{false,false,true},{true,true,true}} },
            {BlockShape.S, new bool[,]{ {false,true,true },{true,true,false } } },
            {BlockShape.Z, new bool[,]{{true,true,false},{false,true,true}} },
            {BlockShape.O, new bool[,]{ { true, true },{ true, true } } },
            {BlockShape.I, new bool[,]{{true,true,true,true}} },
            {BlockShape.J, new bool[,]{{true,false,false},{true,true,true}} }
        };
        public int Row { get; set; }
        public int Column { get; set; }

        public bool[,] Shape { get; }
        private static Random random = new Random();

        public Block(int row, int column)
        {
            Shape = RandomBlockShape();
            Row = row;
            Column = column;
        }

        public void UpdatePosition(int row, int column)
        {
            Row = row;
            Column = column;
        }

        public static bool[,] RandomBlockShape()
        {
            return BlockMap[(BlockShape)Enum.GetValues(typeof(BlockShape)).GetValue(random.Next(Enum.GetValues(typeof(BlockShape)).Length))];
        }
    }


}
