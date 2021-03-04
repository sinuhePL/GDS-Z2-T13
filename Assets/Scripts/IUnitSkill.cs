using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUnitSkill
{
    void EndTurnAction(int playerId);
    int AttackAction(UnitController target);
}
