using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardGrid
{
    private int _height, _width;
    private float _tileSize;
    private GridNode[,] _gridArray;
    private Color _inMoveRangeColor, _pathColor, _hoverColor, _inAttackRangeColor;

    private Vector3 GetWorldPosition(GridPosition gp)
    {
        return new Vector3(gp.x * _tileSize, gp.y * -_tileSize, 0.0f);
    }

    #region Pathfinding

    private GridNode GetLowestFCostNode(List<GridNode> nodeList)
    {
        GridNode lowestFCostNode = null;
        foreach(GridNode gn in nodeList)
        {
            if(lowestFCostNode == null || gn._fCost < lowestFCostNode._fCost)
            {
                lowestFCostNode = gn;
            }
        }
        return lowestFCostNode;
    }

    private List<GridNode> GetNeighbourList(GridPosition myPosition)
    {
        List<GridNode> resultList;
        resultList = new List<GridNode>();
        if (myPosition.x > 0) resultList.Add(_gridArray[myPosition.x - 1, myPosition.y]);
        if(myPosition.x < _width - 1) resultList.Add(_gridArray[myPosition.x + 1, myPosition.y]);
        if (myPosition.y > 0) resultList.Add(_gridArray[myPosition.x, myPosition.y - 1]);
        if (myPosition.y < _height - 1) resultList.Add(_gridArray[myPosition.x, myPosition.y + 1]);
        return resultList;
    }

    private List<GridNode> CalculatePath(GridNode endNode)
    {
        List<GridNode> resultPath = new List<GridNode>();
        resultPath.Add(endNode);
        GridNode currentNode = endNode;
        while (currentNode._cameFromNode != null)
        {
            resultPath.Add(currentNode._cameFromNode);
            currentNode = currentNode._cameFromNode;
        }
        resultPath.Reverse();
        return resultPath;
    }

    public List<GridNode> FindPath(GridPosition startPosition, GridPosition endPosition)
    {
        List<GridNode> openList, closedList;

        GridNode startNode = _gridArray[startPosition.x, startPosition.y];
        GridNode endNode = _gridArray[endPosition.x, endPosition.y];
        openList = new List<GridNode> { startNode };
        closedList = new List<GridNode>();
        for (int y = 0; y < _gridArray.GetLength(0); y++)
        {
            for (int x = 0; x < _gridArray.GetLength(1); x++)
            {
                GridNode pathNode = _gridArray[x, y];
                pathNode._gCost = int.MaxValue;
                pathNode.CalculateFCost();
                pathNode._cameFromNode = null;
            }
        }
        startNode._gCost = 0;
        startNode._hCost = CalculateDistance(startNode._nodePosition, endNode._nodePosition);
        startNode.CalculateFCost();
        while (openList.Count > 0)
        {
            GridNode currentNode = GetLowestFCostNode(openList);
            if (currentNode == endNode)
            {
                return CalculatePath(endNode);
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            foreach (GridNode neighbourNode in GetNeighbourList(currentNode._nodePosition))
            {
                if (closedList.Contains(neighbourNode)) continue;
                if (!neighbourNode.isWalkable())
                {
                    closedList.Add(neighbourNode);
                    continue;
                }

                int tempGCost = currentNode._gCost + 1;
                if (tempGCost < neighbourNode._gCost)
                {
                    neighbourNode._cameFromNode = currentNode;
                    neighbourNode._gCost = tempGCost;
                    neighbourNode._hCost = CalculateDistance(neighbourNode._nodePosition, endNode._nodePosition);
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

    public BoardGrid(string[] gridInfo, ScriptableTile[] tDict, GameObject tilePrefab, float tileSize)
    {
        _height = gridInfo.Length;
        _width = (gridInfo[0].Length+1)/2;
        _gridArray = new GridNode[_width, _height];
        _tileSize = tileSize;
        _inMoveRangeColor = new Color(0.0f, 1.0f, 1.0f, 0.25f);
        _inAttackRangeColor = new Color(1.0f, 0.0f, 0.0f, 0.25f);
        _pathColor = new Color(0.0f, 0.0f, 1.0f, 0.25f);
        _hoverColor = new Color(0.0f, 1.0f, 0.0f, 0.25f);
        for (int y = 0; y < _gridArray.GetLength(0); y++)
        {
            string[] gridLine = gridInfo[y].Split(',');
            for (int x = 0; x < _gridArray.GetLength(1); x++)
            {
                foreach (ScriptableTile st in tDict)
                {
                    if (gridLine[x] == st.letter)
                    {
                        GridPosition tempGridPosition = new GridPosition(x, y);
                        TileController tempTileController = Object.Instantiate(tilePrefab, GetWorldPosition(tempGridPosition), Quaternion.identity).GetComponent<TileController>();
                        tempTileController.InitializeTile(st, tempGridPosition);
                        _gridArray[x, y] = new GridNode(tempGridPosition, tempTileController);
                    }
                }
                if (_gridArray[x, y] == null) Debug.Log("Error: Tile"+x.ToString()+", "+y.ToString()+" not initialized");
            }
        }
    }

    public int CalculateDistance(GridPosition start, GridPosition end)
    {
        return Mathf.Abs(start.x - end.x) + Mathf.Abs(start.y - end.y);
    }

    public TileController GetTile(GridPosition tilePosition)
    {
        return _gridArray[tilePosition.x, tilePosition.y]._nodeTile;
    }

    public TileController GetTile(int x, int y)
    {
        return _gridArray[x, y]._nodeTile;
    }

    public void ShowMoveRange(GridPosition startingPosition, int range)
    {
        List<GridNode> pathNodeList;
        for (int y = 0; y < _gridArray.GetLength(0); y++)
        {
            for (int x = 0; x < _gridArray.GetLength(1); x++)
            {
                if(_gridArray[x, y].GetGridDistance(startingPosition) <= range)
                {
                    // skip unwalkable tiles
                    if (!_gridArray[x, y].isWalkable()) continue;
                    pathNodeList = FindPath(startingPosition, _gridArray[x, y]._nodePosition);
                    if (pathNodeList != null && pathNodeList.Count-1 <= range) _gridArray[x, y].Highlight(_inMoveRangeColor);
                }
            }
        }
    }

    public bool IsTileInMoveRange(UnitController myUnit, TileController myTile)
    {
        List<GridNode> pathNodeList;
        pathNodeList = FindPath(myUnit.GetGridPosition(), myTile.GetGridPosition());
        if (pathNodeList != null && pathNodeList.Count - 1 <= myUnit.GetMoveRange()) return true;
        else return false;
    }

    public void ShowPath(UnitController myUnit, TileController myTile)
    {
        List<GridNode> pathNodeList;
        //hide previous path
        ShowMoveRange(myUnit.GetGridPosition(), myUnit.GetMoveRange());
        if (IsTileInMoveRange(myUnit, myTile) && myTile.isWalkable())
        {
            //show new path
            pathNodeList = FindPath(myUnit.GetGridPosition(), myTile.GetGridPosition());
            foreach (GridNode myNode in pathNodeList)
            {
                myNode.Highlight(_pathColor);
            }
        }
        else if (myTile.isWalkable()) myTile.Highlight(_hoverColor);
    }

    public void TileHovered(TileController hoveredTile)
    {
        if(hoveredTile.isWalkable()) hoveredTile.Highlight(_hoverColor);
    }

    public void HideHighlight()
    {
        for (int y = 0; y < _gridArray.GetLength(0); y++)
        {
            for (int x = 0; x < _gridArray.GetLength(1); x++)
            {
                _gridArray[x, y].ClearHighlight();
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

    public void ShowAttackRange(GridPosition startingPosition, int range)
    {
        if(range == 1)  // highlight melee attack range
        {
            if (startingPosition.x > 0)
            {
                _gridArray[startingPosition.x - 1, startingPosition.y].Highlight(_inAttackRangeColor);
                if(startingPosition.y > 0) _gridArray[startingPosition.x - 1, startingPosition.y-1].Highlight(_inAttackRangeColor);
                if(startingPosition.y < _height-1) _gridArray[startingPosition.x - 1, startingPosition.y + 1].Highlight(_inAttackRangeColor);
            }
            if(startingPosition.x < _width - 1)
            {
                _gridArray[startingPosition.x + 1, startingPosition.y].Highlight(_inAttackRangeColor);
                if (startingPosition.y > 0) _gridArray[startingPosition.x + 1, startingPosition.y - 1].Highlight(_inAttackRangeColor);
                if (startingPosition.y < _height - 1) _gridArray[startingPosition.x + 1, startingPosition.y + 1].Highlight(_inAttackRangeColor);
            }
            if(startingPosition.y > 0) _gridArray[startingPosition.x, startingPosition.y - 1].Highlight(_inAttackRangeColor);
            if (startingPosition.y < _height - 1) _gridArray[startingPosition.x, startingPosition.y + 1].Highlight(_inAttackRangeColor);
        }
        else
        {
            // highlight range attack range
        }
        {
            for(int i=1; i<_width; i++)
            {
                if(startingPosition.x + i < _width && i <= range) _gridArray[startingPosition.x + i, startingPosition.y].Highlight(_inAttackRangeColor);
                if (startingPosition.x - i >= 0 && i <= range) _gridArray[startingPosition.x - i, startingPosition.y].Highlight(_inAttackRangeColor);
            }
            for (int i = 1; i < _height; i++)
            {
                if (startingPosition.y + i < _height && i <= range) _gridArray[startingPosition.x, startingPosition.y+i].Highlight(_inAttackRangeColor);
                if (startingPosition.y - i >= 0 && i <= range) _gridArray[startingPosition.x, startingPosition.y-i].Highlight(_inAttackRangeColor);
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
}
