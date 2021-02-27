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
    [SerializeField] private UIController _myUIController;
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
    private int _activePlayerId;
    private Camera myCamera;
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
        myCamera = Camera.main;
        string configFilePath = Application.streamingAssetsPath + "/grid.csv";
        string[] gridFile = File.ReadAllLines(configFilePath);
        _myGrid = new BoardGrid(gridFile, _tilesDictionary, _tilePrefab, _tileSize);
        _activePlayerId = 1;
        _units = new List<UnitController>();
        for(int i=0; i< _player1UnitPrefabs.Length; i++)
        { 
            _units.Add(Instantiate(_player1UnitPrefabs[i], Vector3.zero, Quaternion.identity).GetComponent<UnitController>());
            _units[i].InitializeUnit(_myGrid.GetTile(0, i));
        }
        for (int i = 0; i < _player2UnitPrefabs.Length; i++)
        {
            _units.Add(Instantiate(_player2UnitPrefabs[i], Vector3.zero, Quaternion.identity).GetComponent<UnitController>());
            _units[i + _player1UnitPrefabs.Length].InitializeUnit(_myGrid.GetTile(_myGrid.GetBoardWidth()-1, _myGrid.GetBoardHeight()-1-i));
        }
        _myGameState = new BeginTurnState(_activePlayerId);
        EventManager._instance.OnUnitClicked += OnUnitClicked;
        EventManager._instance.OnTileClicked += OnTileClicked;
        EventManager._instance.OnTileHovered += OnTileHovered;
        EventManager._instance.OnExecutionEnd += OnExecutionEnded;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePosition = myCamera.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D[] hits = Physics2D.RaycastAll(mousePosition, new Vector2(0, 0), 0.01f);
            SpriteRenderer topRenderer = null;
            foreach(RaycastHit2D hit in hits)
            {
                if(topRenderer == null)
                {
                    topRenderer = hit.collider.gameObject.GetComponent<SpriteRenderer>();
                    continue;
                }
                SpriteRenderer currentRenderer = hit.collider.gameObject.GetComponent<SpriteRenderer>();
                if (currentRenderer.sortingLayerID > topRenderer.sortingLayerID) topRenderer = currentRenderer;
                else if (currentRenderer.sortingLayerID == topRenderer.sortingLayerID)
                {
                    if(currentRenderer.sortingOrder > topRenderer.sortingOrder) topRenderer = currentRenderer;
                }
            }
            if(topRenderer != null)
            {
                IClickable clickedObject = null;
                clickedObject = topRenderer.gameObject.GetComponent<IClickable>();
                if(clickedObject != null) clickedObject.Click();
            }
        }
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

    public UIController GetUI()
    {
        return _myUIController;
    }

    public int GetNextPlayer()
    {
        bool allUnitsNotAvailable = true;
        foreach(UnitController unit in _units)
        {
            if (unit.GetPlayerId() == _activePlayerId && unit._isAvailable) allUnitsNotAvailable = false;
        }
        if (allUnitsNotAvailable)
        {
            _activePlayerId = (_activePlayerId == 1 ? 2 : 1);
            foreach (UnitController unit in _units)
            {
                if (unit.GetPlayerId() == _activePlayerId) unit._isAvailable = true;
            }
        }
        return _activePlayerId;
    }

    public void KillUnit(UnitController killedUnit)
    {
        _units.Remove(killedUnit);
        Destroy(killedUnit.gameObject);
    }

    public void EndTurnAction()
    {
        IGameState newState;
        newState = _myGameState.EndTurnPressed(this);
        if (newState != null)
        {
            _myGameState = newState;
        }
    }

    public void AttackAction()
    {
        IGameState newState;
        newState = _myGameState.AttackPressed(this);
        if (newState != null)
        {
            _myGameState = newState;
        }
    }

    public int GetWinner()
    {
        int player1UnitsCount = 0, player2UnitsCount = 0;
        bool player1KingAlive = false, player2KingAlive = false;
        foreach(UnitController unit in _units)
        {
            if(unit.GetPlayerId() == 1)
            {
                player1UnitsCount++;
                if (unit.IsKing()) player1KingAlive = true;
            }
            if (unit.GetPlayerId() == 2)
            {
                player2UnitsCount++;
                if (unit.IsKing()) player2KingAlive = true;
            }
        }
        if (player1UnitsCount == 0 || player1KingAlive == false) return 2;
        else if (player2UnitsCount == 0 || player2KingAlive == false) return 1;
        else return 0;
    }
}
