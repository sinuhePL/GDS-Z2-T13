using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class TileController : MonoBehaviour, IClickable
{
    public UnitController _myUnit { get; set; }
    public bool _isOccupied { get; set; }
    public int _gCost { get; set; }
    public int _hCost { get; set; }
    public int _fCost { get; set; }
    public TileController _cameFromNode { get; set; }
    [SerializeField] private SpriteRenderer _overlaySpriteRenderer;
    [SerializeField] private SpriteRenderer _crosshairSpriteRenderer;
    [SerializeField] private ScriptableTile _tile;
    private SpriteRenderer _mySpriteRenderer;
    private GridPosition _gridPosition;
    private Color _previousColor;
    private ITileBehaviour _myBehaviour;
    private BoardGrid _myBoard;

    private void OnMouseEnter()
    {
        if(_isOccupied) EventManager._instance.UnitHovered(_myUnit);
        else if (_tile.isWalkable) EventManager._instance.TileHovered(this);
    }

    private void OnMouseExit()
    {
        if (_isOccupied) EventManager._instance.UnitUnhovered(_myUnit);
        if (_tile.isWalkable && !_isOccupied)
        {
            _overlaySpriteRenderer.color = _previousColor;
        }
    }

    public void InitializeTile(GridPosition position, BoardGrid myBoardGrid)
    {
        _mySpriteRenderer = GetComponent<SpriteRenderer>();
        _mySpriteRenderer.sprite = _tile.tileDesignerSprite;
        _crosshairSpriteRenderer.enabled = false;
        _gridPosition = position;
        _previousColor = _overlaySpriteRenderer.color;
        _myUnit = null;
        _isOccupied = false;
        _gCost = 0;
        _hCost = 0;
        _fCost = 0;
        _myBehaviour = GetComponent<ITileBehaviour>();
        _myBoard = myBoardGrid;
    }

    public void CalculateFCost()
    {
        _fCost = _gCost + _hCost;
    }

    public int GetGridDistance(GridPosition startingPosition)
    {
        return Mathf.Abs(startingPosition.x - _gridPosition.x) + Mathf.Abs(startingPosition.y - _gridPosition.y);
    }

    public void Click()
    {
        Debug.Log("Kliknięto na tile");
        if (!_isOccupied) EventManager._instance.TileClicked(this);
    }

    public GridPosition GetGridPosition()
    {
        return _gridPosition;
    }

    public bool isWalkable()
    {
        return _tile.isWalkable;
    }

    public void ClearTile()
    {
        _overlaySpriteRenderer.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        _previousColor = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        _crosshairSpriteRenderer.enabled = false;
    }

    public void Highlight(Color highlightColor, bool showAttackRange, int playerId = 0)
    {
        if (!_isOccupied && showAttackRange && _tile.isWalkable) _crosshairSpriteRenderer.enabled = true;
        else
        {
            if (!_isOccupied && _tile.isWalkable || _isOccupied && playerId != _myUnit.GetPlayerId())
            {
                _previousColor = _overlaySpriteRenderer.color;
                _overlaySpriteRenderer.color = highlightColor;
            }
        }
    }

    public string GetLetter()
    {
        return _tile.letter;
    }

    public TileController GetAnotherTile(GridPosition tilePosition)
    {
        return _myBoard.GetTile(tilePosition);
    }

    public TileController GetAnotherTile(int x, int y)
    {
        return _myBoard.GetTile(x,y);
    }

    public int GetPlayerZone()
    {
        if (_gridPosition.x < _myBoard.GetBoardWidth() / 2) return 1;
        else return 2;
    }

    public string GetTileName()
    {
        return _tile.tileName;
    }

    public string GetDescription()
    {
        return _tile.tileDescription;
    }
}
