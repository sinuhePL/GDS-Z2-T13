using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UnitController))]
public class SkillBurn : MonoBehaviour, IAttackModifier, ISkill
{
    [SerializeField] private int _burnAmount;
    [SerializeField] private int _burnTurns;
    [SerializeField] private string _description;
    [SerializeField] private string _effectDescription;
    private UnitController _myUnitController;

    private void Awake()
    {
        _myUnitController = GetComponent<UnitController>();
    }

    public int GetAttackModifier(UnitController target)
    {
        EffectBurn myBurnEffect;
        if(target.gameObject.GetComponent<EffectBurn>() == null)
        {
            myBurnEffect = target.gameObject.AddComponent<EffectBurn>();
            myBurnEffect.InitializeEffect(_burnAmount, _burnTurns, _effectDescription);
        }
        return 0;
    }

    public string GetDescription()
    {
        return _description;
    }
}
