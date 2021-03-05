using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UnitController))]
public class SkillBurn : MonoBehaviour, IUnitSkill
{
    [SerializeField] private int _burnAmount;
    [SerializeField] private int _burnTurns;
    private UnitController _myUnitController;

    private void Awake()
    {
        _myUnitController = GetComponent<UnitController>();
    }

    public void EnterTileAction(TileController newTile)
    { }

    public int AttackAction(UnitController target)
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
