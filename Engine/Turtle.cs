using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public sealed class Turtle
    {
        public Point StartPosition { get; private set; }
        public Point CurrentPosition { get; private  set; }
        public Direction StartDirection { get; private set; }
        public Direction CurrentDirection { get; private set; }
        public TurtleState Status { get; private set; }
        internal void MoveTo(Point destination)
        {
            this.CurrentPosition = destination;
        }
        internal Turtle(Point startPosition, Direction startDirection)
        {
            this.StartPosition = startPosition;
            this.CurrentPosition = startPosition;
            this.StartDirection = startDirection;
            this.CurrentDirection = startDirection;
            this.Status = TurtleState.IsInDanger;
        }
        internal void Kill()
        {
            this.Status = TurtleState.Killed;

        }
        internal void Escape()
        {
            this.Status = TurtleState.Escaped;

        }
        internal void Reset()
        {
            this.CurrentDirection = StartDirection;
            this.CurrentPosition = this.StartPosition;
            this.Status = TurtleState.IsInDanger;
        }
        internal void Rotate()
        {
            switch (this.CurrentDirection)
            {
                case Direction.North:
                    this.CurrentDirection = Direction.East;
                    break;
                case Direction.West:
                    this.CurrentDirection = Direction.North;
                    break;
                case Direction.East:
                    this.CurrentDirection = Direction.South;

                    break;
                case Direction.South:
                    this.CurrentDirection = Direction.West;

                    break;
            }
        }
    }
}
