using StaticData;
using UnityEngine;

public class CellMover
{
    private Cell _movingCell;
    private Point _newPoint;
    private Vector2 _mouseStartPosition;
    private BoardService _boardService;

    public CellMover(BoardService boardService)
    {
        _boardService = boardService;
    }
    
    public void Update()
    {
        if (_movingCell == null)
            return;

        var mousePosition = (Vector2)Input.mousePosition - _mouseStartPosition;
        var mouseDirection = mousePosition.normalized;
        var absoluteDirection = new Vector2(Mathf.Abs(mousePosition.x), Mathf.Abs(mousePosition.y));

        _newPoint = Point.Clone(_movingCell.Point);
        var addPoint = Point.zero;
        if (mousePosition.magnitude > Config.PieceSize / 4)
        {
            if (absoluteDirection.x > absoluteDirection.y)
                addPoint = new Point(mouseDirection.x > 0 ? 1 : -1, 0);
            else
                addPoint = new Point(0, mouseDirection.y > 0 ? -1 : 1);
        }
        _newPoint.Add(addPoint);

        var position = BoardService.GetBoardPositionFromPoint(_movingCell.Point);
        if (!_newPoint.Equals(_movingCell.Point))
            position += Point.Multilpy(new Point(addPoint.x, -addPoint.y), Config.PieceSize / 2).ToVector();
        _movingCell.MoveToPosition(position);
    }
    
    public void MoveCell(Cell cell)
    {
        if (_movingCell != null)
            return;
        _movingCell = cell;
        _mouseStartPosition = Input.mousePosition;
    }

    public void DropCell()
    {
        if (_movingCell == null)
            return;
        if (_newPoint.Equals(_movingCell.Point))
            _boardService.ResetCell(_movingCell);
        else
            _boardService.FlipCells(_movingCell.Point, _newPoint, true);
        _movingCell = null;
    }
}