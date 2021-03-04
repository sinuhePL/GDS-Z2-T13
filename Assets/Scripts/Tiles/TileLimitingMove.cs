using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TileController))]
public class TileLimitingMove : MonoBehaviour, ITileBehaviour
{
    [SerializeField] private int _movementRangeBonus;
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
            myUnit._myBonusMoveRange = _movementRangeBonus;
        }
    }

    public void InstantAction(UnitController myUnit)
    {
        myUnit._myBonusAttackRange = 0;
        myUnit._myBonusAttackDamage = 0;
        myUnit._myBonusArmor = 0;
    }
}
