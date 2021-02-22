using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExecutionState : IGameState
{
    public IGameState TileClicked(GameController myGameController, TileController clickedTile)
    {
        return null;
    }

    public IGameState UnitClicked(GameController myGameController, UnitController clickedUnit)
    {
        return null;
    }

    public IGameState TileHovered(GameController myGameController, TileController hoveredTile)
    {
        return null;
    }

    public IGameState UnitHovered(GameController myGameController, UnitController hoveredUnit)
    {
        return null;
    }

    public IGameState ExecutionEnd(GameController myGameController)
    {
        return new BeginTurnState(myGameController.GetNextUnit());
    }
}
