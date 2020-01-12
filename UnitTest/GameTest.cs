using Engine;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;
using static Engine.Game;
using System.Linq;
using System.Collections.Generic;

namespace UnitTest
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class GameTest
    {
        [TestMethod]
        public void Test_Game_Create_Ok()
        {
            var h = 10;
            var w = 15;
            var game = Game.Create(w, h);
            IGame g = game as IGame;
            Assert.IsNotNull(g);
            var snapShot=g.TakeSnapshot();
            Assert.AreEqual(snapShot.Board_Width,w);
            Assert.AreEqual(snapShot.Board_Height,h);
        }
        [TestMethod]
        public void Test_Game_LayMine_Ok()
        {
            List<Point> points = new List<Point>();
            points.Add(new Point() { X = 12, Y = 6 });
            points.Add(new Point() { X = 14, Y = 3 });
            var h = 10;
            var w = 15;
            var game = Game.Create(w, h).LayMine(points);
            IGame g = game as IGame;
            Assert.IsNotNull(g);
            var snapShot = g.TakeSnapshot();
            Assert.AreEqual(snapShot.Board_Mines.Count, points.Count);
            Assert.AreEqual(snapShot.Board_Mines[0], points[0]);
            Assert.AreEqual(snapShot.Board_Mines[1], points[1]);
        }
        [TestMethod]
        public void Test_Game_SetExitPoint_Ok()
        {
            var exitPoint = new Point() { X = 4, Y = 4 };
            var h = 10;
            var w = 15;
            var game = Game.Create(w, h).LayMine(null).SetExitPoint(exitPoint);
            IGame g = game as IGame;
            Assert.IsNotNull(g);
            var snapShot = g.TakeSnapshot();
            Assert.AreEqual(snapShot.Board_ExitPoint, exitPoint);
        }

        [TestMethod]
        public void Test_Game_AddTurtle_Ok()
        {
            var exitPoint = new Point() { X = 4, Y = 4 };
            var startPosition = new Point() { X = 1, Y = 4 };
            var h = 10;
            var w = 15;
            var game = Game.Create(w, h).LayMine(null).SetExitPoint(exitPoint).AddTurtle(startPosition,Direction.East,null);
            IGame g = game as IGame;
            Assert.IsNotNull(g);
            var snapShot = g.TakeSnapshot();
            Assert.AreEqual(snapShot.Turtle_StartPosition, startPosition);
            Assert.AreEqual(snapShot.Turtle_CurrentPosition, startPosition);
            Assert.AreEqual(snapShot.Turtle_CurrentDirection, Direction.East);
            Assert.AreEqual(snapShot.Turtle_StartDirection, Direction.East);
        }
        [TestMethod]
        public void Test_Game_MoveTurtle_Ok()
        {
            var exitPoint = new Point() { X = 4, Y = 4 };
            var startPosition = new Point() { X = 1, Y = 2 };
            var nextPosition = new Point() { X = startPosition.X, Y = startPosition.Y-1 };
            var h = 10;
            var w = 15;
            var game = Game.Create(w, h).LayMine(null).SetExitPoint(exitPoint).AddTurtle(startPosition, Direction.North, null);
            game.Move();
            var snapShot = game.TakeSnapshot();
            Assert.AreEqual(snapShot.Turtle_StartPosition, startPosition);
            Assert.AreEqual(snapShot.Turtle_CurrentPosition, nextPosition);

        }
        [TestMethod]
        public void Test_Game_RotateTurtle_Ok()
        {
            var exitPoint = new Point() { X = 4, Y = 4 };
            var startPosition = new Point() { X = 1, Y = 2 };
            var h = 10;
            var w = 15;
            var game = Game.Create(w, h).LayMine(null).SetExitPoint(exitPoint).AddTurtle(startPosition, Direction.North, null);
            game.Rotate();
            var snapShot = game.TakeSnapshot();
            Assert.AreEqual(snapShot.Turtle_StartPosition, startPosition);
            Assert.AreEqual(snapShot.Turtle_CurrentPosition, startPosition);
            Assert.AreEqual(snapShot.Turtle_StartDirection, Direction.North);
            Assert.AreEqual(snapShot.Turtle_CurrentDirection, Direction.East);

        }
        [TestMethod]
        public void Test_Game_ResetTurtle_Ok()
        {
            var exitPoint = new Point() { X = 4, Y = 4 };
            var startPosition = new Point() { X = 1, Y = 2 };
            var h = 10;
            var w = 15;
            var game = Game.Create(w, h).LayMine(null).SetExitPoint(exitPoint).AddTurtle(startPosition, Direction.North, null);
            game.Move();
            game.Rotate();
            game.Reset();
            var snapShot = game.TakeSnapshot();
            Assert.AreEqual(snapShot.Turtle_StartPosition, startPosition);
            Assert.AreEqual(snapShot.Turtle_CurrentPosition, startPosition);
            Assert.AreEqual(snapShot.Turtle_StartDirection, Direction.North);
            Assert.AreEqual(snapShot.Turtle_CurrentDirection, Direction.North);

        }
        [TestMethod]
        public void Test_Game_ApplySnapShot_Ok()
        {
            var exitPoint = new Point() { X = 2, Y = 1 };
            var startPosition = new Point() { X = 1, Y = 1 };
            var h = 10;
            var w = 15;
            var game = Game.Create(w, h).LayMine(null).SetExitPoint(exitPoint).AddTurtle(startPosition, Direction.North, null);
            game.Rotate();
            game.Move();
            var snapShot = game.TakeSnapshot();
            var newGame =  Game.LoadSnapshot(snapShot, null);
            var newSnapShot = newGame.TakeSnapshot();
            Assert.AreEqual(snapShot.Turtle_StartPosition, newSnapShot.Turtle_StartPosition);
            Assert.AreEqual(snapShot.Turtle_CurrentPosition, newSnapShot.Turtle_CurrentPosition);
            Assert.AreEqual(snapShot.Turtle_StartDirection, newSnapShot.Turtle_StartDirection);
            Assert.AreEqual(snapShot.Turtle_CurrentDirection, newSnapShot.Turtle_CurrentDirection);
        }
        [TestMethod]
        public void Test_Game_ApplySnapShot_Killed_Ok()
        {
            var exitPoint = new Point() { X = 4, Y = 1 };
            var startPosition = new Point() { X = 1, Y = 1 };
            List<Point> mins = new List<Point>();
            mins.Add(new Point() { X = 2, Y = 1 });
            var h = 10;
            var w = 15;
            var game = Game.Create(w, h).LayMine(mins).SetExitPoint(exitPoint).AddTurtle(startPosition, Direction.North, null);
            game.Rotate();
            game.Move();
            var snapShot = game.TakeSnapshot();
            var newGame = Game.LoadSnapshot(snapShot, null);
            var newSnapShot = newGame.TakeSnapshot();
            Assert.AreEqual(snapShot.Turtle_StartPosition, newSnapShot.Turtle_StartPosition);
            Assert.AreEqual(snapShot.Turtle_CurrentPosition, newSnapShot.Turtle_CurrentPosition);
            Assert.AreEqual(snapShot.Turtle_StartDirection, newSnapShot.Turtle_StartDirection);
            Assert.AreEqual(snapShot.Turtle_CurrentDirection, newSnapShot.Turtle_CurrentDirection);
        }
        [TestMethod]
        public void Test_Game_TurtuleStateChanged_Ok()
        {
            var exitPoint = new Point() { X = 4, Y = 4 };
            var startPosition = new Point() { X = 1, Y = 2 };
            var nextPosition = new Point() { X = startPosition.X, Y = startPosition.Y - 1 };
            var h = 10;
            var w = 15;
            bool isFired = false;
    
            var game = Game.Create(w, h).LayMine(null).SetExitPoint(exitPoint).AddTurtle(startPosition, Direction.North, (s, e) =>
            isFired=true);
            game.Move();
            Assert.IsTrue(isFired);

        }

        [TestMethod]
        public void Test_Game_TurtuleStateChanged_Killed_Ok()
        {
            var exitPoint = new Point() { X = 4, Y = 4 };
            var startPosition = new Point() { X = 1, Y = 2 };
            var nextPosition = new Point() { X = startPosition.X, Y = startPosition.Y - 1 };
            List<Point> mins = new List<Point>();
            mins.Add(new Point() { X = 0, Y = 1 });
            var h = 10;
            var w = 15;
            TurtleStatusChangedEventArg lastEventArg=null;
            var game = Game.Create(w, h).LayMine(mins).SetExitPoint(exitPoint).AddTurtle(startPosition, Direction.North, (s, e) =>
            lastEventArg = e);
            game.Move();
            Assert.AreEqual(lastEventArg.Position, nextPosition);
            Assert.AreEqual(lastEventArg.Direction, Direction.North);
            game.Rotate();
            Assert.AreEqual(lastEventArg.Position, nextPosition);
            Assert.AreEqual(lastEventArg.Direction, Direction.East);
            game.Rotate();
            Assert.AreEqual(lastEventArg.Position, nextPosition);
            Assert.AreEqual(lastEventArg.Direction, Direction.South);
            game.Rotate();
            Assert.AreEqual(lastEventArg.Position, nextPosition);
            Assert.AreEqual(lastEventArg.Direction, Direction.West);
            game.Move();
            Assert.AreEqual(lastEventArg.Position, mins[0]);
            Assert.AreEqual(lastEventArg.Direction, Direction.West);
            Assert.AreEqual(lastEventArg.Status, TurtleState.Killed);

        }

        [TestMethod]
        public void Test_Game_TurtuleStateChanged_Escaped_Ok()
        {
            var exitPoint = new Point() { X = 1, Y = 1 };
            var startPosition = new Point() { X = 1, Y = 2 };
            List<Point> mins = new List<Point>();
            mins.Add(new Point() { X = 0, Y = 1 });
            var h = 10;
            var w = 15;
            TurtleStatusChangedEventArg lastEventArg = null;
            var game = Game.Create(w, h).LayMine(mins).SetExitPoint(exitPoint).AddTurtle(startPosition, Direction.North, (s, e) =>
            lastEventArg = e);
            game.Move();
            Assert.AreEqual(lastEventArg.Position, exitPoint);
            Assert.AreEqual(lastEventArg.Direction, Direction.North);
            Assert.AreEqual(lastEventArg.Status, TurtleState.Escaped);

        }

        [TestMethod]
        public void Test_Game_TurtuleStateChanged_IsInDanger_Ok()
        {
            var exitPoint = new Point() { X = 3, Y = 3 };
            var startPosition = new Point() { X = 1, Y = 2 };
            var nextPosition = new Point() { X = startPosition.X, Y = startPosition.Y+1 };
            List<Point> mins = new List<Point>();
            mins.Add(new Point() { X = 0, Y = 1 });
            var h = 10;
            var w = 15;
            TurtleStatusChangedEventArg lastEventArg = null;
            var game = Game.Create(w, h).LayMine(mins).SetExitPoint(exitPoint).AddTurtle(startPosition, Direction.North, (s, e) =>
            lastEventArg = e);
            game.Rotate();
            game.Rotate();
            game.Move();
            Assert.AreEqual(lastEventArg.Position, nextPosition);
            Assert.AreEqual(lastEventArg.Direction, Direction.South);
            Assert.AreEqual(lastEventArg.Status, TurtleState.IsInDanger);

        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException), "Game is finished, You have to reset the game!")]
        public void Test_Game_TurtuleStateChanged_GameIsFinished_Move_Ok()
        {
            var exitPoint = new Point() { X = 1, Y = 1 };
            var startPosition = new Point() { X = 1, Y = 2 };
            List<Point> mins = new List<Point>();
            mins.Add(new Point() { X = 0, Y = 1 });
            var h = 10;
            var w = 15;
            TurtleStatusChangedEventArg lastEventArg = null;
            var game = Game.Create(w, h).LayMine(mins).SetExitPoint(exitPoint).AddTurtle(startPosition, Direction.North, (s, e) =>
            lastEventArg = e);
            game.Move();
            Assert.AreEqual(lastEventArg.Position, exitPoint);
            Assert.AreEqual(lastEventArg.Direction, Direction.North);
            Assert.AreEqual(lastEventArg.Status, TurtleState.Escaped);

         
                game.Move();
           

        }


        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException), "Game is finished, You have to reset the game!")]
        public void Test_Game_TurtuleStateChanged_GameIsFinished_Rotate_Ok()
        {
            var exitPoint = new Point() { X = 1, Y = 1 };
            var startPosition = new Point() { X = 1, Y = 2 };
            List<Point> mins = new List<Point>();
            mins.Add(new Point() { X = 0, Y = 1 });
            var h = 10;
            var w = 15;
            TurtleStatusChangedEventArg lastEventArg = null;
            var game = Game.Create(w, h).LayMine(mins).SetExitPoint(exitPoint).AddTurtle(startPosition, Direction.North, (s, e) =>
            lastEventArg = e);
            game.Move();
            Assert.AreEqual(lastEventArg.Position, exitPoint);
            Assert.AreEqual(lastEventArg.Direction, Direction.North);
            Assert.AreEqual(lastEventArg.Status, TurtleState.Escaped);
            game.Rotate();


        }


        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Test_Game_TurtuleStateChanged_CrossTheborders_Move_Ok()
        {
            var exitPoint = new Point() { X = 1, Y = 1 };
            var startPosition = new Point() { X = 0, Y = 0 };
            List<Point> mins = new List<Point>();
            mins.Add(new Point() { X = 0, Y = 1 });
            var h = 10;
            var w = 15;
            TurtleStatusChangedEventArg lastEventArg = null;
            var game = Game.Create(w, h).LayMine(mins).SetExitPoint(exitPoint).AddTurtle(startPosition, Direction.North, (s, e) =>
            lastEventArg = e);
            game.Move();
        }
    }
}
