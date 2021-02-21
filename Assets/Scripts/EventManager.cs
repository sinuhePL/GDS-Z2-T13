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

    public event Action<UnitController> OnUnitClicked;

    public void UnitClicked(UnitController clickedUnit)
    {
        OnUnitClicked?.Invoke(clickedUnit);
    }

    public event Action<TileController> OnTileClicked;

    public void TileClicked(TileController clickedTile)
    {
        OnTileClicked?.Invoke(clickedTile);
    }

    public event Action<TileController> OnTileHovered;

    public void TileHovered(TileController hoveredTile)
    {
        OnTileHovered?.Invoke(hoveredTile);
    }

    public event Action OnExecutionEnd;

    public void ExecutionEnded()
    {
        OnExecutionEnd?.Invoke();
    }
}
