using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UnitController))]
public class SkillBurn : MonoBehaviour, IAttackModifier
{
    [SerializeField] private int _burnAmount;
    [SerializeField] private int _burnTurns;
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
            myBurnEffect.InitializeEffect(_burnAmount, _burnTurns);
        }
        return 0;
    }
}
