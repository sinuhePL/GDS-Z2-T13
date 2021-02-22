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

    private IGameState _myGameState;
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

    private void OnUnitClicked(UnitController clickedUnit)
    {
        IGameState newState;
        newState = _myGameState.UnitClicked(this, clickedUnit);
        if(newState != null)
        {
            _myGameState = newState;
        }
    }

    private void OnTileClicked(TileController clickedTile)
    {
        IGameState newState;
        newState = _myGameState.TileClicked(this, clickedTile);
        if (newState != null)
        {
            _myGameState = newState;
        }
    }

    private void OnTileHovered(TileController hoveredTile)
    {
        IGameState newState;
        newState = _myGameState.TileHovered(this, hoveredTile);
        if (newState != null)
        {
            _myGameState = newState;
        }
    }

    private void OnExecutionEnded()
    {
        IGameState newState;
        newState = _myGameState.ExecutionEnd(this);
        if (newState != null)
        {
            _myGameState = newState;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        string configFilePath = Application.streamingAssetsPath + "/grid.csv";
        string[] gridFile = File.ReadAllLines(configFilePath);
        _myGrid = new BoardGrid(gridFile, _tilesDictionary, _tilePrefab, _tileSize);
        _units = new List<UnitController>();
        _units.Add(Instantiate(_player1UnitPrefabs[0], Vector3.zero, Quaternion.identity).GetComponent<UnitController>());
        _units[0].InitializeUnit(_myGrid.GetTile(2,2));
        _units.Add(Instantiate(_player2UnitPrefabs[0], Vector3.zero, Quaternion.identity).GetComponent<UnitController>());
        _units[1].InitializeUnit(_myGrid.GetTile(8, 8));
        _myGameState = new BeginTurnState(GetNextUnit());
        EventManager._instance.OnUnitClicked += OnUnitClicked;
        EventManager._instance.OnTileClicked += OnTileClicked;
        EventManager._instance.OnTileHovered += OnTileHovered;
        EventManager._instance.OnExecutionEnd += OnExecutionEnded;
    }

    private void OnDestroy()
    {
        EventManager._instance.OnUnitClicked -= OnUnitClicked;
        EventManager._instance.OnTileClicked -= OnTileClicked;
        EventManager._instance.OnTileHovered -= OnTileHovered;
        EventManager._instance.OnExecutionEnd -= OnExecutionEnded;
    }

    public BoardGrid GetGrid()
    {
        return _myGrid;
    }

    public UnitController GetNextUnit()
    {
        UnitController nextUnit;
        nextUnit = _units[0];
        _units.Remove(nextUnit);
        _units.Add(nextUnit);
        return nextUnit;
    }

    public void KillUnit(UnitController killedUnit)
    {
        _units.Remove(killedUnit);
        Destroy(killedUnit.gameObject);
    }
}
