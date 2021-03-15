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
        UIController ui;
        if (clickedUnit.GetPlayerId() == _activePlayerId && clickedUnit._isAvailable)
        {
            myGrid = myGameController.GetGrid();
            ui = myGameController.GetUI();
            return new UnitSelectedState(clickedUnit, myGrid, ui);
        }
        return null;
    }

    public IGameState TileHovered(GameController myGameController, TileController hoveredTile)
    {
        //highlight tile
        BoardGrid myGrid;
        UIController ui;
        UnitTilePanelController infoPanel;
        myGrid = myGameController.GetGrid();
        myGrid.TileHovered(hoveredTile);
        ui = myGameController.GetUI();
        infoPanel = ui.GetInfoPanel();
        infoPanel.DisplayTile(hoveredTile);
        return null;
    }

    public IGameState UnitHovered(GameController myGameController, UnitController hoveredUnit)
    {
        //show units move and attack ranges
        BoardGrid myGrid;
        UnitTilePanelController infoPanel;
        UIController ui;
        myGrid = myGameController.GetGrid();
        myGrid.ShowMoveRange(hoveredUnit.GetGridPosition(), hoveredUnit.GetMoveRange());
        myGrid.ShowAttackRange(hoveredUnit.GetGridPosition(), hoveredUnit.GetAttackRange(), hoveredUnit.GetPlayerId());
        ui = myGameController.GetUI();
        infoPanel = ui.GetInfoPanel();
        infoPanel.DisplayUnit(hoveredUnit);
        return null;
    }

    public IGameState UnitUnhovered(GameController myGameController, UnitController unhoveredUnit)
    {
        //clear board
        BoardGrid myGrid;
        myGrid = myGameController.GetGrid();
        myGrid.HideHighlight();
        return null;
    }

    public IGameState ExecutionEnd(GameController myGameController)
    {
        //nothing happens
        return null;
    }

    public IGameState EndTurnPressed(GameController myGameController)
    {
        //change player turn
        int newPlayer;

        myGameController.EndPlayerTurn(_activePlayerId);
        newPlayer = (_activePlayerId == 1 ? 2 : 1);
        return new BeginTurnState(newPlayer);
    }

    public IGameState AttackPressed(GameController myGameController)
    {
        //nothing happens
        return null;
    }
}
