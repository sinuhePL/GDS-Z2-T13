using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UnitController))]
public class SkillRecovery : MonoBehaviour, IUnitSkill
{
    [SerializeField] private int _recoveryRate;
    private UnitController _myUnitController;

    private void Start()
    {
        _myUnitController = GetComponent<UnitController>();
    }

    public void EndTurnAction(int playerId)
    {
        if(_myUnitController.GetPlayerId() == playerId) _myUnitController.HealUnit(_recoveryRate);
    }

    public int AttackAction(UnitController target)
    {
        return 0;
    }
}
