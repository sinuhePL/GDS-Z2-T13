using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UnitController))]
public class SkillPiercing : MonoBehaviour, IUnitSkill
{
    private UnitController _myUnitController;

    private void Awake()
    {
        _myUnitController = GetComponent<UnitController>();
    }

    public void EnterTileAction(TileController newTile)
    { }

    public void EndTurnAction(int playerId)
    { }

    public int AttackAction(UnitController target)
    {
        return target.GetArmor();
    }
}
