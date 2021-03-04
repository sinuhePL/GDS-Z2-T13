using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TileController))]
public class TileIncreasingRange : MonoBehaviour, ITileBehaviour
{
    [SerializeField] private int _attackRangeBonus;
    private TileController _myTileController;

    private void Start()
    {
        _myTileController = GetComponent<TileController>();
    }

    public void EndTurnAction(int playerId)
    {
        UnitController myUnit;
        myUnit = _myTileController._myUnit;
        if (myUnit != null && myUnit.GetPlayerId() == playerId)
        {
            myUnit._myBonusMoveRange = 0;
        }
    }

    public void InstantAction(UnitController myUnit)
    {
        myUnit._myBonusAttackRange = _attackRangeBonus;
        myUnit._myBonusAttackDamage = 0;
        myUnit._myBonusArmor = 0;
    }
}
