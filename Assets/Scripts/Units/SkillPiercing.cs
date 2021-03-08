using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UnitController))]
public class SkillPiercing : MonoBehaviour, IAttackModifier
{
    private UnitController _myUnitController;

    private void Awake()
    {
        _myUnitController = GetComponent<UnitController>();
    }

    public int GetAttackModifier(UnitController target)
    {
        return target.GetArmor();
    }
}
