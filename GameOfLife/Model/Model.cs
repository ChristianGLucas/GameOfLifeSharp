
using System.Drawing;

namespace GameOfLife.Model
{
    public class Model : IModel
    {
        private HashSet<Point> _liveCellPositions = new();

        public bool IsAlive(Point position)
        {
            return _liveCellPositions.Contains(position);
        }

        public int LiveCellsCount()
        {
            return _liveCellPositions.Count;
        }

        public void Clear()
        {
            _liveCellPositions.Clear();
        }

        public void InsertLiveCell(Point position)
        {
            _liveCellPositions.Add(position);
        }

        public void InsertLiveCells(IEnumerable<Point> positions)
        {
            foreach(var position in positions)
            {
                _liveCellPositions.Add(position);
            }
        }

        public IEnumerable<Point> LiveCellPositions()
        {
            return _liveCellPositions;
        }

        public void RemoveLiveCell(Point position)
        {
            _liveCellPositions.Remove(position);
        }

        public void RemoveLiveCells(IEnumerable<Point> positions)
        {
            foreach (var position in positions)
            {
                _liveCellPositions.Remove(position);
            }
        }

        public async Task Step()
        {
            List<Action> stepActions = new();
            HashSet<Point> alreadyChecked = new();
            foreach (Point liveCellPosition in _liveCellPositions)
            {
                if (!alreadyChecked.Contains(liveCellPosition))
                {
                    alreadyChecked.Add(liveCellPosition);
                    int liveNeighborCount = LiveNeighborCount(liveCellPosition);
                    if (liveNeighborCount < 2 || liveNeighborCount > 3)
                    {
                        stepActions.Add(() => RemoveLiveCell(liveCellPosition));
                    }
                    List<Point> neighborPositions = Neighbors(liveCellPosition);
                    foreach(Point neighborsPosition in neighborPositions)
                    {
                        if (!alreadyChecked.Contains(neighborsPosition))
                        {
                            alreadyChecked.Add(neighborsPosition);
                            int neighborsLiveNeighborCount = LiveNeighborCount(neighborsPosition);
                            if (IsAlive(neighborsPosition))
                            {
                                if (neighborsLiveNeighborCount < 2 || neighborsLiveNeighborCount > 3)
                                {
                                    stepActions.Add(() => RemoveLiveCell(neighborsPosition));
                                }
                            }
                            else
                            {
                                if (neighborsLiveNeighborCount == 3)
                                {
                                    stepActions.Add(() => InsertLiveCell(neighborsPosition));
                                }
                            }
                        }
                    }
                }
            }
            stepActions.ForEach(stepAction => stepAction());
            await Task.CompletedTask;
        }

        private static List<Point> Neighbors(Point point)
        {
            return new List<Point>
            {
                new Point(point.X-1, point.Y-1),
                new Point(point.X-1, point.Y),
                new Point(point.X-1, point.Y+1),
                new Point(point.X, point.Y-1),
                new Point(point.X, point.Y+1),
                new Point(point.X+1, point.Y-1),
                new Point(point.X+1, point.Y),
                new Point(point.X+1, point.Y+1),
            };
        }

        private int LiveNeighborCount(Point position)
        {
            return Neighbors(position).Count(p => _liveCellPositions.Contains(p));
        }
    }
}
