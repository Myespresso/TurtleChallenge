using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public sealed class TurtleStatusChangedEventArg : EventArgs
    {
        public Point Position { get; internal set; }
        public Direction Direction { get; internal set; }
        public TurtleState Status { get; internal set; }
        public bool GameIsFinished { get; internal set; }
        internal TurtleStatusChangedEventArg(Turtle turtle)
        {
            this.Position = turtle.CurrentPosition;
            this.Direction = turtle.CurrentDirection;
            this.Status = turtle.Status;
        }
    }
}
