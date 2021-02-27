using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private Text _winnerText;

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
}
