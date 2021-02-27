using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeginTurnState : IGameState
{
    private int _activePlayerId;

    public BeginTurnState(int playerId)
    {
        _activePlayerId = playerId;
        Debug.Log("Stan: Początek tury dla gracza: " + _activePlayerId);
    }

    public IGameState TileClicked(GameController myGameController, TileController clickedTile)  
    {
        //nothing happens
        return null;
    }

    public IGameState UnitClicked(GameController myGameController, UnitController clickedUnit)
    {
        //if it's active player's unit and this unit is available, change state to selected unit 
        BoardGrid myGrid;
        if (clickedUnit.GetPlayerId() == _activePlayerId && clickedUnit._isAvailable)
        {
            myGrid = myGameController.GetGrid();
            return new UnitSelectedState(clickedUnit, myGrid);
        }
        return null;
    }

    public IGameState TileHovered(GameController myGameController, TileController hoveredTile)
    {
        //highlight tile
        BoardGrid myGrid;
        myGrid = myGameController.GetGrid();
        myGrid.TileHovered(hoveredTile);
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
        //nothing happens
        return null;
    }

    public IGameState AttackPressed(GameController myGameController)
    {
        //nothing happens
        return null;
    }
}
