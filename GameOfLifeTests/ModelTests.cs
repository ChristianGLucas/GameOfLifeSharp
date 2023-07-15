using GameOfLife.Model;
using System.Diagnostics;
using System.Drawing;

namespace GameOfLifeTests
{
    [TestClass]
    public class ModelTests
    {
        [TestMethod]
        public void TestIsAlive()
        {
            IModel model = new Model();
            Assert.IsFalse(model.IsAlive(new Point(0, 0)));
            model.InsertLiveCell(new Point(0, 0));
            Assert.IsTrue(model.IsAlive(new Point(0, 0)));
        }

        [TestMethod]
        public void TestInsertLiveCell()
        {
            IModel model = new Model();
            model.InsertLiveCell(new Point(0, 0));
            Assert.IsTrue(model.IsAlive(new Point(0, 0)));
            model.InsertLiveCell(new Point(0, 1));
            model.InsertLiveCell(new Point(0, 2));
            Assert.IsTrue(model.IsAlive(new Point(0, 0)));
            Assert.IsTrue(model.IsAlive(new Point(0, 1)));
            Assert.IsTrue(model.IsAlive(new Point(0, 2)));
        }

        [TestMethod]
        public void TestInsertLiveCells()
        {
            IModel model = new Model();
            List<Point> points = new List<Point>()
            {
                new Point(0, 0),
                new Point(0, 1),
                new Point(0, 2),
            };
            model.InsertLiveCells(points);
            Assert.IsTrue(model.IsAlive(new Point(0, 0)));
            Assert.IsTrue(model.IsAlive(new Point(0, 1)));
            Assert.IsTrue(model.IsAlive(new Point(0, 2)));
        }

        [TestMethod]
        public void TestRemoveLiveCell()
        {
            IModel model = new Model();
            model.InsertLiveCell(new Point(0, 0));
            model.InsertLiveCell(new Point(0, 1));
            Assert.IsTrue(model.IsAlive(new Point(0, 0)));
            model.RemoveLiveCell(new Point(0, 0));
            Assert.IsFalse(model.IsAlive(new Point(0, 0)));
            Assert.IsTrue(model.IsAlive(new Point(0, 1)));
        }

        [TestMethod]
        public void TestRemoveLiveCells()
        {
            IModel model = new Model();
            List<Point> points = new List<Point>()
            {
                new Point(0, 0),
                new Point(0, 1),
                new Point(0, 2),
            };
            model.InsertLiveCells(points);
            Assert.IsTrue(model.IsAlive(new Point(0, 0)));
            Assert.IsTrue(model.IsAlive(new Point(0, 1)));
            Assert.IsTrue(model.IsAlive(new Point(0, 2)));

            model.RemoveLiveCells(new List<Point>() { new Point(0, 0),
                new Point(0, 1) });
            Assert.IsFalse(model.IsAlive(new Point(0, 0)));
            Assert.IsFalse(model.IsAlive(new Point(0, 1)));
            Assert.IsTrue(model.IsAlive(new Point(0, 2)));
        }

        [TestMethod]
        public void TestLiveCellPositions()
        {
            IModel model = new Model();
            List<Point> points = new List<Point>()
            {
                new Point(0, 0),
                new Point(0, 1),
                new Point(0, 2),
            };
            model.InsertLiveCells(points);
            var liveCellPositions = model.LiveCellPositions();
            Assert.AreEqual(points.Count, liveCellPositions.Count());
            foreach(var cellPosition in liveCellPositions)
            {
                Assert.IsTrue(points.Contains(cellPosition));
            }
        }

        [TestMethod]
        public void TestClear()
        {
            IModel model = new Model();
            List<Point> points = new List<Point>()
            {
                new Point(0, 0),
                new Point(0, 1),
                new Point(0, 2),
            };
            model.InsertLiveCells(points);
            model.Clear();
            Assert.AreEqual(0, model.LiveCellPositions().Count());
        }

