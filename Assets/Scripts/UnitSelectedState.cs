using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSelectedState : IGameState
{
    private UnitController _activeUnit;

    public UnitSelectedState(UnitController uc)
    {
        _activeUnit = uc;
    }

    public IGameState TileClicked(GameController myGameController, TileController clickedTile)
    {
        BoardGrid myGrid;

        myGrid = myGameController.GetGrid();
        if (myGrid.IsTileInMoveRange(_activeUnit, clickedTile))
        {
            _activeUnit.MoveUnit(myGrid.FindPath(_activeUnit.GetGridPosition(), clickedTile.GetGridPosition()));
            myGrid.HideHighlight();
            return new ExecutionState();
        }
        else
        {
            myGrid.HideHighlight();
            return new BeginTurnState(_activeUnit);
        }
    }

    public IGameState UnitClicked(GameController myGameController, UnitController clickedUnit)
    {
        BoardGrid myGrid;

        myGrid = myGameController.GetGrid();
        if (_activeUnit.GetPlayerId() != clickedUnit.GetPlayerId() && myGrid.CalculateDistance(_activeUnit.GetGridPosition(), clickedUnit.GetGridPosition()) <= _activeUnit.GetAttackRange())
        {
            if(clickedUnit.DamageUnit(_activeUnit.GetAttackDamage())) myGameController.KillUnit(clickedUnit);
            myGrid.HideHighlight();
            return new BeginTurnState(myGameController.GetNextUnit());
        }
        else return null;
    }

    public IGameState TileHovered(GameController myGameController, TileController hoveredTile)
    {
        BoardGrid myGrid;
        myGrid = myGameController.GetGrid();
        myGrid.ShowPath(_activeUnit, hoveredTile);
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
