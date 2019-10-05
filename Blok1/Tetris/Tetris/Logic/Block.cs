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

        public bool[,] Shape { get; private set; }
        private static readonly Random random = new Random();

        public Block(int row, int column)
        {
            Shape = RandomBlockShape();
            Row = row;
            Column = column;
        }

        public Block(Block copy)
        {
            this.Row = copy.Row;
            this.Column = copy.Column;
            this.Shape = copy.Shape;
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

        #region Rotating Block
        public void Rotate()
        {
            bool[,] newShape = new bool[Shape.GetLength(1), Shape.GetLength(0)];
            for (int r = 0; r < Shape.GetLength(0); r++)
            {
                for (int c = 0; c < Shape.GetLength(1); c++)
                {
                    newShape[c, Shape.GetLength(0) - r - 1] = Shape[r, c];
                }
            }
            Shape = newShape;
        }

        public bool CanRotate(bool[,] gameBoard)
        {
            Block tempBlock = new Block(this);
            tempBlock.Rotate();
            return !tempBlock.Collision(gameBoard);
        }

        public bool SafeRotate(bool[,] gameBoard)
        {
            if (CanRotate(gameBoard))
            {
                Rotate();
                return true;
            }
            return false;
        }
        #endregion

        #region Falling Block
        public void Fall()
        {
            Row++;
        }

        public bool SafeFall(bool[,] gameBoard)
        {
            if (CanFall(gameBoard))
            {
                Fall();
                return true;
            }
            return false;
        }

        public bool CanFall(bool[,] gameBoard)
        {
            Block tempBlock = new Block(this);
            tempBlock.Fall();
            return !tempBlock.Collision(gameBoard);
        }
        #endregion

        #region Moving Block Left
        public void MoveLeft()
        {
            Column--;
        }

        public bool CanMoveLeft(bool[,] gameBoard)
        {
            Block tempBlock = new Block(this);
            tempBlock.MoveLeft();
            return !tempBlock.Collision(gameBoard);
        }

        public bool SafeMoveLeft(bool[,] gameBoard)
        {
            if (CanMoveLeft(gameBoard))
            {
                MoveLeft();
                return true;
            }
            return false;
        }
        #endregion

        #region Moving Block Right
        public void MoveRight()
        {
            Column++;
        }

        public bool CanMoveRight(bool[,] gameBoard)
        {
            Block tempBlock = new Block(this);
            tempBlock.MoveRight();
            return !tempBlock.Collision(gameBoard);
        }

        public bool SafeMoveRight(bool[,] gameBoard)
        {
            if (CanMoveRight(gameBoard))
            {
                MoveRight();
                return true;
            }
            return false;
        }
        #endregion

        private bool Collision(bool[,] gameBoard)
        {
            for (int row = Row; row < Row + Shape.GetLength(0); row++)
            {
                for (int column = Column; column < Column + Shape.GetLength(1); column++)
                {
                    // If the rotated block falls outside of the gameBoard bounds, return false
                    if (row >= gameBoard.GetLength(0) || row < 0 || column < 0 || column >= gameBoard.GetLength(1)) return true;
                    // If the rotated block will hit another block then also return false
                    if (Shape[row - Row, column - Column] & gameBoard[row, column]) return true;
                }
            }
            return false;
        }

    }


}
