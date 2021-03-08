using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UnitController))]
public class EffectIncreaseArmor : MonoBehaviour, IArmorModifier, IEnterTile
{
    private int _increaseAmount;
    private UnitController _myUnitController;
    private GridPosition _effectPosition;

    public void InitializeEffect(int armor)
    {
        _increaseAmount = armor;
        _myUnitController = GetComponent<UnitController>();
        _effectPosition = _myUnitController.GetGridPosition();
    }

    public int GetArmorModifier()
    {
        return _increaseAmount;
    }

    public void EnterTileAction(TileController newTile)
    {
        if (newTile.GetGridPosition() != _effectPosition) Destroy(this);
    }
}
