using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UnitController))]
public class SkillPoison : MonoBehaviour, IAttackModifier, ISkill
{
    [SerializeField] private int _weaknessAmount;
    [SerializeField] private int _weaknessTurns;
    [SerializeField] private string _description;
    [SerializeField] private string _effectDescription;
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
            myPoisonEffect.InitializeEffect(_weaknessAmount, _weaknessTurns, _effectDescription);
            return -1;
        }
        else return 0;
    }

    public string GetDescription()
    {
        return _description;
    }
}
