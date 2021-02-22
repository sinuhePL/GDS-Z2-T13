using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class UnitController : MonoBehaviour
{
    [SerializeField] private ScriptableUnit _unit;
    [SerializeField] private HealthController _myHealth;
    public TileController _myTile { get; set; }
    private SpriteRenderer _mySpriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        _mySpriteRenderer = GetComponent<SpriteRenderer>();
        _mySpriteRenderer.sprite = _unit.unitSprite;
        gameObject.AddComponent<BoxCollider2D>();
    }

    private void OnMouseDown()
    {
        EventManager._instance.UnitClicked(this);  
    }

    private IEnumerator MakeMove(List<GridNode> movePath)
    {
        GridNode currentNode;
        float step;

        while (movePath.Count > 0)
        {
            currentNode = movePath[0];
            while (Vector3.Distance(currentNode._nodeTile.transform.position, transform.position) > 0.001f)
            {
                step = _unit.moveSpeed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, currentNode._nodeTile.transform.position, step);
                yield return 0;
            }
            _myTile = currentNode._nodeTile;
            movePath.Remove(currentNode);
        }
        EventManager._instance.ExecutionEnded();
    }

    public void InitializeUnit(TileController initialTile)
    {
        _myTile = initialTile;
        transform.position = initialTile.transform.position;
        _myHealth.InitializeHealth(_unit.unitHealth);
    }

    public GridPosition GetGridPosition()
    {
        return _myTile.GetGridPosition();
    }

    public int GetMoveRange()
    {
        return _unit.moveRange;
    }

    public float GetMoveSpeed()
    {
        return _unit.moveSpeed;
    }

    public void MoveUnit(List<GridNode> movePath)
    {
        StartCoroutine(MakeMove(movePath));
    }

    public int GetPlayerId()
    {
        return _unit.playerId;
    }

    public int GetAttackDamage()
    {
        return _unit.attackDamage;
    }

    public int GetAttackRange()
    {
        return _unit.attackRange;
    }

    public bool DamageUnit(int damage)
    {
        return _myHealth.ChangeHealth(-damage);
    }
}
