using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSelectedState : IGameState
{
    private UnitController _activeUnit;

    public AttackSelectedState(UnitController uc, BoardGrid myGrid)
    {
        _activeUnit = uc;
        myGrid.ShowAttackRange(uc.GetGridPosition(), uc.GetAttackRange());
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
        myGrid = myGameController.GetGrid();
        if (_activeUnit.GetPlayerId() != clickedUnit.GetPlayerId() && myGrid.IsTileInAttackRange(_activeUnit, clickedUnit._myTile))
        {
            bool isKilled = clickedUnit.DamageUnit(_activeUnit.GetAttackDamage());
            if (isKilled) myGameController.KillUnit(clickedUnit);
            myGrid.HideHighlight();
            _activeUnit.SetReticle(false);
            _activeUnit._isAvailable = false;
            return new BeginTurnState(myGameController.GetNextPlayer());
        }
        else if (_activeUnit.GetPlayerId() == clickedUnit.GetPlayerId() && _activeUnit != clickedUnit && clickedUnit._isAvailable)
        {
            myGrid.HideHighlight();
            _activeUnit.SetReticle(false);
            _activeUnit._isAvailable = false;
            return new UnitSelectedState(clickedUnit, myGrid);
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
        // disable unit reticle and turn off highlight
        BoardGrid myGrid;
        myGrid = myGameController.GetGrid();
        myGrid.HideHighlight();
        _activeUnit.SetReticle(false);
        _activeUnit._isAvailable = false;
        return new BeginTurnState(myGameController.GetNextPlayer());
    }

    public IGameState AttackPressed(GameController myGameController)
    {
        //nothing happens
        return null;
    }
}
