using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeploymentState : IGameState
{
    private UnitController _activeUnit;
    private UnitController _unitToDeploy;

    private bool IsTileInDeploymentZone(TileController startingTile, TileController checkedTile)
    {
        GridPosition startingPosition, checkedPosition;
        startingPosition = startingTile.GetGridPosition();
        checkedPosition = checkedTile.GetGridPosition();
        if (Mathf.Abs(startingPosition.x - checkedPosition.x) == 1 && Mathf.Abs(startingPosition.y - checkedPosition.y) == 0 
            || Mathf.Abs(startingPosition.y - checkedPosition.y) == 1 && Mathf.Abs(startingPosition.x - checkedPosition.x) == 0) return true;
        else return false;
    }

    public DeploymentState(UnitController unit, BoardGrid myGrid, UIController ui)
    {
        _unitToDeploy = null;
        _activeUnit = unit;
        myGrid.HideHighlight();
        if(unit.IsKing())
        {
            ui.ShowDeployableUnits();
        }
    }

    public IGameState TileClicked(GameController myGameController, TileController clickedTile)
    {
        UIController ui;
        BoardGrid myGrid;
        int newPlayer;

        ui = myGameController.GetUI();
        myGrid = myGameController.GetGrid();
        GridPosition tilePosition;
        tilePosition = clickedTile.GetGridPosition();
        if (_unitToDeploy == null)
        {
            ui.EndDeployment();
            if (_activeUnit._hasMoved) _activeUnit._isAvailable = false;
            return new UnitSelectedState(_activeUnit, myGrid, ui);
        }
        else if (IsTileInDeploymentZone(_activeUnit._myTile, clickedTile) && !clickedTile._isOccupied)
        {
            _unitToDeploy.DeployUnit(clickedTile);
            ui.MarkUnitUnavailable(_unitToDeploy);
            _unitToDeploy._isDeployed = true;
            _unitToDeploy._isAvailable = false;
            _activeUnit.SetReticle(false);
            _activeUnit._isAvailable = false;
            ui.MarkUnitUnavailable(_activeUnit);
            clickedTile.ClearTile();
            ui.EndDeployment();
            myGrid.HideHighlight();
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
        return null;
    }

    public IGameState UnitClicked(GameController myGameController, UnitController clickedUnit)
    {
        UIController ui;
        BoardGrid myGrid;
        ui = myGameController.GetUI();
        myGrid = myGameController.GetGrid();
        if (clickedUnit._isDeployed)
        {
            _activeUnit.SetReticle(false);
            myGrid.HideHighlight();
            ui.EndDeployment();
            if (_activeUnit == clickedUnit && _activeUnit._hasMoved)
            {
                _activeUnit._isAvailable = false;
                ui.MarkUnitUnavailable(_activeUnit);
                ui.SelectUnit(_activeUnit);
                return null;
            }
            if (clickedUnit._hasMoved) return new AttackSelectedState(clickedUnit, myGrid, ui);
            else return new UnitSelectedState(clickedUnit, myGrid, ui);
        }
        else
        {
            _unitToDeploy = clickedUnit;
            ui.DisplayUnit(_unitToDeploy);
            ui.SelectUnit(_unitToDeploy);
            myGrid.ShowDeploymentZone(_activeUnit._myTile);
        }
        return null;
    }

    public IGameState TileHovered(GameController myGameController, TileController hoveredTile)
    {
        BoardGrid myGrid;
        UIController ui;

        myGrid = myGameController.GetGrid();
        myGrid.TileHovered(hoveredTile);
        ui = myGameController.GetUI();
        ui.DisplayTile(hoveredTile);
        return null;
    }

    public IGameState UnitHovered(GameController myGameController, UnitController hoveredUnit)
    {
        UIController ui;

        ui = myGameController.GetUI();
        ui.DisplayUnit(hoveredUnit);
        return null;
    }

    public IGameState UnitUnhovered(GameController myGameController, UnitController unhoveredUnit)
    {
        UIController ui;

        ui = myGameController.GetUI();
        ui.ClearDisplay();
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
        UIController ui;

        ui = myGameController.GetUI();
        myGrid = myGameController.GetGrid();
        myGrid.HideHighlight();
        _activeUnit.SetReticle(false);
        _activeUnit._isAvailable = false;
        ui.MarkUnitUnavailable(_activeUnit);
        myGameController.EndPlayerTurn(_activeUnit.GetPlayerId());
        newPlayer = (_activeUnit.GetPlayerId() == 1 ? 2 : 1);
        ui.EndDeployment();
        return new BeginTurnState(newPlayer);
    }

    public IGameState DeploymentPressed(GameController myGameController)
    {
        //nothing happens
        return null;
    }

    public void ChangeMode(GameController myGameController)
    {
        BoardGrid myGrid;

        myGrid = myGameController.GetGrid();
        myGrid.ChangeMode();
    }
}
