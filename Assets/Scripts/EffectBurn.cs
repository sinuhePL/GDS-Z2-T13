using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UnitController))]
public class EffectBurn : MonoBehaviour, IEffect, IEndturnable
{
    private int _damage;
    private int _duration;
    private UnitController _myUnitController;

    public void InitializeEffect(int damage, int duration)
    {
        _damage = damage;
        _duration = duration;
        _myUnitController = GetComponent<UnitController>();
    }

    public int AttackModifier()
    {
        return 0;
    }

    public int ArmorModifier()
    {
        return 0;
    }

    public int MoveRangeModifier()
    {
        return 0;
    }

    public int AttackRangeModifier()
    {
        return 0;
    }

    public int DamageModifier()
    {
        return 0;
    }

    public void EndTurnAction(int playerId)
    {
        if (playerId == _myUnitController.GetPlayerId())
        {
            if (_duration > 0) _myUnitController.DamageUnit(_damage);
            _duration--;
            if (_duration == 0) Destroy(this);
        }
    }
}
