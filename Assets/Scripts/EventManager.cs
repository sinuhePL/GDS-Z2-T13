using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EventManager : MonoBehaviour
{
    public static EventManager _instance;

    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        else
        {
            Destroy(this);
        }
    }

    public event Action<GridPosition, int> OnUnitClicked;

    public void UnitClicked(GridPosition unitPosition, int moveRange)
    {
        OnUnitClicked?.Invoke(unitPosition, moveRange);
    }

    public event Action OnTileClicked;

    public void TileClicked()
    {
        OnTileClicked?.Invoke();
    }
}
