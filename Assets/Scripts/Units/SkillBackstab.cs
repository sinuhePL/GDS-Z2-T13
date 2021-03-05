using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UnitController))]
public class SkillBackstab : MonoBehaviour, IUnitSkill
{
    [SerializeField] private int _attackBonus;
    private UnitController _myUnitController;

    private void Awake()
    {
        _myUnitController = GetComponent<UnitController>();
    }

    public void EnterTileAction(TileController newTile)
    { }

    public void EndTurnAction(int playerId)
    { }

    public int AttackAction(UnitController target)
    {
        GridPosition targetPosition;
        targetPosition = target.GetGridPosition();
        if (_myUnitController.GetPlayerId() == 1 && _myUnitController.GetGridPosition().x == target.GetGridPosition().x + 1
            || _myUnitController.GetPlayerId() == 2 && _myUnitController.GetGridPosition().x == target.GetGridPosition().x - 1) return _attackBonus;
        else return 0;

    }
}
