using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    [Serializable]
    public sealed class GameSnapeshot
    {
        internal GameSnapeshot()
        {

        }
        public int Board_Width { get; internal set; }
        public int Board_Height { get; internal set; }
        public IReadOnlyList<Point> Board_Mines { get; internal set; }
        public Point Board_ExitPoint { get; internal set; }
        public Point Turtle_StartPosition { get; internal set; }
        public Point Turtle_CurrentPosition { get; internal set; }
        public Direction Turtle_StartDirection { get; internal set; }
        public Direction Turtle_CurrentDirection { get; internal set; }
        public TurtleState Turtle_Status { get; internal set; }
    }
}
