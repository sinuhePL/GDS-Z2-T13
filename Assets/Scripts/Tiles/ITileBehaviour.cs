using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITileBehaviour
{
    void EndTurnAction(int playerId);
    void EnterTileAction(UnitController myUnit);
}
