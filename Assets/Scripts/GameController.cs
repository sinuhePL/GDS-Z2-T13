﻿using System.Collections;
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

    public static bool operator ==(GridPosition gp1, GridPosition gp2)
    {
        if (gp1.x == gp2.x && gp1.y == gp2.y) return true;
        else return false;
    }

    public static bool operator !=(GridPosition gp1, GridPosition gp2)
    {
        if (gp1.x != gp2.x || gp1.y != gp2.y) return true;
        else return false;
    }

    public override bool Equals(object obj)
    {
        return base.Equals(obj);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}

public class GameController : MonoBehaviour
{
    [Header("Technical:")]
    [SerializeField] private UIController _myUIController;
    [Header("For designers:")]
    [Tooltip("Size of square board Tile, depends on tile sprote size.")]
    [SerializeField] private float _tileSize;
    [Tooltip("Every new tile should be added here.")]
    [SerializeField] private GameObject[] _tilePrefabs;
    [Tooltip("Every new player 1's army unit should be added here.")]
    [SerializeField] private GameObject[] _player1UnitPrefabs;
    [Tooltip("Every new player 2's army unit should be added here.")]
    [SerializeField] private GameObject[] _player2UnitPrefabs;
    [Tooltip("Starting player Id.")]
    [SerializeField] private int _startingPlayer;

    private static GameController _instance;
    private IGameState _myGameState;
    private BoardGrid _myGrid;
    private List<UnitController> _units;
    private Camera myCamera;
    private bool gameEnded;
    private List<UnitController> _unitsKilledThisTurn;

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

    private void OnUnitKilled(UnitController killedUnit)
    {
        int winner;
        if (killedUnit.IsKing())
        {
            gameEnded = true;
            winner = (killedUnit.GetPlayerId() == 1 ? 2 : 1);
            _myGameState = new EndState(this, winner);
        }
        _unitsKilledThisTurn.Add(killedUnit);
    }

    private void OnExecutionEnded(UnitController unit)
    {
        IGameState newState;
        newState = _myGameState.ExecutionEnd(this);
        if (newState != null && !gameEnded)
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
        _myGrid = new BoardGrid(gridFile, _tilePrefabs, _tileSize);
        _units = new List<UnitController>();
        _unitsKilledThisTurn = new List<UnitController>();
        gameEnded = false;
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
        foreach(UnitController unit in _units)
        {
            IEnterTile[] unitEnterTileReactors;
            unitEnterTileReactors = unit.gameObject.GetComponents<IEnterTile>();
            foreach(IEnterTile reactor in unitEnterTileReactors)
            {
                reactor.EnterTileAction(unit._myTile);
            }
        }
        _myGameState = new BeginTurnState(_startingPlayer);
        EventManager._instance.OnUnitClicked += OnUnitClicked;
        EventManager._instance.OnTileClicked += OnTileClicked;
        EventManager._instance.OnTileHovered += OnTileHovered;
        EventManager._instance.OnExecutionEnd += OnExecutionEnded;
        EventManager._instance.OnUnitKilled += OnUnitKilled;
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
        EventManager._instance.OnUnitKilled -= OnUnitKilled;
    }

    public BoardGrid GetGrid()
    {
        return _myGrid;
    }

    public UIController GetUI()
    {
        return _myUIController;
    }

    public bool MovesDepleted(int playerId)
    {
        bool allUnitsNotAvailable = true;
        foreach(UnitController unit in _units)
        {
            if (unit.GetPlayerId() == playerId && unit._isAvailable) allUnitsNotAvailable = false;
        }
        return allUnitsNotAvailable;
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

    public void EndPlayerTurn(int playerId)
    {
        IEndturnable[] endturnableList;

        foreach (UnitController unit in _units)
        {
            if (unit.GetPlayerId() != playerId) unit._isAvailable = true;
            endturnableList = unit.gameObject.GetComponents<IEndturnable>();
            if (endturnableList.Length > 0)
            {
                foreach (IEndturnable endturnObject  in endturnableList)
                {
                    endturnObject.EndTurnAction(playerId);
                }
            }
        }
        _myGrid.MakeEndTurnActions(playerId);
        foreach(UnitController killedUnit in _unitsKilledThisTurn)
        {
            _units.Remove(killedUnit);
            Destroy(killedUnit.gameObject);
        }
        _unitsKilledThisTurn.Clear();
    }
}
