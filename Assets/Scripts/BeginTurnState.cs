using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeginTurnState : IGameState
{
    private UnitController _activeUnit;

    public BeginTurnState(UnitController uc)
    {
        _activeUnit = uc;
    }

    public IGameState TileClicked(GameController myGameController, TileController clickedTile)
    {
        return null;
    }

    public IGameState UnitClicked(GameController myGameController, UnitController clickedUnit)
    {
        BoardGrid myGrid;
        if (clickedUnit == _activeUnit)
        {
            myGrid = myGameController.GetGrid();
            myGrid.ShowMoveRange(clickedUnit.GetGridPosition(), clickedUnit.GetMoveRange());
            return new UnitSelectedState(clickedUnit);
        }
        return null;
    }

    public IGameState TileHovered(GameController myGameController, TileController hoveredTile)
    {
        BoardGrid myGrid;
        myGrid = myGameController.GetGrid();
        myGrid.TileHovered(hoveredTile);
        return null;
    }

    public IGameState UnitHovered(GameController myGameController, UnitController hoveredUnit)
    {
        return null;
    }

    public IGameState ExecutionEnd(GameController myGameController)
    {
        return null;
    }
}
