using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class TileArmoring : MonoBehaviour, ITileBehaviour
{
    [SerializeField] private int _armorBonus;
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
            myUnit._myBonusArmor = _armorBonus;
            myUnit._myBonusMoveRange = 0;
        }
    }

    public void MakeInstantAction(UnitController myUnit)
    {
        myUnit._myBonusAttackRange = 0;
        myUnit._myBonusAttackDamage = 0;
    }
}
