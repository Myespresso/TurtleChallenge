using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    internal sealed class Board
    {
        internal int Width { get; private set; }
        internal int Height { get; private set; }
        Board(int width, int height)
        {
            this.Width = width;
            this.Height = height;
            this.mines = new HashSet<Point>();

        }
        HashSet<Point> mines;

        public static Board Create(int width, int height)
        {
            if (width <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(width), width, " A board's width can not be less than Zero");
            }
            if (height <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(height), height, " A board's height can not be less than Zero");
            }
            return new Board(width, height);

        }

        Point _ExitPoint;
        internal Point ExitPoint
        {
            get
            {
                return _ExitPoint;
            }
            set
            {
                bool isValidForUse = CanBeSpecialPoint(value);
                if (isValidForUse)
                {
                    this._ExitPoint = value;
                }
                else
                {
                    throw new ArgumentOutOfRangeException(nameof(ExitPoint), value, " Exit point cannot be out of Boarder's boundry!, the board coordination is Zero Base.");

                }
            }
        }

        internal void LayMine(Point point)
        {
            bool isValidForUse = CanBeSpecialPoint(point);
            if (isValidForUse)
            {
                mines.Add(point);
            }
            else
            {
                var ctype = GetCellType(point);
                string msg = "The point already is a Mine";
                if (ctype == CellType.Exit)
                {
                    msg = "The point  already is ExitPoint";
                }
                throw new ArgumentOutOfRangeException(nameof(point), point, msg);

            }
        }
      
      

        private bool CanBeSpecialPoint(Point point)
        {
            bool isInBoard = IsInBoarder(point);
            bool isExitPoint = false;
            bool isMine = false;
            if (isInBoard)
            {
                isMine = mines.Contains(point);
                isExitPoint = this.ExitPoint == point;
            }

            bool resut = isInBoard && !isExitPoint && !isMine;
            return resut;
        }

        private bool IsInBoarder(Point point)
        {
            return point.X < Width && point.Y < Height && (point.X >= 0 && point.Y >= 0);
        }

        internal CellType GetCellType(Point point)
        {
            CellType type = CellType.None;
            if (IsInBoarder(point))
            {
                if (point == ExitPoint)
                {
                    type = CellType.Exit;
                }
                else if (mines.Contains(point))
                {
                    type = CellType.Mine;
                }
                return type;
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(point), point, $"The point X:{point.X} Y:{point.Y} is out of the board, the board coordination is Zero Base!");
            }

        }
        internal Point GetNextCell(Point point, Direction direction)
        {
            Point nextPoint = new Point();
            switch (direction)
            {
                case Direction.North:
                    nextPoint = new Point { X = point.X, Y = point.Y - 1 };
                    break;
                case Direction.West:
                    nextPoint = new Point { X = point.X - 1, Y = point.Y };
                    break;
                case Direction.East:
                    nextPoint = new Point { X = point.X + 1, Y = point.Y };
                    break;
                case Direction.South:
                    nextPoint = new Point { X = point.X, Y = point.Y + 1 };
                    break;
            }
            if (!IsInBoarder(nextPoint))
            {
                throw new ArgumentOutOfRangeException("NextCell", nextPoint, $"The point X:{nextPoint.X} Y:{nextPoint.Y} is out of the board, the board coordination is Zero Base!");
            }
            return nextPoint;
        }
        internal IReadOnlyList<Point> GetMines()
        {
            return mines.ToList().AsReadOnly();
        }


    }

}
