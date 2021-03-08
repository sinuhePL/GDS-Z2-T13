using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UnitController))]
public class SkillPoison : MonoBehaviour, IAttackModifier
{
    [SerializeField] private int _weaknessAmount;
    [SerializeField] private int _weaknessTurns;
    private UnitController _myUnitController;

    private void Awake()
    {
        _myUnitController = GetComponent<UnitController>();
    }

    public int GetAttackModifier(UnitController target)
    {
        EffectPoison myPoisonEffect;
        if (target.gameObject.GetComponent<EffectPoison>() == null)
        {
            myPoisonEffect = target.gameObject.AddComponent<EffectPoison>();
            myPoisonEffect.InitializeEffect(_weaknessAmount, _weaknessTurns);
            return -1;
        }
        else return 0;
    }
}
