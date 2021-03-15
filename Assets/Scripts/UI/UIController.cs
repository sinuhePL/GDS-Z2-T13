using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private Text _winnerText;
    [SerializeField] private UnitTilePanelController _myInfoPanel;
    [SerializeField] private PlayerUnitsController _myUnitsPanel;

    private void Start()
    {
        _winnerText.enabled = false;
    }

    public void DisplayWinner(int winnerId)
    {
        _winnerText.enabled = true;
        if (winnerId == 1) _winnerText.text = "Winner: first player";
        else _winnerText.text = "Winner: second player";
    }

    public void InitializeUnitsPanel(List<UnitController> units, int startingPlayer)
    {
        _myUnitsPanel.InitializePanel(units, startingPlayer);
    }

    public UnitTilePanelController GetInfoPanel()
    {
        return _myInfoPanel;
    }

    public void ChangePlayer(int playerId)
    {
        _myUnitsPanel.SetNewPlayer(playerId);
    }

    public void SelectUnit(UnitController unit)
    {
        _myUnitsPanel.UnitSelected(unit);
    }
}
