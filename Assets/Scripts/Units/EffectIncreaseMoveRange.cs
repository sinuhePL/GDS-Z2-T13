using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UnitController))]
public class EffectIncreaseMoveRange : MonoBehaviour, IMoveRangeModifier
{
    private int _increaseAmount;
    private UnitController _myUnitController;
    private GridPosition _effectPosition;

    public void InitializeEffect(int range)
    {
        _increaseAmount = range;
        _myUnitController = GetComponent<UnitController>();
        _effectPosition = _myUnitController.GetGridPosition();
    }

    public int GetMoveRangeModifier()
    {
        return _increaseAmount;
    }

    public void EnterTileAction(TileController newTile)
    {
        if (newTile.GetGridPosition() != _effectPosition) Destroy(this);
    }
}
