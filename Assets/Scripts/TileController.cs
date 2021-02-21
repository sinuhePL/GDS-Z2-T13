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
    private Color _previousColor;

    private void OnMouseEnter()
    {
        EventManager._instance.TileHovered(this);
    }

    private void OnMouseExit()
    {
        if (_tile.isWalkable)
        {
            _overlaySpriteRenderer.color = _previousColor;
        }
    }

    private void OnMouseDown()
    {
        EventManager._instance.TileClicked(this);
    }

    public void InitializeTile(ScriptableTile myScriptableTile, GridPosition position)
    {
        _mySpriteRenderer = GetComponent<SpriteRenderer>();
        _tile = myScriptableTile;
        _mySpriteRenderer.sprite = _tile.tileSprite;
        _gridPosition = position;
        gameObject.AddComponent<BoxCollider2D>();
        _previousColor = _overlaySpriteRenderer.color;
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
    }

    public void Highlight(Color highlightColor)
    {
        _previousColor = _overlaySpriteRenderer.color;
        _overlaySpriteRenderer.color = highlightColor;
    }
}
