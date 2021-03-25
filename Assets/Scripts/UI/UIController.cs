using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private Text _winnerText;
    [SerializeField] private UnitTilePanelController _myInfoPanel;
    [SerializeField] private PlayerUnitsController _myUnitsPanel;
    [SerializeField] private Button _deployMinionButton;
    [SerializeField] private Text _timerText;
    private int _myTimer;


    private void Start()
    {
        _winnerText.enabled = false;
        _deployMinionButton.gameObject.SetActive(false);
        _myTimer = 0;
    }

    private IEnumerator TurnTimer(int timeLimit, GameController myGameController)
    {
        while (true)
        {
            if (_myTimer >= timeLimit)
            {
                myGameController.EndTurnAction();
                _myTimer = 0;
            }
            _timerText.text = (timeLimit - _myTimer).ToString();
            _myTimer += 1;
            yield return new WaitForSeconds(1.0f);
        }
    }

    public void DisplayWinner(int winnerId)
    {
        _winnerText.enabled = true;
        if (winnerId == 1) _winnerText.text = "Winner: first player";
        else _winnerText.text = "Winner: second player";
    }

    public void InitializeUnitsPanel(List<UnitController> units, int startingPlayer, GameController myGameController, int timeLimit)
    {
        _myUnitsPanel.InitializePanel(units, startingPlayer);
        StartCoroutine(TurnTimer(timeLimit, myGameController));
    }

    public void DisplayTile(TileController tile)
    {
        _myInfoPanel.DisplayTile(tile);
    }

    public void DisplayUnit(UnitController unit)
    {
        _myInfoPanel.DisplayUnit(unit);
    }

    public void ClearDisplay()
    {
        _myInfoPanel.ClearDisplay();
    }

    public void StartPlayerTurn(int playerId)
    {
        _myTimer = 0;
        _myUnitsPanel.SetNewPlayer(playerId);
    }

    public void SelectUnit(UnitController unit)
    {
        if(unit.IsKing() && !_myUnitsPanel.AllUnitsDeployed()) _deployMinionButton.gameObject.SetActive(true);
        else _deployMinionButton.gameObject.SetActive(false);
        _myUnitsPanel.UnitSelected(unit);
    }

    public void KillUnit(UnitController unit)
    {
        _myUnitsPanel.UnitKilled(unit);
    }

    public void ShowDeployableUnits()
    {
        _myUnitsPanel.ShowDeployableMinions();
    }

    public void EndDeployment()
    {
        _myUnitsPanel.EndDeployment();
    }

    public void MarkUnitUnavailable(UnitController unit)
    {
        _myUnitsPanel.MarkUnitUnavailable(unit);
    }

    public bool AllUnitsDeployed()
    {
        return _myUnitsPanel.AllUnitsDeployed();
    }
}
