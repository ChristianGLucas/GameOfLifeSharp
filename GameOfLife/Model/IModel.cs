
using System.Drawing;

namespace GameOfLife.Model
{
    public interface IModel
    {
        public Task Step();

        public int LiveCellsCount();

        public void InsertLiveCell(Point position);

        public void InsertLiveCells(IEnumerable<Point> positions);

        public void RemoveLiveCell(Point position);

        public void RemoveLiveCells(IEnumerable<Point> positions);

        public bool IsAlive(Point position);

        public IEnumerable<Point> LiveCellPositions();

        public void Clear();
    }
}
