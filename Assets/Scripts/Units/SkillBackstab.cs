using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UnitController))]
public class SkillBackstab : MonoBehaviour, IAttackModifier
{
    [SerializeField] private int _attackBonus;
    private UnitController _myUnitController;

    private void Awake()
    {
        _myUnitController = GetComponent<UnitController>();
    }

    public int GetAttackModifier(UnitController target)
    {
        GridPosition targetPosition;
        targetPosition = target.GetGridPosition();
        if (_myUnitController.GetPlayerId() == 1 && _myUnitController.GetGridPosition().x == target.GetGridPosition().x + 1
            || _myUnitController.GetPlayerId() == 2 && _myUnitController.GetGridPosition().x == target.GetGridPosition().x - 1) return _attackBonus;
        else return 0;

    }
}
