using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSelectedState : IGameState
{
    private UnitController _activeUnit;

    public AttackSelectedState(UnitController uc, BoardGrid myGrid, UIController ui)
    {
        _activeUnit = uc;
        _activeUnit.SetReticle(true);
        ui.DisplayUnit(uc);
        ui.SelectUnit(uc);
        myGrid.HideHighlight();
        myGrid.ShowAttackRange(uc.GetGridPosition(), uc.GetAttackRange(), _activeUnit.GetPlayerId());
        Debug.Log("Stan: Wybrany atak jednostki gracza: " + _activeUnit.GetPlayerId());
    }

    public IGameState TileClicked(GameController myGameController, TileController clickedTile)
    {
        //nothing happens
        return null;
    }

    public IGameState UnitClicked(GameController myGameController, UnitController clickedUnit)
    {
        BoardGrid myGrid;
        UIController ui;
        bool attackEndsTurn;

        ui = myGameController.GetUI();
        myGrid = myGameController.GetGrid();
        if (_activeUnit.IsTargetValid(clickedUnit) && myGrid.IsTileInAttackRange(_activeUnit, clickedUnit._myTile))
        {
            myGrid.HideHighlight();
            _activeUnit.SetReticle(false);
            _activeUnit._isAvailable = false;
            clickedUnit.StopShowingPotentialDamage();
            _activeUnit.AttackUnit(clickedUnit);
            if (_activeUnit.GetFreeAttackNumber() < 1) attackEndsTurn = true;
            else attackEndsTurn = false;
            return new ExecutionState(_activeUnit, attackEndsTurn);
        }
        else if (_activeUnit.GetPlayerId() == clickedUnit.GetPlayerId() && _activeUnit != clickedUnit && clickedUnit._isAvailable && clickedUnit._isDeployed)
        {
            myGrid.HideHighlight();
            _activeUnit.SetReticle(false);
            if (clickedUnit._hasMoved) return new AttackSelectedState(clickedUnit, myGrid, ui);
            else return new UnitSelectedState(clickedUnit, myGrid, ui);
        }
        else return null;
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
        //show units move and attack ranges
        BoardGrid myGrid;
        UIController ui;

        ui = myGameController.GetUI();
        myGrid = myGameController.GetGrid();
        ui.DisplayUnit(hoveredUnit);
        if (hoveredUnit.GetPlayerId() != _activeUnit.GetPlayerId() && !myGrid.IsTileInAttackRange(_activeUnit, hoveredUnit._myTile))
        {
            myGrid.HideHighlight();
            myGrid.ShowMoveRange(hoveredUnit.GetGridPosition(), hoveredUnit.GetMoveRange());
            myGrid.ShowAttackRange(hoveredUnit.GetGridPosition(), hoveredUnit.GetAttackRange(), hoveredUnit.GetPlayerId());
        }
        else if(hoveredUnit.GetPlayerId() != _activeUnit.GetPlayerId()) hoveredUnit.ShowPotentialDamage(_activeUnit.GetCalculatedAttack(hoveredUnit));
        return null;
    }

    public IGameState UnitUnhovered(GameController myGameController, UnitController unhoveredUnit)
    {
        //clear board
        BoardGrid myGrid;
        UIController ui;

        ui = myGameController.GetUI();
        myGrid = myGameController.GetGrid();
        ui.DisplayUnit(_activeUnit);
        if (unhoveredUnit.GetPlayerId() != _activeUnit.GetPlayerId() && !myGrid.IsTileInAttackRange(_activeUnit, unhoveredUnit._myTile))
        {
            myGrid.HideHighlight();
            myGrid.ShowAttackRange(_activeUnit.GetGridPosition(), _activeUnit.GetAttackRange(), _activeUnit.GetPlayerId());
        }
        else if (unhoveredUnit.GetPlayerId() != _activeUnit.GetPlayerId()) unhoveredUnit.StopShowingPotentialDamage();
        return null;
    }

    public IGameState ExecutionEnd(GameController myGameController)
    {
        //nothing happens
        return null;
    }

    public IGameState EndTurnPressed(GameController myGameController)
    {
        // disable unit reticle and turn off highlight
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

    public IGameState DeploymentPressed(GameController myGameController)
    {
        UIController ui;
        BoardGrid myGrid;
        ui = myGameController.GetUI();
        myGrid = myGameController.GetGrid();
        return new DeploymentState(_activeUnit, myGrid, ui);
    }
}
