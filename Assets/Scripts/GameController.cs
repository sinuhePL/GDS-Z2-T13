using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GameController : MonoBehaviour
{
    [Header("Technical:")]
    [SerializeField] private GameObject tilePrefab;
    [Header("For designers:")]
    [Tooltip("Size of square board Tile, depends on tile sprote size.")]
    [SerializeField] private float tileSize;
    [Tooltip("Every new tile should be added here.")]
    [SerializeField] private ScriptableTile[] tilesDictionary;

    private BoardGrid _myGrid;

    // Start is called before the first frame update
    void Start()
    {
        string configFilePath = Application.streamingAssetsPath + "/grid.csv";
        string[] gridFile = File.ReadAllLines(configFilePath);
        _myGrid = new BoardGrid(gridFile, tilesDictionary, tilePrefab, tileSize);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
