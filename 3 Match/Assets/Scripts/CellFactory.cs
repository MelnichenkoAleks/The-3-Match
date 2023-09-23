using System.Collections.Generic;
using StaticData;
using UnityEngine;

public class CellFactory : MonoBehaviour
{
    private BoardService _boardService;
    private readonly List<KilledCell> _killedCells = new List<KilledCell>();
    
    [Header("BoardRects")]
    [SerializeField] private RectTransform _boardRect;
    [SerializeField] private RectTransform _killedBoardRect;
    
    [Header("Prefabs")]
    [SerializeField] private Cell _cellPrefab;
    [SerializeField] private KilledCell _killedCellPrefab;

    public void InstantiateBoard(BoardService boardService, CellMover cellMover)
    {
        _boardService = boardService;
        for (int y = 0; y < Config.BoardHeight; y++)
        {
            for (int x = 0; x < Config.BoardWidth; x++)
            {
                var point = new Point(x, y);
                var cellData = boardService.GetCellAtPoint(point);
                var cellType = cellData.cellType;
                if (cellType <= 0)
                    continue;
                
                var cell = InstantiateCell();
                cell.rect.anchoredPosition = BoardService.GetBoardPositionFromPoint(point);
                cell.Initialize(
                    new CellData(cellType, new Point(x, y)),
                    boardService.CellSprites[(int)(cellType - 1)],
                    cellMover
                    );
                cellData.SetCell(cell);
            }
        }
    }

    public Cell InstantiateCell()
        => Instantiate(_cellPrefab, _boardRect);

    public void KillCell(Point point)
    {
        var availableCells = new List<KilledCell>();
        foreach (var killedCell in _killedCells)
            if (!killedCell.isFalling)
                availableCells.Add(killedCell);
        KilledCell showedKilledCell;
        if (availableCells.Count > 0)
            showedKilledCell = availableCells[0];
        else
        {
            var killedCel = Instantiate(_killedCellPrefab, _killedBoardRect);
            showedKilledCell = killedCel;
            _killedCells.Add(killedCel);
        }

        var cellTypeIndex = (int)_boardService.GetCellTypeAtPoint(point) - 1;
        if (showedKilledCell != null && cellTypeIndex >= 0 && cellTypeIndex < _boardService.CellSprites.Length)
            showedKilledCell.Initialize(
                _boardService.CellSprites[cellTypeIndex],
                BoardService.GetBoardPositionFromPoint(point)
                );
    }
}