using Engine;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;

namespace UnitTest
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class TurtleTest
    {
        [TestMethod]
        public void Test_TurtleTest_Ok()
        {
            var startPosition = new Point() { X = 10, Y = 20 };
            Turtle turtle = new Turtle(startPosition, Direction.East);
            Assert.IsNotNull(turtle);
            Assert.AreEqual(turtle.CurrentPosition, startPosition);
            Assert.AreEqual(turtle.StartPosition, startPosition);
        }
        [TestMethod]
        public void Test_TurtleTest_Rotate_South_Ok()
        {
            var startPosition = new Point() { X = 10, Y = 20 };
            Turtle turtle = new Turtle(startPosition, Direction.East);
            turtle.Rotate();
            Assert.AreEqual(turtle.CurrentDirection, Direction.South);
        }
        [TestMethod]
        public void Test_TurtleTest_Rotate_West_Ok()
        {
            var startPosition = new Point() { X = 10, Y = 20 };
            Turtle turtle = new Turtle(startPosition, Direction.South);
            turtle.Rotate();
            Assert.AreEqual(turtle.CurrentDirection, Direction.West);
        }

        [TestMethod]
        public void Test_TurtleTest_Rotate_North_Ok()
        {
            var startPosition = new Point() { X = 10, Y = 20 };
            Turtle turtle = new Turtle(startPosition, Direction.West);
            turtle.Rotate();
            Assert.AreEqual(turtle.CurrentDirection, Direction.North);
        }
        [TestMethod]
        public void Test_TurtleTest_Rotate_East_Ok()
        {
            var startPosition = new Point() { X = 10, Y = 20 };
            Turtle turtle = new Turtle(startPosition, Direction.North);
            turtle.Rotate();
            Assert.AreEqual(turtle.CurrentDirection, Direction.East);
        }

        [TestMethod]
        public void Test_TurtleTest_MoveTo_Ok()
        {
            var startPosition = new Point() { X = 10, Y = 20 };
            Turtle turtle = new Turtle(startPosition, Direction.North);
            var destinationPosition = new Point() { X = 11, Y = 20 };
            turtle.MoveTo(destinationPosition);
            Assert.AreEqual(turtle.CurrentPosition, destinationPosition);
        }


        [TestMethod]
        public void Test_TurtleTest_Kill_Ok()
        {
            var startPosition = new Point() { X = 10, Y = 20 };
            Turtle turtle = new Turtle(startPosition, Direction.North);
            turtle.Kill();
            Assert.AreEqual(turtle.Status, TurtleState.Killed);
        }


        [TestMethod]
        public void Test_TurtleTest_Escape_Ok()
        {
            var startPosition = new Point() { X = 10, Y = 20 };
            Turtle turtle = new Turtle(startPosition, Direction.North);
            turtle.Escape();
            Assert.AreEqual(turtle.Status, TurtleState.Escaped);
        }
        [TestMethod]
        public void Test_TurtleTest_Reset_Ok()
        {
            var startPosition = new Point() { X = 10, Y = 20 };
            var destinationPosition = new Point() { X = 11, Y = 20 };
            Turtle turtle = new Turtle(startPosition, Direction.North);
            turtle.MoveTo(destinationPosition);
            turtle.Kill();
            Assert.AreEqual(turtle.Status, TurtleState.Killed);
            turtle.Reset();
            Assert.AreEqual(turtle.Status, TurtleState.IsInDanger);
            Assert.AreEqual(turtle.CurrentPosition, startPosition);

        }
    }
}
