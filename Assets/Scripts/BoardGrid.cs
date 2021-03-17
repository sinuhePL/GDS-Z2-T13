using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardGrid
{
    private int _height, _width;
    private float _tileSize;
    private TileController[,] _gridArray;
    private Color _inMoveRangeColor, _pathColor, _hoverColor, _inAttackRangeColor, _deploymentZoneColor;

    private Vector3 GetWorldPosition(GridPosition gp)
    {
        return new Vector3(gp.x * _tileSize, gp.y * -_tileSize, 0.0f);
    }

    #region Pathfinding

    private TileController GetLowestFCostNode(List<TileController> nodeList)
    {
        TileController lowestFCostNode = null;
        foreach(TileController gn in nodeList)
        {
            if(lowestFCostNode == null || gn._fCost < lowestFCostNode._fCost)
            {
                lowestFCostNode = gn;
            }
        }
        return lowestFCostNode;
    }

    private List<TileController> GetNeighbourList(GridPosition myPosition)
    {
        List<TileController> resultList;
        resultList = new List<TileController>();
        if (myPosition.x > 0) resultList.Add(_gridArray[myPosition.x - 1, myPosition.y]);
        if(myPosition.x < _width - 1) resultList.Add(_gridArray[myPosition.x + 1, myPosition.y]);
        if (myPosition.y > 0) resultList.Add(_gridArray[myPosition.x, myPosition.y - 1]);
        if (myPosition.y < _height - 1) resultList.Add(_gridArray[myPosition.x, myPosition.y + 1]);
        return resultList;
    }

    private List<TileController> CalculatePath(TileController endNode)
    {
        List<TileController> resultPath = new List<TileController>();
        resultPath.Add(endNode);
        TileController currentNode = endNode;
        while (currentNode._cameFromNode != null)
        {
            resultPath.Add(currentNode._cameFromNode);
            currentNode = currentNode._cameFromNode;
        }
        resultPath.Reverse();
        return resultPath;
    }

    private int CalculateDistance(GridPosition start, GridPosition end)
    {
        return Mathf.Abs(start.x - end.x) + Mathf.Abs(start.y - end.y);
    }

    public List<TileController> FindPath(GridPosition startPosition, GridPosition endPosition)
    {
        List<TileController> openList, closedList;

        TileController startNode = _gridArray[startPosition.x, startPosition.y];
        TileController endNode = _gridArray[endPosition.x, endPosition.y];
        openList = new List<TileController> { startNode };
        closedList = new List<TileController>();
        for (int y = 0; y < _gridArray.GetLength(0); y++)
        {
            for (int x = 0; x < _gridArray.GetLength(1); x++)
            {
                TileController pathNode = _gridArray[x, y];
                pathNode._gCost = int.MaxValue;
                pathNode.CalculateFCost();
                pathNode._cameFromNode = null;
            }
        }
        startNode._gCost = 0;
        startNode._hCost = CalculateDistance(startNode.GetGridPosition(), endNode.GetGridPosition());
        startNode.CalculateFCost();
        while (openList.Count > 0)
        {
            TileController currentNode = GetLowestFCostNode(openList);
            if (currentNode == endNode)
            {
                return CalculatePath(endNode);
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            foreach (TileController neighbourNode in GetNeighbourList(currentNode.GetGridPosition()))
            {
                if (closedList.Contains(neighbourNode)) continue;
                if (!neighbourNode.isWalkable() || neighbourNode._isOccupied)
                {
                    closedList.Add(neighbourNode);
                    continue;
                }

                int tempGCost = currentNode._gCost + 1;
                if (tempGCost < neighbourNode._gCost)
                {
                    neighbourNode._cameFromNode = currentNode;
                    neighbourNode._gCost = tempGCost;
                    neighbourNode._hCost = CalculateDistance(neighbourNode.GetGridPosition(), endNode.GetGridPosition());
                    neighbourNode.CalculateFCost();

                    if (!openList.Contains(neighbourNode))
                    {
                        openList.Add(neighbourNode);
                    }
                }
            }
        }
        return null;
    }

    #endregion

    public BoardGrid(string[] gridInfo, GameObject[] tilePrefabs, float tileSize)
    {
        _height = gridInfo.Length;
        _width = (gridInfo[0].Length+1)/2;
        _gridArray = new TileController[_width, _height];
        _tileSize = tileSize;
        _inMoveRangeColor = new Color(0.0f, 1.0f, 1.0f, 0.25f);
        _inAttackRangeColor = new Color(1.0f, 0.0f, 0.0f, 0.25f);
        _pathColor = new Color(0.0f, 0.0f, 1.0f, 0.25f);
        _hoverColor = new Color(0.0f, 1.0f, 0.0f, 0.25f);
        _deploymentZoneColor = new Color(1.0f, 1.0f, 0.0f, 0.25f);
        for (int y = 0; y < _gridArray.GetLength(0); y++)
        {
            string[] gridLine = gridInfo[y].Split(',');
            for (int x = 0; x < _gridArray.GetLength(1); x++)
            {
                foreach (GameObject g in tilePrefabs)
                {
                    TileController tc = g.GetComponent<TileController>();
                    if (gridLine[x] == tc.GetLetter())// do zmiany
                    {
                        GridPosition tempGridPosition = new GridPosition(x, y);
                        TileController tempTileController = Object.Instantiate(g, GetWorldPosition(tempGridPosition), Quaternion.identity).GetComponent<TileController>();
                        tempTileController.InitializeTile(tempGridPosition, this);
                        _gridArray[x, y] = tempTileController;
                    }
                }
                if (_gridArray[x, y] == null) Debug.Log("Error: Tile"+x.ToString()+", "+y.ToString()+" not initialized");
            }
        }
    }

    public TileController GetTile(GridPosition tilePosition)
    {
        if (tilePosition.x < 0 || tilePosition.y < 0 || tilePosition.x >= _width || tilePosition.y >= _height) return null;
        else return _gridArray[tilePosition.x, tilePosition.y];
    }

    public TileController GetTile(int x, int y)
    {
        if (x < 0 || y < 0 || x >= _width || y >= _height) return null;
        return _gridArray[x, y];
    }

    public void ShowMoveRange(GridPosition startingPosition, int range)
    {
        List<TileController> pathNodeList;
        for (int y = 0; y < _gridArray.GetLength(0); y++)
        {
            for (int x = 0; x < _gridArray.GetLength(1); x++)
            {
                if(_gridArray[x, y].GetGridDistance(startingPosition) <= range)
                {
                    // skip unwalkable tiles
                    if (!_gridArray[x, y].isWalkable()) continue;
                    pathNodeList = FindPath(startingPosition, _gridArray[x, y].GetGridPosition());
                    if (pathNodeList != null && pathNodeList.Count-1 <= range) _gridArray[x, y].Highlight(_inMoveRangeColor, false);
                }
            }
        }
    }

    public bool IsTileInMoveRange(UnitController myUnit, TileController myTile)
    {
        List<TileController> pathNodeList;
        pathNodeList = FindPath(myUnit.GetGridPosition(), myTile.GetGridPosition());
        if (pathNodeList != null && pathNodeList.Count - 1 <= myUnit.GetMoveRange()) return true;
        else return false;
    }

    public void ShowPath(UnitController myUnit, TileController myTile)
    {
        List<TileController> pathNodeList;
        //hide previous path
        ShowMoveRange(myUnit.GetGridPosition(), myUnit.GetMoveRange());
        if (IsTileInMoveRange(myUnit, myTile) && myTile.isWalkable())
        {
            //show new path
            pathNodeList = FindPath(myUnit.GetGridPosition(), myTile.GetGridPosition());
            foreach (TileController myNode in pathNodeList)
            {
                myNode.Highlight(_pathColor, false);
            }
        }
        else if (myTile.isWalkable()) myTile.Highlight(_hoverColor, false);
    }

    public void TileHovered(TileController hoveredTile)
    {
        if(hoveredTile.isWalkable()) hoveredTile.Highlight(_hoverColor, false);
    }

    public void HideHighlight()
    {
        for (int y = 0; y < _gridArray.GetLength(0); y++)
        {
            for (int x = 0; x < _gridArray.GetLength(1); x++)
            {
                _gridArray[x, y].ClearTile();
            }
        }
    }

    public int GetBoardWidth()
    {
        return _width;
    }

    public int GetBoardHeight()
    {
        return _height;
    }

    public void ShowAttackRange(GridPosition startingPosition, int range, int playerId)
    {
        if(range == 1)  // highlight melee attack range
        {
            if (startingPosition.x > 0)
            {
                _gridArray[startingPosition.x - 1, startingPosition.y].Highlight(_inAttackRangeColor, true, playerId);
                if(startingPosition.y > 0) _gridArray[startingPosition.x - 1, startingPosition.y-1].Highlight(_inAttackRangeColor, true, playerId);
                if(startingPosition.y < _height-1) _gridArray[startingPosition.x - 1, startingPosition.y + 1].Highlight(_inAttackRangeColor, true, playerId);
            }
            if(startingPosition.x < _width - 1)
            {
                _gridArray[startingPosition.x + 1, startingPosition.y].Highlight(_inAttackRangeColor, true, playerId);
                if (startingPosition.y > 0) _gridArray[startingPosition.x + 1, startingPosition.y - 1].Highlight(_inAttackRangeColor, true, playerId);
                if (startingPosition.y < _height - 1) _gridArray[startingPosition.x + 1, startingPosition.y + 1].Highlight(_inAttackRangeColor, true, playerId);
            }
            if(startingPosition.y > 0) _gridArray[startingPosition.x, startingPosition.y - 1].Highlight(_inAttackRangeColor, true, playerId);
            if (startingPosition.y < _height - 1) _gridArray[startingPosition.x, startingPosition.y + 1].Highlight(_inAttackRangeColor, true, playerId);
        }
        else
        {
            // highlight range attack range
        }
        {
            for(int i=1; i<_width; i++)
            {
                if(startingPosition.x + i < _width && i <= range) _gridArray[startingPosition.x + i, startingPosition.y].Highlight(_inAttackRangeColor, true, playerId);
                if (startingPosition.x - i >= 0 && i <= range) _gridArray[startingPosition.x - i, startingPosition.y].Highlight(_inAttackRangeColor, true, playerId);
            }
            for (int i = 1; i < _height; i++)
            {
                if (startingPosition.y + i < _height && i <= range) _gridArray[startingPosition.x, startingPosition.y+i].Highlight(_inAttackRangeColor, true, playerId);
                if (startingPosition.y - i >= 0 && i <= range) _gridArray[startingPosition.x, startingPosition.y-i].Highlight(_inAttackRangeColor, true, playerId);
            }
        }
    }

    public bool IsTileInAttackRange(UnitController myUnit, TileController targetTile)
    {
        if(myUnit.GetAttackRange() == 1)
        {
            if (Mathf.Abs(myUnit.GetGridPosition().x - targetTile.GetGridPosition().x) <= 1 && Mathf.Abs(myUnit.GetGridPosition().y - targetTile.GetGridPosition().y) <= 1) return true;
            else return false;
        }
        else
        {
            if (myUnit.GetGridPosition().x == targetTile.GetGridPosition().x && Mathf.Abs(myUnit.GetGridPosition().y - targetTile.GetGridPosition().y) <= myUnit.GetAttackRange()
                || myUnit.GetGridPosition().y == targetTile.GetGridPosition().y && Mathf.Abs(myUnit.GetGridPosition().x - targetTile.GetGridPosition().x) <= myUnit.GetAttackRange()) return true;
            else return false;
        }
    }

    public void MakeEndTurnActions(int playerId)
    {
        IEndturnable myTileEndTurn;
        for (int y = 0; y < _gridArray.GetLength(0); y++)
        {
            for (int x = 0; x < _gridArray.GetLength(1); x++)
            {
                myTileEndTurn = _gridArray[x, y].gameObject.GetComponent<IEndturnable>();
                if (myTileEndTurn != null) myTileEndTurn.EndTurnAction(playerId);
            }
        }
    }

    public void ShowDeploymentZone(TileController startingTile)
    {
        GridPosition startingPosition;
        startingPosition = startingTile.GetGridPosition();
        if (startingPosition.x > 0 && !_gridArray[startingPosition.x - 1, startingPosition.y]._isOccupied) _gridArray[startingPosition.x - 1, startingPosition.y].Highlight(_deploymentZoneColor, false);
        if(startingPosition.x < _width - 1 && !_gridArray[startingPosition.x + 1, startingPosition.y]._isOccupied) _gridArray[startingPosition.x + 1, startingPosition.y].Highlight(_deploymentZoneColor, false);
        if (startingPosition.y > 0 && !_gridArray[startingPosition.x, startingPosition.y - 1]._isOccupied) _gridArray[startingPosition.x, startingPosition.y - 1].Highlight(_deploymentZoneColor, false);
        if (startingPosition.y < _height - 1 && !_gridArray[startingPosition.x, startingPosition.y + 1]._isOccupied) _gridArray[startingPosition.x, startingPosition.y + 1].Highlight(_deploymentZoneColor, false);
    }
}
