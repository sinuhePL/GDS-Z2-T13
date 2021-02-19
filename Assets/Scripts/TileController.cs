using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class TileController : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _overlaySpriteRenderer;
    private ScriptableTile _tile;
    private SpriteRenderer _mySpriteRenderer;
    private GridPosition _gridPosition;
    private Color _hoverColor, _previousColor, _rangeColor, _clearColor;

    private void OnMouseEnter()
    {
        if (_tile.isPassable)
        {
            _previousColor = _overlaySpriteRenderer.color;
            _overlaySpriteRenderer.color = _hoverColor;
        }
    }

    private void OnMouseExit()
    {
        if (_tile.isPassable)
        {
            _overlaySpriteRenderer.color = _previousColor;
        }
    }

    private void OnMouseDown()
    {
        EventManager._instance.TileClicked();
    }

    private void ClearTile()
    {
        _overlaySpriteRenderer.color = new Color(1.0f, 1.0f, 1.0f, 0.0f); ;
    }

    private int GetGridDistance(GridPosition startingPosition)
    {
        return Mathf.Abs(startingPosition.x - _gridPosition.x) + Mathf.Abs(startingPosition.y - _gridPosition.y);
    }

    public void InitializeTile(ScriptableTile myScriptableTile, GridPosition position)
    {
        _mySpriteRenderer = GetComponent<SpriteRenderer>();
        _tile = myScriptableTile;
        _mySpriteRenderer.sprite = _tile.tileSprite;
        _gridPosition = position;
        gameObject.AddComponent<BoxCollider2D>();
        _hoverColor = new Color(0.0f, 1.0f, 0.0f, 0.25f);
        _rangeColor = new Color(0.0f, 0.0f, 1.0f, 0.25f);
        _previousColor = _overlaySpriteRenderer.color;
    }

    public GridPosition GetGridPosition()
    {
        return _gridPosition;
    }

    public void HighlightIfInRange(GridPosition unitPosition, int moveRange)
    {
        if (_tile.isPassable)
        {
            if (GetGridDistance(unitPosition) <= moveRange)
            {
                _previousColor = _overlaySpriteRenderer.color;
                _overlaySpriteRenderer.color = _rangeColor;
            }
            else _overlaySpriteRenderer.color = _previousColor;
        }
    }
}
