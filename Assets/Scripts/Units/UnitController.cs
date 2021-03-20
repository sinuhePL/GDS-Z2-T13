using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class UnitController : MonoBehaviour, IClickable, IEndturnable
{
    [SerializeField] private ScriptableUnit _unit;
    [SerializeField] private HealthController _myHealth;
    [SerializeField] private SpriteRenderer _myReticle;
    public TileController _myTile { get; set; }
    public bool _isAvailable { get; set; }
    public bool _isKilled { get; set; }
    public bool _isDeployed { get; set; }
    public bool _hasMoved { get; set; }
    private SpriteRenderer _mySpriteRenderer;
    private int _freeAttacksCount;
    private bool _showingPotentialDamage;

    // Start is called before the first frame update
    void Start()
    {
        _mySpriteRenderer = GetComponent<SpriteRenderer>();
        _mySpriteRenderer.sprite = _unit.unitDesignerSprite;
    }

    private void OnMouseEnter()
    {
        if(_isDeployed) EventManager._instance.UnitHovered(this);
    }

    private void OnMouseExit()
    {
        if (_isDeployed) EventManager._instance.UnitUnhovered(this);
    }

    private IEnumerator MakeMove(List<TileController> movePath)
    {
        ITileBehaviour myTileBehaviour;
        IEnterTile[] enterTileReactors;
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
        if(myTileBehaviour != null) myTileBehaviour.EnterTileAction(this);
        enterTileReactors = GetComponents<IEnterTile>();
        foreach(IEnterTile reactor in enterTileReactors)
        {
            reactor.EnterTileAction(_myTile);
        }
        EventManager._instance.ExecutionEnded(this);
    }

    private IEnumerator MakeAttack(UnitController target)
    {
        target.DamageUnit(CalculateAttack(target));
        yield return 0;
        if (_freeAttacksCount < 1) _freeAttacksCount = _unit.attacksCount;
        EventManager._instance.ExecutionEnded(this);
    }

    private int GetBonusArmor()
    {
        int result = 0;
        IArmorModifier[] myArmorModifiers;
        myArmorModifiers = GetComponents<IArmorModifier>();
        foreach(IArmorModifier modifier in myArmorModifiers)
        {
            result += modifier.GetArmorModifier();
        }
        return result;
    }

    private int CalculateDamage(int damage)
    {
        int damageTaken;
        int damageModifier = 0;
        IDamageModifier[] myDamageModifiers;
        myDamageModifiers = GetComponents<IDamageModifier>();
        foreach (IDamageModifier modifier in myDamageModifiers)
        {
            damageModifier += modifier.GetDamageModifier();
        }
        damageTaken = damage - _unit.armor - GetBonusArmor() + damageModifier;
        Debug.Log(_unit.unitName + " received " + damage + " damage plus " + damageModifier + " modifiers minus " + _unit.armor + " armor.");
        if (damageTaken < 0) damageTaken = 0;
        return damageTaken;
    }

    private int CalculateAttack(UnitController target)
    {
        IAttackModifier[] myAttackModifiers;
        int modifiersDamageBonus = 0;
        myAttackModifiers = GetComponents<IAttackModifier>();
        foreach (IAttackModifier modifier in myAttackModifiers)
        {
            modifiersDamageBonus += modifier.GetAttackModifier(target);
        }
        return _unit.attackDamage + modifiersDamageBonus;
    }

    public void EndTurnAction(int playerId)
    {
        if (_unit.playerId != playerId)
        {
            _isAvailable = true;
            _hasMoved = false;
        }
    }

    public void DeployUnit(TileController initialTile)
    {
        _myTile = initialTile;
        _myTile._myUnit = this;
        _myTile._isOccupied = true;
        transform.position = initialTile.transform.position;
        ITileBehaviour myTileBehaviour = initialTile.gameObject.GetComponent<ITileBehaviour>();
        if (myTileBehaviour != null) myTileBehaviour.EnterTileAction(this);
    }

    public void InitializeUnit()
    {
        _myHealth.InitializeHealth(_unit.unitHealth);
        _myReticle.enabled = false;
        _isAvailable = true;
        _hasMoved = false;
        _freeAttacksCount = _unit.attacksCount;
        _isKilled = false;
        if (_unit.isKing) _isDeployed = true;
        else _isDeployed = false;
        _showingPotentialDamage = false;
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
        int moveRangeModifier = 0;
        IMoveRangeModifier[] myMoveRangeModifiers;
        myMoveRangeModifiers = GetComponents<IMoveRangeModifier>();
        foreach(IMoveRangeModifier modifier in myMoveRangeModifiers)
        {
            moveRangeModifier += modifier.GetMoveRangeModifier();
        }
        if (_unit.moveRange + moveRangeModifier < 1) return 0;
        else return _unit.moveRange + moveRangeModifier;
    }

    public float GetMoveSpeed()
    {
        return _unit.moveSpeed;
    }

    public void MoveUnit(List<TileController> movePath)
    {
        _hasMoved = true;
        StartCoroutine(MakeMove(movePath));
    }

    public void AttackUnit(UnitController targetUnit)
    {
        _freeAttacksCount--;
        StartCoroutine(MakeAttack(targetUnit));
    }

    public int GetPlayerId()
    {
        return _unit.playerId;
    }

    public int GetAttackRange()
    {
        int rangeModifier = 0;
        IAttackRangeModifier[] myAttackRangeModifiers;
        myAttackRangeModifiers = GetComponents<IAttackRangeModifier>();
        foreach(IAttackRangeModifier modifier in myAttackRangeModifiers)
        {
            rangeModifier += modifier.GetAttackRangeModifier();
        }
        return _unit.attackRange + rangeModifier;
    }

    public void DamageUnit(int damage)
    {
        int damageTaken;

        damageTaken = CalculateDamage(damage);
        _isKilled = _myHealth.ChangeHealth(-damageTaken);
        if (_isKilled)
        {
            _myTile._isOccupied = false;
            _myTile._myUnit = null;
            _myTile = null;
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
        return _unit.armor + GetBonusArmor();
    }

    public void ChangeHP(int change)
    {
        bool isKilled;
        isKilled = _myHealth.ChangeHPNumber(change);
        if (isKilled) EventManager._instance.UnitKilled(this);
    }

    public bool IsTargetValid(UnitController attackTarget)
    {
        IValidateTarget[] myValidateTargetModifiers;
        myValidateTargetModifiers = GetComponents<IValidateTarget>();
        foreach(IValidateTarget modifier in myValidateTargetModifiers)
        {
            if (!modifier.IsTargetValid(attackTarget)) return false;
        }
        if (attackTarget.GetPlayerId() != _unit.playerId) return true;
        else return false;
    }

    public void ShowPotentialDamage(int damage)
    {
        int damageTaken;

        if (!_showingPotentialDamage)
        {
            _showingPotentialDamage = true;
            damageTaken = CalculateDamage(damage);
            StartCoroutine(_myHealth.ShowPotentialDamage(damageTaken));
        }
    }

    public void StopShowingPotentialDamage()
    {
        _myHealth.StopShowingPotentialDamage();
        _showingPotentialDamage = false;
    }

    public int GetFreeAttackNumber()
    {
        return _freeAttacksCount;
    }

    public string GetUnitName()
    {
        return _unit.unitName;
    }

    public int GetHP()
    {
        return _myHealth.GetCurrentHealth();
    }

    public int GetMaxHP()
    {
        return _unit.unitHealth;
    }

    public int GetBaseMoveRange()
    {
        return _unit.moveRange;
    }

    public int GetAttackStrength()
    {
        return _unit.attackDamage;
    }

    public int GetCalculatedAttack(UnitController target)
    {
        return CalculateAttack(target);
    }

    public int GetBaseAttackRange()
    {
        return _unit.attackRange;
    }

    public int GetBaseArmor()
    {
        return _unit.armor;
    }

    public int GetBaseAttacksCount()
    {
        return _unit.attacksCount;
    }

    public Sprite GetUnitImage()
    {
        return _unit.unitDesignerSprite;
    }

    public int GetUnitType()
    {
        return _unit.unitTypeId;
    }

    public void ChangePosition(Vector3 newPosition)
    {
        transform.position = newPosition;
    }
}
