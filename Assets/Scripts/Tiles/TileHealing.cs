using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TileController))]
public class TileHealing : MonoBehaviour, ITileBehaviour
{
    [SerializeField] private int _healAmount;
    private TileController _myTileController;

    private void Start()
    {
        _myTileController = GetComponent<TileController>();
    }

    public void MakeEndTurnAction(int playerId)
    {
        UnitController myUnit;
        myUnit = _myTileController._myUnit;
        if (myUnit != null && myUnit.GetPlayerId() == playerId)
        {
            myUnit.HealUnit(_healAmount);
            myUnit._myBonusMoveRange = 0;
        }
    }

    public void MakeInstantAction(UnitController myUnit)
    {
        myUnit._myBonusAttackRange = 0;
        myUnit._myBonusAttackDamage = 0;
        myUnit._myBonusArmor = 0;
    }
}
