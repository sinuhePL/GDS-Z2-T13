using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UnitController))]
public class EffectPoison : MonoBehaviour, IDamageModifier, IEndturnable
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

    public int GetDamageModifier()
    {
        return _damage;
    }

    public void EndTurnAction(int playerId)
    {
        if (playerId == _myUnitController.GetPlayerId())
        {
            _duration--;
            if (_duration == 0) Destroy(this);
        }
    }
}
