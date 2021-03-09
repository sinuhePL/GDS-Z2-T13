using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UnitController))]
public class SkillProvoke : MonoBehaviour, IEndturnable
{
    private UnitController _myUnitController;

    private void Awake()
    {
        _myUnitController = GetComponent<UnitController>();
    }

    public void EndTurnAction(int playerId)
    {
        TileController neighbourTile;
        EffectProvoked myEffect;

        neighbourTile = _myUnitController._myTile.GetAnotherTile(_myUnitController.GetGridPosition().x - 1, _myUnitController.GetGridPosition().y - 1);
        if (neighbourTile != null && neighbourTile._isOccupied && neighbourTile._myUnit.GetPlayerId() != _myUnitController.GetPlayerId())
        {
            myEffect = neighbourTile._myUnit.gameObject.AddComponent<EffectProvoked>();
            myEffect.InitializeEffect(_myUnitController);
        }
        neighbourTile = _myUnitController._myTile.GetAnotherTile(_myUnitController.GetGridPosition().x - 1, _myUnitController.GetGridPosition().y);
        if (neighbourTile != null && neighbourTile._isOccupied && neighbourTile._myUnit.GetPlayerId() != _myUnitController.GetPlayerId())
        {
            myEffect = neighbourTile._myUnit.gameObject.AddComponent<EffectProvoked>();
            myEffect.InitializeEffect(_myUnitController);
        }
        neighbourTile = _myUnitController._myTile.GetAnotherTile(_myUnitController.GetGridPosition().x - 1, _myUnitController.GetGridPosition().y + 1);
        if (neighbourTile != null && neighbourTile._isOccupied && neighbourTile._myUnit.GetPlayerId() != _myUnitController.GetPlayerId())
        {
            myEffect = neighbourTile._myUnit.gameObject.AddComponent<EffectProvoked>();
            myEffect.InitializeEffect(_myUnitController);
        }
        neighbourTile = _myUnitController._myTile.GetAnotherTile(_myUnitController.GetGridPosition().x, _myUnitController.GetGridPosition().y - 1);
        if (neighbourTile != null && neighbourTile._isOccupied && neighbourTile._myUnit.GetPlayerId() != _myUnitController.GetPlayerId())
        {
            myEffect = neighbourTile._myUnit.gameObject.AddComponent<EffectProvoked>();
            myEffect.InitializeEffect(_myUnitController);
        }
        neighbourTile = _myUnitController._myTile.GetAnotherTile(_myUnitController.GetGridPosition().x, _myUnitController.GetGridPosition().y + 1);
        if (neighbourTile != null && neighbourTile._isOccupied && neighbourTile._myUnit.GetPlayerId() != _myUnitController.GetPlayerId())
        {
            myEffect = neighbourTile._myUnit.gameObject.AddComponent<EffectProvoked>();
            myEffect.InitializeEffect(_myUnitController);
        }
        neighbourTile = _myUnitController._myTile.GetAnotherTile(_myUnitController.GetGridPosition().x + 1, _myUnitController.GetGridPosition().y - 1);
        if (neighbourTile != null && neighbourTile._isOccupied && neighbourTile._myUnit.GetPlayerId() != _myUnitController.GetPlayerId())
        {
            myEffect = neighbourTile._myUnit.gameObject.AddComponent<EffectProvoked>();
            myEffect.InitializeEffect(_myUnitController);
        }
        neighbourTile = _myUnitController._myTile.GetAnotherTile(_myUnitController.GetGridPosition().x + 1, _myUnitController.GetGridPosition().y);
        if (neighbourTile != null && neighbourTile._isOccupied && neighbourTile._myUnit.GetPlayerId() != _myUnitController.GetPlayerId())
        {
            myEffect = neighbourTile._myUnit.gameObject.AddComponent<EffectProvoked>();
            myEffect.InitializeEffect(_myUnitController);
        }
        neighbourTile = _myUnitController._myTile.GetAnotherTile(_myUnitController.GetGridPosition().x + 1, _myUnitController.GetGridPosition().y + 1);
        if (neighbourTile != null && neighbourTile._isOccupied && neighbourTile._myUnit.GetPlayerId() != _myUnitController.GetPlayerId())
            {
            myEffect = neighbourTile._myUnit.gameObject.AddComponent<EffectProvoked>();
            myEffect.InitializeEffect(_myUnitController);
        }
    }
}
