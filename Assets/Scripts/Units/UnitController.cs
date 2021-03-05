using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class UnitController : MonoBehaviour, IClickable
{
    [SerializeField] private ScriptableUnit _unit;
    [SerializeField] private HealthController _myHealth;
    [SerializeField] private SpriteRenderer _myReticle;
    public TileController _myTile { get; set; }
    public bool _isAvailable { get; set; }
    public int _myBonusArmor { get; set; }
    public int _myBonusAttackDamage { get; set; }
    public int _myBonusAttackRange { get; set; }
    public int _myBonusMoveRange { get; set; }
    private SpriteRenderer _mySpriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        _mySpriteRenderer = GetComponent<SpriteRenderer>();
        _mySpriteRenderer.sprite = _unit.unitDesignerSprite;
        //gameObject.AddComponent<BoxCollider2D>();
    }

    private void OnDestroy()
    {
        _myTile._isOccupied = false;
    }

    private IEnumerator MakeMove(List<TileController> movePath)
    {
        ITileBehaviour myTileBehaviour;
        IUnitSkill[] mySkills;
        TileController currentNode;
        float step;
        _myTile._myUnit = null;
        _myTile._isOccupied = false;
        while (movePath.Count > 0)
        {
            currentNode = movePath[0];
            while (Vector3.Distance(currentNode.transform.position, transform.position) > 0.001f)
            {
                step = _unit.moveSpeed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, currentNode.transform.position, step);
                yield return 0;
            }
            _myTile = currentNode;
            movePath.Remove(currentNode);
        }
        _myTile._myUnit = this;
        _myTile._isOccupied = true;
        myTileBehaviour = _myTile.gameObject.GetComponent<ITileBehaviour>();
        myTileBehaviour.EnterTileAction(this);
        mySkills = GetComponents<IUnitSkill>();
        foreach(IUnitSkill skill in mySkills)
        {
            skill.EnterTileAction(_myTile);
        }
        EventManager._instance.ExecutionEnded(this);
    }

    private IEnumerator MakeAttack(UnitController target)
    {
        IUnitSkill[] mySkills;
        int skillsDamageBonus = 0;
        mySkills = GetComponents<IUnitSkill>();
        foreach(IUnitSkill skill in mySkills)
        {
            skillsDamageBonus += skill.AttackAction(target);
        }
        target.DamageUnit(_unit.attackDamage + _myBonusAttackDamage + skillsDamageBonus);
        yield return 0;
        EventManager._instance.ExecutionEnded(this);
    }

    public void InitializeUnit(TileController initialTile)
    {
        _myTile = initialTile;
        _myTile._myUnit = this;
        _myTile._isOccupied = true;
        transform.position = initialTile.transform.position;
        _myHealth.InitializeHealth(_unit.unitHealth);
        _myReticle.enabled = false;
        _isAvailable = true;
        _myBonusArmor = 0;
        _myBonusAttackDamage = 0;
        _myBonusAttackRange = 0;
        ITileBehaviour myTileBehaviour = initialTile.gameObject.GetComponent<ITileBehaviour>();
        myTileBehaviour.EnterTileAction(this);
    }

    public void Click()
    {
        Debug.Log("Kliknięta jednostka: " + _unit.name);
        EventManager._instance.UnitClicked(this);
    }

    public GridPosition GetGridPosition()
    {
        return _myTile.GetGridPosition();
    }

    public int GetMoveRange()
    {
        if (_unit.moveRange + _myBonusMoveRange < 1) return 1;
        else return _unit.moveRange + _myBonusMoveRange;
    }

    public float GetMoveSpeed()
    {
        return _unit.moveSpeed;
    }

    public void MoveUnit(List<TileController> movePath)
    {
        StartCoroutine(MakeMove(movePath));
    }

    public void AttackUnit(UnitController targetUnit)
    {
        StartCoroutine(MakeAttack(targetUnit));
    }

    public int GetPlayerId()
    {
        return _unit.playerId;
    }

    public int GetAttackRange()
    {
        return _unit.attackRange + _myBonusAttackRange;
    }

    public void DamageUnit(int damage)
    {
        bool isKilled;
        int effectsModifier = 0;
        IEffect[] myEffects;
        myEffects = GetComponents<IEffect>();
        foreach(IEffect effect in myEffects)
        {
            effectsModifier += effect.DamageModifier();
        }
        int damageTaken = damage - _unit.armor - _myBonusArmor + effectsModifier;
        Debug.Log(_unit.unitName + " received " + damage + " damage plus " + effectsModifier + " modifiers minus " + _unit.armor + " armor.");
        if (damageTaken < 0) damageTaken = 0;
        isKilled = _myHealth.ChangeHealth(-damageTaken);
        if (isKilled)
        {
            gameObject.SetActive(false);
            EventManager._instance.UnitKilled(this);
        }
    }

    public void HealUnit(int healPoints)
    {
        _myHealth.ChangeHealth(healPoints);
    }

    public void SetReticle(bool visible)
    {
        _myReticle.enabled = visible;
    }

    public bool IsKing()
    {
        return _unit.isKing;
    }

    public int GetArmor()
    {
        return _unit.armor + _myBonusArmor;
    }

    public void ChangeHP(int change)
    {
        bool isKilled;
        isKilled = _myHealth.ChangeHPNumber(change);
        if (isKilled) EventManager._instance.UnitKilled(this);
    }
}
