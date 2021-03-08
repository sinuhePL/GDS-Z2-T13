using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TileController))]
public class TileIncreasingDamage : MonoBehaviour, ITileBehaviour
{
    [SerializeField] private int _attackBonus;
    private TileController _myTileController;

    private void Start()
    {
        _myTileController = GetComponent<TileController>();
    }

    public void EnterTileAction(UnitController myUnit)
    {
        EffectIncreaseDamage myEffect;
        myEffect = myUnit.gameObject.AddComponent<EffectIncreaseDamage>();
        myEffect.InitializeEffect(_attackBonus);
    }
}
