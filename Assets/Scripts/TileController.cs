using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class TileController : MonoBehaviour
{
    private ScriptableTile _tile;
    private SpriteRenderer _mySpriteRenderer;

    public void InitializeTile(ScriptableTile myScriptableTile)
    {
        _mySpriteRenderer = GetComponent<SpriteRenderer>();
        _tile = myScriptableTile;
        _mySpriteRenderer.sprite = _tile.tileSprite;
    }
}
