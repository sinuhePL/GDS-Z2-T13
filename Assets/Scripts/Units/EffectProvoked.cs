using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UnitController))]
public class EffectProvoked : MonoBehaviour, IValidateTarget, IMoveRangeModifier, IEndturnable
{
    private UnitController _instigator;
    private UnitController _myUnitController;

    public void InitializeEffect(UnitController provokingUnit)
    {
        _instigator = provokingUnit;
        _myUnitController = GetComponent<UnitController>();
    }

    public bool IsTargetValid(UnitController targetUnit)
    {
        if (targetUnit == _instigator) return true;
        else return false;
    }

    public void EndTurnAction(int playerId)
    {
        if(_myUnitController.GetPlayerId() != playerId 
            && (Mathf.Abs(_instigator.GetGridPosition().x - _myUnitController.GetGridPosition().x) > 1 || Mathf.Abs(_instigator.GetGridPosition().y - _myUnitController.GetGridPosition().y) > 1))
        {
            Destroy(this);
        }
    }

    public int GetMoveRangeModifier()
    {
        return -10;
    }
}
