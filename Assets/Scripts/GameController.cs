using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public struct GridPosition
{
    public int x, y;

    public GridPosition(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
}

public class GameController : MonoBehaviour
{
    [Header("Technical:")]
    [SerializeField] private GameObject _tilePrefab;
    [Header("For designers:")]
    [Tooltip("Size of square board Tile, depends on tile sprote size.")]
    [SerializeField] private float _tileSize;
    [Tooltip("Every new tile should be added here.")]
    [SerializeField] private ScriptableTile[] _tilesDictionary;
    [Tooltip("Every new player 1's army unit should be added here.")]
    [SerializeField] private GameObject[] _player1UnitPrefabs;
    [Tooltip("Every new player 2's army unit should be added here.")]
    [SerializeField] private GameObject[] _player2UnitPrefabs;

    private BoardGrid _myGrid;
    private List<UnitController> _units;
    public static GameController _instance;

    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        else
        {
            Destroy(this);
        }
    }

    private void OnUnitClicked(GridPosition unitPosition, int moveRange)
    {
        _myGrid.ShowMoveRange(unitPosition, moveRange);
    }

    // Start is called before the first frame update
    void Start()
    {
        string configFilePath = Application.streamingAssetsPath + "/grid.csv";
        string[] gridFile = File.ReadAllLines(configFilePath);
        _myGrid = new BoardGrid(gridFile, _tilesDictionary, _tilePrefab, _tileSize);
        _units = new List<UnitController>();
        _units.Add(Instantiate(_player1UnitPrefabs[0], Vector3.zero, Quaternion.identity).GetComponent<UnitController>());
        _units[0].InitializeUnit(_myGrid.GetTile(1,1));
        EventManager._instance.OnUnitClicked += OnUnitClicked;
    }

    private void OnDestroy()
    {
        EventManager._instance.OnUnitClicked -= OnUnitClicked;
    }
}
