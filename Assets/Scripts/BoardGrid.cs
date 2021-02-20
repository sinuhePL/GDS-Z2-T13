using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardGrid
{
    private int _height, _width;
    private float _tileSize;
    private GridNode[,] _gridArray;
    private Color _inRangeColor;

    private Vector3 GetWorldPosition(GridPosition gp)
    {
        return new Vector3(gp.x * _tileSize, gp.y * -_tileSize, 0.0f);
    }

    private int CalculateDistance(GridPosition start, GridPosition end)
    {
        return Mathf.Abs(start.x - end.x) + Mathf.Abs(start.y - end.y);
    }

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

    private List<GridNode> FindPath(GridPosition startPosition, GridPosition endPosition)
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

    public BoardGrid(string[] gridInfo, ScriptableTile[] tDict, GameObject tilePrefab, float tileSize)
    {
        _height = gridInfo.Length;
        _width = (gridInfo[0].Length+1)/2;
        _gridArray = new GridNode[_width, _height];
        _tileSize = tileSize;
        _inRangeColor = new Color(0.0f, 0.0f, 1.0f, 0.25f);
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
        List<GridNode> tempNodeList;
        for (int y = 0; y < _gridArray.GetLength(0); y++)
        {
            for (int x = 0; x < _gridArray.GetLength(1); x++)
            {
                if(_gridArray[x, y].GetGridDistance(startingPosition) <= range)
                {
                    tempNodeList = FindPath(startingPosition, _gridArray[x, y]._nodePosition);
                    if (tempNodeList != null && tempNodeList.Count-1 <= range) _gridArray[x, y].Highlight(_inRangeColor);
                }
            }
        }
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
}
