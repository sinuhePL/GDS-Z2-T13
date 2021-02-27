using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExecutionState : IGameState
{
    private UnitController _activeUnit;
    private bool _executionEndsTurn;

    public ExecutionState(UnitController activeUnit, bool endTurn)
    {
        _activeUnit = activeUnit;
        _executionEndsTurn = endTurn;
        _activeUnit.SetReticle(false);
        Debug.Log("Stan: Wykonuję akcję ruchu jednostki gracza: " + _activeUnit.GetPlayerId());
    }

    public IGameState TileClicked(GameController myGameController, TileController clickedTile)
    {
        //nothing happens
        return null;
    }

    public IGameState UnitClicked(GameController myGameController, UnitController clickedUnit)
    {
        //nothing happens
        return null;
    }

    public IGameState TileHovered(GameController myGameController, TileController hoveredTile)
    {
        //nothing happens
        return null;
    }

    public IGameState UnitHovered(GameController myGameController, UnitController hoveredUnit)
    {
        //nothing happens
        return null;
    }

    public IGameState ExecutionEnd(GameController myGameController)
    {
        //if move ended change state to Attack Selected State, if attack ended change state to Begin Turn State
        BoardGrid myGrid;
        myGrid = myGameController.GetGrid();
        if (_executionEndsTurn)
        {
            _activeUnit.SetReticle(false);
            _activeUnit._isAvailable = false;
            return new BeginTurnState(myGameController.GetNextPlayer());
        }
        else return new AttackSelectedState(_activeUnit, myGrid);
    }

    public IGameState EndTurnPressed(GameController myGameController)
    {
        //nothing happens
        return null;
    }

    public IGameState AttackPressed(GameController myGameController)
    {
        //nothing happens
        return null;
    }
}
