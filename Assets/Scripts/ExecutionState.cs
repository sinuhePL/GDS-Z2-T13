﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExecutionState : IGameState
{
    private UnitController _activeUnit;
    private bool _executionEndsUnitTurn;

    public ExecutionState(UnitController activeUnit, bool endTurn)
    {
        _activeUnit = activeUnit;
        _executionEndsUnitTurn = endTurn;
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
        int newPlayer;

        myGrid = myGameController.GetGrid();
        if (_executionEndsUnitTurn)
        {
            _activeUnit.SetReticle(false);
            _activeUnit._isAvailable = false;
            if (myGameController.MovesDepleted(_activeUnit.GetPlayerId()))
            {
                myGameController.EndPlayerTurn(_activeUnit.GetPlayerId());
                newPlayer = (_activeUnit.GetPlayerId() == 1 ? 2 : 1);
            }
            else
            {
                newPlayer = _activeUnit.GetPlayerId();
            }
            return new BeginTurnState(newPlayer);
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
