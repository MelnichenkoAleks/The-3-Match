using System.Collections.Generic;

public class MatchMachine
{
    private readonly BoardService _boardService;

    private readonly Point[] directions =
    {
        Point.up, Point.right, Point.down, Point.left
    };

    public MatchMachine(BoardService boardService)
    {
        _boardService = boardService;
    }

    public List<Point> GetMatchedPoints(Point point, bool main)
    {
        var connectedPoints = new List<Point>();
        var cellTypeAtPoint = _boardService.GetCellTypeAtPoint(point);

        CheckForDirectionMatch(ref connectedPoints, point, cellTypeAtPoint);

        CheckForMiddleOfMatch(ref connectedPoints, point, cellTypeAtPoint);

        CheckForSquareMatch(ref connectedPoints, point, cellTypeAtPoint);

        if (main)
        {
            for (int i = 0; i < connectedPoints.Count; i++)
            {
                AddPoints(ref connectedPoints, GetMatchedPoints(connectedPoints[i], false));
            }
        }
        
        return connectedPoints;
    }

    private void CheckForSquareMatch(ref List<Point> connectedPoints, Point point, CellData.CellType cellTypeAtPoint)
    {
        for (int i = 0; i < 4; i++)
        {
            var square = new List<Point>();

            var nextCellIndex = i + 1;
            nextCellIndex = nextCellIndex >= 3 ? 0 : nextCellIndex;

            Point[] checkPoints =
            {
                Point.Add(point, directions[i]),
                Point.Add(point, directions[nextCellIndex]),
                Point.Add(point, Point.Add(directions[i], directions[nextCellIndex]))
            };

            foreach (var checkPoint in checkPoints)
            {
                if (_boardService.GetCellTypeAtPoint(checkPoint) == cellTypeAtPoint)
                    square.Add(checkPoint);
            }
            
            if (square.Count > 2)
                AddPoints(ref connectedPoints, square);
        }
    }

    private void CheckForMiddleOfMatch(ref List<Point> connectedPoints, Point point, CellData.CellType cellTypeAtPoint)
    {
        for (int i = 0; i < 2; i++)
        {
            var line = new List<Point>();

            Point[] checkPoints =
            {
                Point.Add(point, directions[i]),
                Point.Add(point, directions[i + 2])
            };

            foreach (var checkPoint in checkPoints)
            {
                if (_boardService.GetCellTypeAtPoint(checkPoint) == cellTypeAtPoint)
                    line.Add(checkPoint);
            }
            
            if (line.Count > 1)
                AddPoints(ref connectedPoints, line);
        }
    }

    private void CheckForDirectionMatch(ref List<Point> connectedPoints, Point point, CellData.CellType cellTypeAtPoint)
    {
        foreach (var direction in directions)
        {
            var line = new List<Point>();

            for (int i = 1; i < 3; i++)
            {
                var checkPoint = Point.Add(point, Point.Multilpy(direction, i));
                if (_boardService.GetCellTypeAtPoint(checkPoint) == cellTypeAtPoint)
                {
                    line.Add(checkPoint);
                }
            }
            
            if (line.Count > 1)
                AddPoints(ref connectedPoints, line);
        }
    }

    public static void AddPoints(ref List<Point> points, List<Point> addPoints)
    {
        foreach (var addPoint in addPoints)
        {
            var doAdd = true;
            foreach (var point in points)
            {
                if (point.Equals(addPoint))
                {
                    doAdd = false;
                    break;
                }
            }
            if (doAdd)
                points.Add(addPoint);
        }
    }
}