using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEffect
{
    int AttackModifier();
    int ArmorModifier();
    int MoveRangeModifier();
    int AttackRangeModifier();
    int DamageModifier();
}
