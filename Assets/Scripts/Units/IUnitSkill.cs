using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUnitSkill
{
    void EnterTileAction(TileController newTile);
    int AttackAction(UnitController target);
}
