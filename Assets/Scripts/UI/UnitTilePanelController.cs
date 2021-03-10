using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitTilePanelController : MonoBehaviour
{
    [SerializeField] Text _name;
    [SerializeField] Text _description;
    [SerializeField] Text _hpText;
    [SerializeField] Text _hp;
    [SerializeField] Text _moveRangeText;
    [SerializeField] Text _moveRange;
    [SerializeField] Text _attackStrengthText;
    [SerializeField] Text _attackStrength;
    [SerializeField] Text _attackRangeText;
    [SerializeField] Text _attackRange;
    [SerializeField] Text _armorText;
    [SerializeField] Text _armor;
    [SerializeField] Text _attacksCountText;
    [SerializeField] Text _attackCount;
    [SerializeField] Text _skillsText;
    [SerializeField] Text _skills;
    [SerializeField] Text _effectsText;
    [SerializeField] Text _effects;

    // Start is called before the first frame update
    void Start()
    {
        _hpText.enabled = false;
        _moveRangeText.enabled = false;
        _attackStrengthText.enabled = false;
        _attackRangeText.enabled = false;
        _armorText.enabled = false;
        _attacksCountText.enabled = false;
        _skillsText.enabled = false;
        _effectsText.enabled = false;
    }

    public void DisplayTile(TileController myTile)
    {
        _name.text = myTile.GetTileName();
        _description.text = myTile.GetDescription();
        _hpText.enabled = false;
        _moveRangeText.enabled = false;
        _attackStrengthText.enabled = false;
        _attackRangeText.enabled = false;
        _armorText.enabled = false;
        _attacksCountText.enabled = false;
        _skillsText.enabled = false;
        _effectsText.enabled = false;
        _hp.text = "";
        _moveRange.text = "";
        _attackStrength.text = "";
        _attackRange.text = "";
        _armor.text = "";
        _attackCount.text = "";
        _skills.text = "";
        _effects.text = "";
    }

    public void DisplayUnit(UnitController myUnit)
    {
        IEffect[] unitEffects;
        ISkill[] unitSkills;
        string description;
        int counter;

        _hpText.enabled = true;
        _moveRangeText.enabled = true;
        _attackStrengthText.enabled = true;
        _attackRangeText.enabled = true;
        _armorText.enabled = true;
        _attacksCountText.enabled = true;
        _skillsText.enabled = true;
        _effectsText.enabled = true;
        _name.text = myUnit.GetUnitName();
        if (myUnit.IsKing()) _description.text = "Commander";
        else _description.text = "Minion";
        _hp.text = myUnit.GetHP().ToString() + "/" + myUnit.GetMaxHP().ToString();
        _moveRange.text = myUnit.GetBaseMoveRange().ToString();
        _attackStrength.text = myUnit.GetAttackStrength().ToString();
        _attackRange.text = myUnit.GetBaseAttackRange().ToString();
        _armor.text = myUnit.GetBaseArmor().ToString();
        _attackCount.text = myUnit.GetBaseAttacksCount().ToString();
        unitSkills = myUnit.gameObject.GetComponents<ISkill>();
        description = "";
        counter = 0;
        foreach(ISkill skill in unitSkills)
        {
            if (counter > 0) description += "\n";
            description += skill.GetDescription();
            counter++;
        }
        _skills.text = description;
        unitEffects = myUnit.gameObject.GetComponents<IEffect>();
        description = "";
        counter = 0;
        foreach (IEffect effect in unitEffects)
        {
            if(counter > 0) description += "\n";
            description += effect.GetDescription();
            counter++;
        }
        _effects.text = description;
    }

    public void ClearDisplay()
    {
        _hpText.enabled = false;
        _moveRangeText.enabled = false;
        _attackStrengthText.enabled = false;
        _attackRangeText.enabled = false;
        _armorText.enabled = false;
        _attacksCountText.enabled = false;
        _skillsText.enabled = false;
        _effectsText.enabled = false;
        _name.text = "";
        _description.text = "";
        _hp.text = "";
        _moveRange.text = "";
        _attackStrength.text = "";
        _attackRange.text = "";
        _armor.text = "";
        _attackCount.text = "";
        _skills.text = "";
        _effects.text = "";
    }
}
