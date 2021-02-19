using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class UnitController : MonoBehaviour
{
    [SerializeField] private ScriptableUnit _unit;
    private TileController _actualTile;
    private SpriteRenderer _mySpriteRenderer;
    private int _health;

    // Start is called before the first frame update
    void Start()
    {
        _mySpriteRenderer = GetComponent<SpriteRenderer>();
        _mySpriteRenderer.sprite = _unit.unitSprite;
        _health = _unit.unitHealth;
        gameObject.AddComponent<BoxCollider2D>();
    }

    private void OnMouseDown()
    {
        EventManager._instance.UnitClicked(_actualTile.GetGridPosition(), _unit.moveRange);  
    }

    public void InitializeUnit(TileController initialTile)
    {
        _actualTile = initialTile;
        transform.position = initialTile.transform.position;
    }
}
