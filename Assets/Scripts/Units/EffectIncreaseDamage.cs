using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UnitController))]
public class EffectIncreaseDamage : MonoBehaviour, IEnterTile, IAttackModifier
{
    private int _increaseAmount;
    private UnitController _myUnitController;
    private GridPosition _effectPosition;

    public void InitializeEffect(int damage)
    {
        _increaseAmount = damage;
        _myUnitController = GetComponent<UnitController>();
        _effectPosition = _myUnitController.GetGridPosition();
    }

    public int GetAttackModifier(UnitController target)
    {
        return _increaseAmount;
    }

    public void EnterTileAction(TileController newTile)
    {
        if (newTile.GetGridPosition() != _effectPosition) Destroy(this);
    }
}
