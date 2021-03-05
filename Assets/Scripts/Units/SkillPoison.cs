using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UnitController))]
public class SkillPoison : MonoBehaviour, IUnitSkill
{
    [SerializeField] private int _weaknessAmount;
    [SerializeField] private int _weaknessTurns;
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
        if (target._myWeakness == 0)
        {
            target._myWeakness = _weaknessAmount;
            target._myWeaknessCountdown = _weaknessTurns;
            return -1;
        }
        else return 0;
    }
}
