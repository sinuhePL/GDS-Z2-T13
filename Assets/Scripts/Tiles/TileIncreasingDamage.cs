using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TileController))]
public class TileIncreasingDamage : MonoBehaviour, ITileBehaviour, IEndturnable
{
    [SerializeField] private int _attackBonus;
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

    public void EnterTileAction(UnitController myUnit)
    {
        myUnit._myBonusAttackDamage = _attackBonus;
        myUnit._myBonusAttackRange = 0;
        myUnit._myBonusArmor = 0;
    }
}
