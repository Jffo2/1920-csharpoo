using System;
using System.Collections.Generic;

namespace Tetris.Logic
{
    public class Block
    {
        // https://tetris.fandom.com/wiki/Tetromino

        #region Statics
        /// <summary>
        /// A list of blockshapes for readability
        /// </summary>
        public enum BlockShape { T, L, S, Z, O, I, J }
        /// <summary>
        /// A map of readable BlockShapes to their actual shape
        /// </summary>
        public readonly static Dictionary<BlockShape, bool[,]> BlockMap = new Dictionary<BlockShape, bool[,]>()
        {
            { BlockShape.T, new bool[,]{{false,true,false},{true,true,true} } },
            {BlockShape.L, new bool[,]{{false,false,true},{true,true,true}} },
            {BlockShape.S, new bool[,]{ {false,true,true },{true,true,false } } },
            {BlockShape.Z, new bool[,]{{true,true,false},{false,true,true}} },
            {BlockShape.O, new bool[,]{ { true, true },{ true, true } } },
            {BlockShape.I, new bool[,]{{true,true,true,true}} },
            {BlockShape.J, new bool[,]{{true,false,false},{true,true,true}} }
        };

        /// <summary>
        /// A Static randomizer
        /// </summary>
        private static readonly Random random = new Random();

        /// <summary>
        /// Generate a random blockshape
        /// </summary>
        /// <returns>A block shape</returns>
        public static bool[,] RandomBlockShape()
        {
            return BlockMap[(BlockShape)Enum.GetValues(typeof(BlockShape)).GetValue(random.Next(Enum.GetValues(typeof(BlockShape)).Length))];
        }
        #endregion

        /// <summary>
        /// The row position of the block
        /// </summary>
        public int Row { get; set; }

        /// <summary>
        /// The column position of the block
        /// </summary>
        public int Column { get; set; }

        /// <summary>
        /// The shape of the block
        /// </summary>
        public bool[,] Shape { get; private set; }

        /// <summary>
        /// Block constructor
        /// </summary>
        /// <param name="row">The row position of the block</param>
        /// <param name="column">The column position of the block</param>
        public Block(int row, int column)
        {
            Shape = RandomBlockShape();
            Row = row;
            Column = column;
        }

        /// <summary>
        /// Blcok copy constructor
        /// </summary>
        /// <param name="copy">The block to copy</param>
        public Block(Block copy)
        {
            this.Row = copy.Row;
            this.Column = copy.Column;
            this.Shape = copy.Shape;
        }        

        #region Rotating Block

        /// <summary>
        /// Rotate the block
        /// </summary>
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

        /// <summary>
        /// Checks if the block can rotate
        /// </summary>
        /// <param name="gameBoard">the gameboard to check against for collisions</param>
        /// <returns>true if the block can rotate, else false</returns>
        public bool CanRotate(bool[,] gameBoard)
        {
            Block tempBlock = new Block(this);
            tempBlock.Rotate();
            return !tempBlock.Collision(gameBoard);
        }

        /// <summary>
        /// Rotate if possible and don't if impossible
        /// </summary>
        /// <param name="gameBoard">the gameboard to check for collisions</param>
        /// <returns>true if the block rotated false else</returns>
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

        /// <summary>
        /// Move the block one position down
        /// </summary>
        public void Fall()
        {
            Row++;
        }

        /// <summary>
        /// Only fall if the block can fall
        /// </summary>
        /// <param name="gameBoard">the gameboard to check against for collisions</param>
        /// <returns>true if the block fell, false else</returns>

        public bool SafeFall(bool[,] gameBoard)
        {
            if (CanFall(gameBoard))
            {
                Fall();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Checks if the block can fall
        /// </summary>
        /// <param name="gameBoard">the gameboard to check against for collisions</param>
        /// <returns>true if the block can fall, false else</returns>
        public bool CanFall(bool[,] gameBoard)
        {
            Block tempBlock = new Block(this);
            tempBlock.Fall();
            return !tempBlock.Collision(gameBoard);
        }
        #endregion

        #region Moving Block Left

        /// <summary>
        /// Move the block left
        /// </summary>
        public void MoveLeft()
        {
            Column--;
        }

        /// <summary>
        /// Checks if the block can move left
        /// </summary>
        /// <param name="gameBoard">the gameboard to check against collisions</param>
        /// <returns>true if the block can be moved left, false else</returns>
        public bool CanMoveLeft(bool[,] gameBoard)
        {
            Block tempBlock = new Block(this);
            tempBlock.MoveLeft();
            return !tempBlock.Collision(gameBoard);
        }

        /// <summary>
        /// Only move the block left if it can be moved
        /// </summary>
        /// <param name="gameBoard">the gameboard to check against collisions</param>
        /// <returns>true if the block moved left, false else</returns>
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

        /// <summary>
        /// Move the block right
        /// </summary>
        public void MoveRight()
        {
            Column++;
        }

        /// <summary>
        /// Checks if the block can be moved right
        /// </summary>
        /// <param name="gameBoard">the gameboard to check against collisions</param>
        /// <returns>true if the block can move right, false else</returns>
        public bool CanMoveRight(bool[,] gameBoard)
        {
            Block tempBlock = new Block(this);
            tempBlock.MoveRight();
            return !tempBlock.Collision(gameBoard);
        }

        /// <summary>
        /// Moves the block right but only if possible without collision
        /// </summary>
        /// <param name="gameBoard">the gameboard to check against collisions</param>
        /// <returns>true if the block moved right, false else</returns>
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

        /// <summary>
        /// Check for a collision on the board with this current block
        /// </summary>
        /// <param name="gameBoard">the gameboard to check for collisions</param>
        /// <returns>true if there was a collision, false else</returns>
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
