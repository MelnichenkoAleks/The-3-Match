public class CellData
{
    public enum CellType
    {
        Hole = -1,
        Blank = 0,
        Apple = 1,
        Lemon = 2, 
        Bread = 3,
        Broccoli = 4,
        Coconut = 5
    }
    
    public CellType cellType;
    public Point point;
    private Cell _cell;

    public CellData(CellType cellType, Point point)
    {
        this.cellType = cellType;
        this.point = point;
    }

    public Cell GetCell()
        => _cell;

    public void SetCell(Cell newCell)
    {
        _cell = newCell;
        if (_cell == null)
        {
            cellType = CellType.Blank;
        }
        else
        {
            cellType = newCell.CellType;
            _cell.SetCellPoint(point);
        }
    }
}