        [TestMethod]
        public async Task TestStep()
        {
            IModel model = new Model();
            List<Point> points = new List<Point>()
            {
                new Point(-1, 0),
                new Point(0, 0),
                new Point(1, 0),
            };
            model.InsertLiveCells(points);
            await model.Step();
            List<Point> expected = new List<Point>()
            {
                new Point(0, -1),
                new Point(0, 0),
                new Point(0, 1),
            };
            var liveCellPositions = model.LiveCellPositions();
            Assert.AreEqual(expected.Count, liveCellPositions.Count());
            foreach (var cellPosition in liveCellPositions)
            {
                Assert.IsTrue(expected.Contains(cellPosition));
            }
            await model.Step();
            expected = new List<Point>()
            {
                new Point(-1, 0),
                new Point(0, 0),
                new Point(1, 0),
            };
            liveCellPositions = model.LiveCellPositions();
            Assert.AreEqual(expected.Count, liveCellPositions.Count());
            foreach (var cellPosition in liveCellPositions)
            {
                Assert.IsTrue(expected.Contains(cellPosition));
            }
        }

        [TestMethod]
        public async Task TestGliderStep()
        {
            IModel model = new Model();
            List<Point> points = new List<Point>()
            {
                new Point(0, 0),
                new Point(1, -1),
                new Point(2, -1),
                new Point(2, 0),
                new Point(2, 1),
            };
            model.InsertLiveCells(points);
            await model.Step();
            List<Point> expected = new List<Point>()
            {
                new Point(1, 1),
                new Point(1, -1),
                new Point(2, -1),
                new Point(2, 0),
                new Point(3, 0),
            };
            var liveCellPositions = model.LiveCellPositions();
            Assert.AreEqual(expected.Count, liveCellPositions.Count());
            foreach (var cellPosition in liveCellPositions)
            {
                Assert.IsTrue(expected.Contains(cellPosition));
            }
            await model.Step();
            expected = new List<Point>()
            {
                new Point(2, 1),
                new Point(1, -1),
                new Point(2, -1),
                new Point(3, -1),
                new Point(3, 0),
            };
            liveCellPositions = model.LiveCellPositions();
            Assert.AreEqual(expected.Count, liveCellPositions.Count());
            foreach (var cellPosition in liveCellPositions)
            {
                Assert.IsTrue(expected.Contains(cellPosition));
            }
            await model.Step();
            expected = new List<Point>()
            {
                new Point(1, 0),
                new Point(2, -2),
                new Point(2, -1),
                new Point(3, -1),
                new Point(3, 0),
            };
            liveCellPositions = model.LiveCellPositions();
            Assert.AreEqual(expected.Count, liveCellPositions.Count());
            foreach (var cellPosition in liveCellPositions)
            {
                Assert.IsTrue(expected.Contains(cellPosition));
            }
            await model.Step();
            expected = new List<Point>()
            {
                new Point(1, -1),
                new Point(2, -2),
                new Point(3, -2),
                new Point(3, -1),
                new Point(3, 0),
            };
            liveCellPositions = model.LiveCellPositions();
            Assert.AreEqual(expected.Count, liveCellPositions.Count());
            foreach (var cellPosition in liveCellPositions)
            {
                Assert.IsTrue(expected.Contains(cellPosition));
            }
        }

        [TestMethod]
        public async Task TestManySteps()
        {
            int numPoints = 500;
            int numSteps = 100;

            IModel model = new Model();
            model.InsertLiveCell(new Point(0, 0));
            for (int i = 1; i <= numPoints; i++)
            {
                List<Point> points = new()
                {
                    new Point(-i, -i),
                    new Point(-i, 0),
                    new Point(-i, i),
                    new Point(0, -i),
                    new Point(0, i),
                    new Point(i, -i),
                    new Point(i, 0),
                    new Point(i, i),
                };
                model.InsertLiveCells(points);
            }

            Stopwatch sw = new();
            int totalCells = 0;
            int maxLiveCells = 0;
            sw.Start();

            for(int i= 0; i < numSteps; i++)
            {
                await model.Step();
                totalCells += model.LiveCellsCount();
                maxLiveCells = Math.Max(maxLiveCells, model.LiveCellsCount());
            }

            sw.Stop();
            Console.WriteLine($"average number of live cells {totalCells/numSteps}");
            Console.WriteLine($"maximum number of live cells {maxLiveCells}");
            Console.WriteLine($"average time per step: {sw.Elapsed/numSteps}");
        }
    }
}