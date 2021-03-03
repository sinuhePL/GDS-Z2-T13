using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITileBehaviour
{
    void MakeEndTurnAction(int playerId);
    void MakeInstantAction(UnitController myUnit);
}
