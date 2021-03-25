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
    private bool _turnEnded;

    private void Start()
    {
        _winnerText.enabled = false;
        _deployMinionButton.gameObject.SetActive(false);
        _turnEnded = false;
    }

    private IEnumerator TurnTimer(int timeLimit, GameController myGameController)
    {
        int myTimer = 0;

        _turnEnded = false;
        while (!_turnEnded && myTimer < timeLimit)
        {
            _timerText.text = (timeLimit - myTimer).ToString();
            myTimer += 1;
            yield return new WaitForSeconds(1.0f);
        }
        myGameController.EndTurnAction();
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

    public void StartPlayerTurn(int playerId, GameController myGameController, int timeLimit)
    {
        _turnEnded = true;
        _myUnitsPanel.SetNewPlayer(playerId);
        StartCoroutine(TurnTimer(timeLimit, myGameController));
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
