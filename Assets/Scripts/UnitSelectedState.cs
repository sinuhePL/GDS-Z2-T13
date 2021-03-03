using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSelectedState : IGameState
{
    private UnitController _activeUnit;

    public UnitSelectedState(UnitController uc, BoardGrid myGrid)
    {
        _activeUnit = uc;
        _activeUnit.SetReticle(true);
        myGrid.ShowMoveRange(_activeUnit.GetGridPosition(), _activeUnit.GetMoveRange());
        Debug.Log("Stan: Wybrana jednostka gracza: " + _activeUnit.GetPlayerId());
    }

    public IGameState TileClicked(GameController myGameController, TileController clickedTile)
    {
        BoardGrid myGrid;
        // if tile in move range change state to execution, if not go back to begin turn state
        myGrid = myGameController.GetGrid();
        if (myGrid.IsTileInMoveRange(_activeUnit, clickedTile))
        {
            _activeUnit.MoveUnit(myGrid.FindPath(_activeUnit.GetGridPosition(), clickedTile.GetGridPosition()));
            myGrid.HideHighlight();
            return new ExecutionState(_activeUnit, false);
        }
        else
        {
            myGrid.HideHighlight();
            _activeUnit.SetReticle(false);
            return new BeginTurnState(_activeUnit.GetPlayerId());
        }
    }

    public IGameState UnitClicked(GameController myGameController, UnitController clickedUnit)
    {
        BoardGrid myGrid;
        // if it's active player's unit, change state to selected unit if not go back to begin turn state
        myGrid = myGameController.GetGrid();
        if (_activeUnit.GetPlayerId() == clickedUnit.GetPlayerId() && _activeUnit != clickedUnit && clickedUnit._isAvailable)
        {
            myGrid.HideHighlight();
            _activeUnit.SetReticle(false);
            return new UnitSelectedState(clickedUnit, myGrid);
        }
        else if(_activeUnit.GetPlayerId() != clickedUnit.GetPlayerId())
        {
            myGrid.HideHighlight();
            _activeUnit.SetReticle(false);
            return new BeginTurnState(_activeUnit.GetPlayerId());
        }
        else return null;
    }

    public IGameState TileHovered(GameController myGameController, TileController hoveredTile)
    {
        // highlight tile
        BoardGrid myGrid;
        myGrid = myGameController.GetGrid();
        myGrid.ShowPath(_activeUnit, hoveredTile);
        return null;
    }

    public IGameState UnitHovered(GameController myGameController, UnitController hoveredUnit)
    {
        //nothing happens
        return null;
    }

    public IGameState ExecutionEnd(GameController myGameController)
    {
        //nothing happens
        return null;
    }

    public IGameState EndTurnPressed(GameController myGameController)
    {
        // disable unit reticle
        BoardGrid myGrid;
        int newPlayer;

        myGrid = myGameController.GetGrid();
        myGrid.HideHighlight();
        _activeUnit.SetReticle(false);
        _activeUnit._isAvailable = false;
        myGameController.EndPlayerTurn(_activeUnit.GetPlayerId());
        newPlayer = (_activeUnit.GetPlayerId() == 1 ? 2 : 1);
        return new BeginTurnState(newPlayer);
    }

    public IGameState AttackPressed(GameController myGameController)
    {
        //change state to Attack Selected State
        BoardGrid myGrid;
        myGrid = myGameController.GetGrid();
        myGrid.HideHighlight();
        return new AttackSelectedState(_activeUnit, myGrid);
    }
}
