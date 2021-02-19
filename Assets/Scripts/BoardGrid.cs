using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardGrid
{
    private int _height, _width;
    private float _tileSize;
    private TileController[,] _gridArray;

    private Vector3 GetWorldPosition(GridPosition gp)
    {
        return new Vector3(gp.x * _tileSize, gp.y * -_tileSize, 0.0f);
    }

    public BoardGrid(string[] gridInfo, ScriptableTile[] tDict, GameObject tilePrefab, float tileSize)
    {
        _height = gridInfo.Length;
        _width = (gridInfo[0].Length+1)/2;
        _gridArray = new TileController[_width, _height];
        _tileSize = tileSize;
        for (int y = 0; y < _gridArray.GetLength(0); y++)
        {
            string[] gridLine = gridInfo[y].Split(',');
            for (int x = 0; x < _gridArray.GetLength(1); x++)
            {
                foreach (ScriptableTile st in tDict)
                {
                    if (gridLine[x] == st.letter)
                    {
                        GridPosition myGridPosition = new GridPosition(x, y);
                        _gridArray[x, y] = Object.Instantiate(tilePrefab, GetWorldPosition(myGridPosition), Quaternion.identity).GetComponent<TileController>();
                        _gridArray[x, y].InitializeTile(st, myGridPosition);
                    }
                }
                if (_gridArray[x, y] == null) Debug.Log("Error: Tile"+x.ToString()+", "+y.ToString()+" not initialized");
            }
        }
    }

    public TileController GetTile(GridPosition tilePosition)
    {
        return _gridArray[tilePosition.x, tilePosition.y];
    }

    public TileController GetTile(int x, int y)
    {
        return _gridArray[x, y];
    }

    public void ShowMoveRange(GridPosition startingPosition, int range)
    {
        for (int y = 0; y < _gridArray.GetLength(0); y++)
        {
            for (int x = 0; x < _gridArray.GetLength(1); x++)
            {
                _gridArray[x, y].HighlightIfInRange(startingPosition, range);
            }
        }
    }
}
