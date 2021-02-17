using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardGrid
{
    private int _height, _width;
    float _tileSize;
    private TileController[,] _gridArray;

    public BoardGrid(string[] gridInfo, ScriptableTile[] tDict, GameObject tilePrefab, float tileSize)
    {
        _height = gridInfo.Length;
        _width = (gridInfo[0].Length+1)/2;
        _tileSize = tileSize;
        _gridArray = new TileController[_width, _height];
        for (int y = 0; y < _gridArray.GetLength(0); y++)
        {
            string[] gridLine = gridInfo[y].Split(',');
            for (int x = 0; x < _gridArray.GetLength(1); x++)
            {
                foreach (ScriptableTile st in tDict)
                {
                    if (gridLine[x] == st.letter)
                    {
                        _gridArray[x, y] = Object.Instantiate(tilePrefab, GetWorldPosition(x, y), Quaternion.identity).GetComponent<TileController>();
                        _gridArray[x, y].InitializeTile(st);
                    }
                }
            }
        }
    }

    private Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x * _tileSize, y * -_tileSize, 0.0f);
    }

}
