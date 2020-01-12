using Engine;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;

namespace UnitTest
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class BoardTest
    {
        [TestMethod]
        public void Test_BoardTest_Ok()
        {
            var h = 10;
            var w = 15;
            var board = Board.Create(w, h);
            Assert.IsNotNull(board);
            Assert.AreEqual(h, board.Height);
            Assert.AreEqual(w, board.Width);
        }
        [TestMethod]
        public void Test_BoardTest_NotOk_Withd()
        {
            var h = 10;
            var w = 0;
            try
            {
                var board = Board.Create(w, h);

            }
            catch (ArgumentOutOfRangeException exp)
            {
                Assert.AreEqual(exp.ParamName, "width");
                var width = (int)exp.ActualValue;
                Assert.AreEqual(width, w);
                return;
            }
            Assert.Fail("Exception Not Occured");
        }

        [TestMethod]
        public void Test_BoardTest_NotOk_Height()
        {
            var h = 0;
            var w = 10;
            try
            {
                var board = Board.Create(w, h);

            }
            catch (ArgumentOutOfRangeException exp)
            {
                Assert.AreEqual(exp.ParamName, "height");
                var height = (int)exp.ActualValue;
                Assert.AreEqual(height, h);
                return;
            }
            Assert.Fail("Exception Not Occured");
        }

        [TestMethod]
        public void Test_BoardTest_SetExitPoint_Ok()
        {
            var h = 20;
            var w = 10;
            var exitpoint = new Point() { X = 3, Y = 5 };

            var board = Board.Create(w, h);
            board.ExitPoint = exitpoint;
            Assert.AreEqual(board.ExitPoint.X, exitpoint.X);
            Assert.AreEqual(board.ExitPoint.Y, exitpoint.Y);
            Assert.IsTrue(board.ExitPoint == exitpoint);


        }

        [TestMethod]
        public void Test_BoardTest_SetExitPoint_NOk()
        {
            var h = 20;
            var w = 10;
            var exitpoint = new Point() { X = w + 4, Y = h + 12 };
            var board = Board.Create(w, h);
            var oldexitpoint = new Point() { X = board.ExitPoint.X, Y = board.ExitPoint.Y };
            try
            {
                board.ExitPoint = exitpoint;
            }
            catch (ArgumentOutOfRangeException exp)
            {
                Assert.AreEqual(exp.ParamName, "ExitPoint");
                var temppoint = (Point)exp.ActualValue;
                Assert.AreEqual(temppoint.X, exitpoint.X);
                Assert.AreEqual(temppoint.Y, exitpoint.Y);
                Assert.AreEqual(board.ExitPoint.X, oldexitpoint.X);
                Assert.AreEqual(board.ExitPoint.Y, oldexitpoint.Y);
                Assert.IsTrue(board.ExitPoint != exitpoint);
                Assert.IsTrue(board.ExitPoint == oldexitpoint);
                return;
            }
            Assert.Fail();


        }

        [TestMethod]
        public void Test_BoardTest_SetExitPoint_LayMine_Ok()
        {
            var h = 20;
            var w = 10;
            var exitpoint = new Point() { X = 5, Y = 7 };
            var board = Board.Create(w, h);
            var minePoint = new Point { X = 6, Y = 8 };
            board.ExitPoint = exitpoint;
            board.LayMine(minePoint);
            var cType = board.GetCellType(minePoint);
            var mines = board.GetMines();
            Assert.IsTrue(mines.Count == 1);
            Assert.IsTrue(mines[0] == minePoint);
            Assert.AreEqual(cType, CellType.Mine);
        }
        [TestMethod]
        public void Test_BoardTest_SetExitPoint_LayMine_NOk_DueTo_Exist_Mine()
        {
            var h = 20;
            var w = 10;
            var board = Board.Create(w, h);
            var minePoint = new Point { X = 6, Y = 8 };
            board.LayMine(minePoint);
            var cType = board.GetCellType(minePoint);
            var mines = board.GetMines();
            Assert.IsTrue(mines.Count == 1);
            Assert.IsTrue(mines[0] == minePoint);
            Assert.AreEqual(cType, CellType.Mine);
            try
            {
                board.LayMine(minePoint);
            }
            catch (ArgumentOutOfRangeException exp)
            {
                Assert.AreEqual(exp.ParamName, "point");
                var temppoint = (Point)exp.ActualValue;
                Assert.AreEqual(temppoint.X, minePoint.X);
                Assert.AreEqual(temppoint.Y, minePoint.Y);
                Assert.IsTrue(mines.Count == 1);
                Assert.IsTrue(mines[0] == minePoint);
                Assert.AreEqual(cType, CellType.Mine);
                return;
            }
            Assert.Fail();
        }

        [TestMethod]
        public void Test_BoardTest_SetExitPoint_LayMine_NOk_DueTo_ExitPoint()
        {
            var h = 20;
            var w = 10;
            var board = Board.Create(w, h);
            var exitpoint = new Point() { X = 5, Y = 7 };
            board.ExitPoint = exitpoint;
            var cType = board.GetCellType(exitpoint);
            var mines = board.GetMines();
            Assert.IsTrue(mines.Count == 0);
            Assert.AreEqual(cType, CellType.Exit);
            try
            {
                board.LayMine(exitpoint);
            }
            catch (ArgumentOutOfRangeException exp)
            {
                Assert.AreEqual(exp.ParamName, "point");
                var temppoint = (Point)exp.ActualValue;
                Assert.AreEqual(temppoint.X, board.ExitPoint.X);
                Assert.AreEqual(temppoint.Y, board.ExitPoint.Y);
                Assert.IsTrue(mines.Count == 0);
                Assert.IsTrue(board.ExitPoint == exitpoint);
                Assert.AreEqual(cType, CellType.Exit);
                return;
            }
            Assert.Fail();
        }



        [TestMethod]
        public void Test_BoardTest_GetNextCell_North_OK()
        {

            var h = 20;
            var w = 10;
            var board = Board.Create(w, h);
            var myPoint = new Point() { X = 5, Y = 7 };
            var point = board.GetNextCell(myPoint, Direction.North);
            var myExpected = new Point() { X = myPoint.X, Y = myPoint.Y-1 };
            Assert.AreEqual(myExpected, point);
          
        }
        [TestMethod]
        public void Test_BoardTest_GetNextCell_East_OK()
        {

            var h = 20;
            var w = 10;
            var board = Board.Create(w, h);
            var myPoint = new Point() { X = 5, Y = 7 };
            var point = board.GetNextCell(myPoint, Direction.East);
            var myExpected = new Point() { X = myPoint.X+1, Y = myPoint.Y };
            Assert.AreEqual(myExpected, point);

        }
        [TestMethod]
        public void Test_BoardTest_GetNextCell_South_OK()
        {

            var h = 20;
            var w = 10;
            var board = Board.Create(w, h);
            var myPoint = new Point() { X = 5, Y = 7 };
            var point = board.GetNextCell(myPoint, Direction.South);
            var myExpected = new Point() { X = myPoint.X, Y = myPoint.Y+1 };
            Assert.AreEqual(myExpected, point);

        }

        [TestMethod]
        public void Test_BoardTest_GetNextCell_West_OK()
        {

            var h = 20;
            var w = 10;
            var board = Board.Create(w, h);
            var myPoint = new Point() { X = 5, Y = 7 };
            var point = board.GetNextCell(myPoint, Direction.West);
            var myExpected = new Point() { X = myPoint.X-1, Y = myPoint.Y };
            Assert.AreEqual(myExpected, point);

        }

        [TestMethod]
        public void Test_BoardTest_GetNextCell_West_NOK_Negative_X()
        {
            var h = 20;
            var w = 10;
            var board = Board.Create(w, h);
            var myPoint = new Point() { X = -1*w, Y = h };
            try
            {
                var point = board.GetNextCell(myPoint, Direction.West);
            }
            catch (ArgumentOutOfRangeException exp)
            {
                Assert.AreEqual(exp.ParamName, "NextCell");
                var temppoint = (Point)exp.ActualValue;
                Assert.AreEqual(temppoint.X, myPoint.X - 1);
                Assert.AreEqual(temppoint.Y, myPoint.Y);
                return;
            }
            Assert.Fail();
        }
        [TestMethod]
        public void Test_BoardTest_GetNextCell_West_NOK_Negative_Y()
        {
            var h = 20;
            var w = 10;
            var board = Board.Create(w, h);
            var myPoint = new Point() { X = w, Y =-1* h };
            try
            {
                var point = board.GetNextCell(myPoint, Direction.West);
            }
            catch (ArgumentOutOfRangeException exp)
            {
                Assert.AreEqual(exp.ParamName, "NextCell");
                var temppoint = (Point)exp.ActualValue;
                Assert.AreEqual(temppoint.X, myPoint.X - 1);
                Assert.AreEqual(temppoint.Y, myPoint.Y);
                return;
            }
            Assert.Fail();
        }


        [TestMethod]
        public void Test_BoardTest_GetNextCell_West_NOK()
        {
            var h = 20;
            var w = 10;
            var board = Board.Create(w, h);
            var myPoint = new Point() { X = w, Y = h };
            try
            {
                var point = board.GetNextCell(myPoint, Direction.West);
            }
            catch (ArgumentOutOfRangeException exp)
            {
                Assert.AreEqual(exp.ParamName, "NextCell");
                var temppoint = (Point)exp.ActualValue;
                Assert.AreEqual(temppoint.X, myPoint.X-1);
                Assert.AreEqual(temppoint.Y, myPoint.Y);
                return;
            }
            Assert.Fail();
        }
        [TestMethod]
        public void Test_BoardTest_GetNextCell_South_NOK()
        {
            var h = 20;
            var w = 10;
            var board = Board.Create(w, h);
            var myPoint = new Point() { X = w, Y = h };
            try
            {
                var point = board.GetNextCell(myPoint, Direction.South);
            }
            catch (ArgumentOutOfRangeException exp)
            {
                Assert.AreEqual(exp.ParamName, "NextCell");
                var temppoint = (Point)exp.ActualValue;
                Assert.AreEqual(temppoint.X, myPoint.X);
                Assert.AreEqual(temppoint.Y, myPoint.Y+1);
                return;
            }
            Assert.Fail();
        }
        [TestMethod]
        public void Test_BoardTest_GetNextCell_East_NOK()
        {
            var h = 20;
            var w = 10;
            var board = Board.Create(w, h);
            var myPoint = new Point() { X = w, Y = h };
            try
            {
                var point = board.GetNextCell(myPoint, Direction.East);
            }
            catch (ArgumentOutOfRangeException exp)
            {
                Assert.AreEqual(exp.ParamName, "NextCell");
                var temppoint = (Point)exp.ActualValue;
                Assert.AreEqual(temppoint.X, myPoint.X+1);
                Assert.AreEqual(temppoint.Y, myPoint.Y);
                return;
            }
            Assert.Fail();
        }
        [TestMethod]
        public void Test_BoardTest_GetNextCell_North_NOK()
        {
            var h = 20;
            var w = 10;
            var board = Board.Create(w, h);
            var myPoint = new Point() { X = w, Y = h };
            try
            {
                var point = board.GetNextCell(myPoint, Direction.North);
            }
            catch (ArgumentOutOfRangeException exp)
            {
                Assert.AreEqual(exp.ParamName, "NextCell");
                var temppoint = (Point)exp.ActualValue;
                Assert.AreEqual(temppoint.X, myPoint.X);
                Assert.AreEqual(temppoint.Y, myPoint.Y-1);
                return;
            }
            Assert.Fail();
        }

        [TestMethod]
        public void Test_BoardTest_CellType_NOK()
        {
            var h = 20;
            var w = 10;
            var board = Board.Create(w, h);
            var myPoint = new Point() { X = w+1, Y = h+1 };
            try
            {
                var point = board.GetCellType(myPoint);
            }
            catch (ArgumentOutOfRangeException exp)
            {
                Assert.AreEqual(exp.ParamName, "point");
                var temppoint = (Point)exp.ActualValue;
                Assert.AreEqual(temppoint.X, myPoint.X);
                Assert.AreEqual(temppoint.Y, myPoint.Y);
                return;
            }
            Assert.Fail();
        }
    }
}
