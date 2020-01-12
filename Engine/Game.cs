using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public interface IAcceptMine
    {
        IAcceptExitPoint LayMine(IEnumerable<Point> minePositions);
    }
    public interface IAcceptExitPoint
    {
        IAcceptTutle SetExitPoint(Point point);

    }
    public interface IAcceptTutle
    {
        IGame AddTurtle(Point startPosition, Direction direction, EventHandler<TurtleStatusChangedEventArg> turtleStatusChangedHandler);
    }
    public interface IGame
    {
        IGame Move();
        IGame Rotate();
        IGame Reset();
        GameSnapeshot TakeSnapshot();
        event EventHandler<TurtleStatusChangedEventArg> TurtleStatusChanged;
    }
    public static class Game
    {
        public static IAcceptMine Create(int width, int height)
        {
            test tt = new test();
            
            GameImplementation game = new GameImplementation(width, height);
            return game;
        }
        public static IGame LoadSnapshot(GameSnapeshot snapshot, EventHandler<TurtleStatusChangedEventArg> turtleStatusChangedHandler = null)
        {
            GameImplementation game = Create(snapshot.Board_Width, snapshot.Board_Height) as GameImplementation;
            return game.ApplySnapShot(snapshot, turtleStatusChangedHandler);

        }


        internal sealed class GameImplementation : IAcceptMine, IAcceptExitPoint, IAcceptTutle, IGame
        {

            internal GameImplementation(int width, int height)
            {
                board = Board.Create(width, height);

            }
            Board board;
            Turtle turtle;
            event EventHandler<TurtleStatusChangedEventArg> IGame.TurtleStatusChanged
            {
                add
                {
                    if (value == null) turtleStatusChanged = null;
                    else
                    {
                        turtleStatusChanged += value;
                    }
                }
                remove
                {
                    if (value != null)
                    {
                        turtleStatusChanged -= value;

                    }

                }

            }


            IAcceptExitPoint IAcceptMine.LayMine(IEnumerable<Point> minePositions)
            {
                if (minePositions != null)
                {
                    foreach (var mine in minePositions)
                    {
                        this.board.LayMine(mine);

                    }
                }
                return this;
            }
            IAcceptTutle IAcceptExitPoint.SetExitPoint(Point point)
            {
                this.board.ExitPoint = point;
                return this;
            }

            IGame IAcceptTutle.AddTurtle(Point startPosition, Direction direction, EventHandler<TurtleStatusChangedEventArg> turtleStatusChangedHandler)
            {
                this.turtle = new Turtle(startPosition, direction);
                this.turtleStatusChanged += turtleStatusChangedHandler;                
                return this;
            }

            IGame IGame.Move()
            {
                if (gameIsFinished)
                {
                    throw new InvalidOperationException("Game is finished, You have to reset the game!");
                }
                Point pos;
                try
                {
                    pos = this.board.GetNextCell(this.turtle.CurrentPosition, turtle.CurrentDirection);
                }
                catch (ArgumentOutOfRangeException ex)
                {
                    Point tempPoint = (Point)ex.ActualValue;
                    throw new InvalidOperationException($"The turtle can not cross the borders! the distination point X={tempPoint.X} ,Y={ tempPoint.Y} is Out of Board!");
                }
                this.turtle.MoveTo(pos);
                CheckPointEffect(pos);
                return this;
            }

            private void CheckPointEffect(Point point)
            {
                var posType = board.GetCellType(point);
                switch (posType)
                {
                    case CellType.Mine:
                        this.turtle.Kill();
                        this.gameIsFinished = true;

                        break;
                    case CellType.Exit:
                        this.turtle.Escape();
                        this.gameIsFinished = true;
                        break;
                }
                OnTurtleStatusChanged();

            }

            event EventHandler<TurtleStatusChangedEventArg> turtleStatusChanged;
            private void OnTurtleStatusChanged()
            {

                if (turtleStatusChanged != null)
                {
                    var evArg = new TurtleStatusChangedEventArg(this.turtle);
                    evArg.GameIsFinished = this.gameIsFinished;
                    turtleStatusChanged(this, evArg);
                }
            }
            IGame IGame.Rotate()
            {
                if (gameIsFinished)
                {
                    throw new InvalidOperationException("Game is finished, You have to reset the game!");
                }
                turtle.Rotate();
                OnTurtleStatusChanged();
                return this;
            }

            IGame IGame.Reset()
            {
                gameIsFinished = false;
                this.turtle.Reset();
                OnTurtleStatusChanged();
                return this;
            }
            private bool gameIsFinished;

    
            GameSnapeshot IGame.TakeSnapshot()
            {
                GameSnapeshot snapshot = new GameSnapeshot();

                if (this.board != null)
                {
                    snapshot.Board_ExitPoint = this.board.ExitPoint;
                    snapshot.Board_Height = this.board.Height;
                    snapshot.Board_Width = this.board.Width;
                    snapshot.Board_Mines = this.board.GetMines();
                }
                if (this.turtle != null)
                {
                    snapshot.Turtle_CurrentDirection = this.turtle.CurrentDirection;
                    snapshot.Turtle_CurrentPosition = this.turtle.CurrentPosition;
                    snapshot.Turtle_StartDirection = this.turtle.StartDirection;
                    snapshot.Turtle_StartPosition = this.turtle.StartPosition;
                    snapshot.Turtle_Status = this.turtle.Status;
                }
                return snapshot;
            }

            public IGame ApplySnapShot(GameSnapeshot snapshot, EventHandler<TurtleStatusChangedEventArg> turtleStatusChangedHandler)
            {
                (this as IAcceptMine).LayMine(snapshot.Board_Mines);
                (this as IAcceptExitPoint).SetExitPoint(snapshot.Board_ExitPoint);
                (this as IAcceptTutle).AddTurtle(snapshot.Turtle_StartPosition, snapshot.Turtle_StartDirection, turtleStatusChangedHandler);
                this.turtle.MoveTo(snapshot.Turtle_CurrentPosition);
                while (this.turtle.CurrentDirection != snapshot.Turtle_CurrentDirection)
                {
                    this.turtle.Rotate();
                }
                if (snapshot.Turtle_Status == TurtleState.Killed)
                {
                    this.turtle.Kill();
                }
                else if (snapshot.Turtle_Status == TurtleState.Escaped)
                {
                    this.turtle.Escape();
                }

                return this;
            }
        }
        

    }
    class test : IAcceptMine,IAcceptExitPoint
    {
        public IAcceptExitPoint LayMine(IEnumerable<Point> minePositions)
        {
               
            throw new NotImplementedException();
        }

        public IAcceptTutle SetExitPoint(Point point)
        {
            throw new NotImplementedException();
        }
    }
}
