using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridNode
{
    public int _gCost { get; set; }
    public int _hCost { get; set; }
    public int _fCost { get; set; }
    public GridNode _cameFromNode { get; set; }
    public GridPosition _nodePosition { get; }
    public TileController _nodeTile { get; }

    public GridNode(GridPosition myPosition, TileController myTileController)
    {
        _nodePosition = myPosition;
        _nodeTile = myTileController;
        _gCost = 0;
        _hCost = 0;
        _fCost = 0;
    }

    public bool isWalkable()
    {
        return _nodeTile.isWalkable();
    }

    public void CalculateFCost()
    {
        _fCost = _gCost + _hCost;
    }

    public int GetGridDistance(GridPosition startingPosition)
    {
        return Mathf.Abs(startingPosition.x - _nodePosition.x) + Mathf.Abs(startingPosition.y - _nodePosition.y);
    }

    public void Highlight(Color highlightColor)
    {
        _nodeTile.Highlight(highlightColor);
    }

    public void ClearHighlight()
    {
        _nodeTile.ClearTile();
    }
}