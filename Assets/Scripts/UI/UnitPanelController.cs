using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitPanelController : MonoBehaviour
{
    [SerializeField] private Image _myUnitImage;
    [SerializeField] private Text _myUnitText;
    [SerializeField] private Button _myLeftButton;
    [SerializeField] private Button _myRightButton;

    private GameObject _myUnitPrefab;

    public void SetUnit(GameObject myUnit)
    {
        UnitController myUnitController;
        myUnitController = myUnit.GetComponent<UnitController>();
        _myUnitPrefab = myUnit;
        _myUnitImage.sprite = myUnitController.GetUnitImage();
        _myUnitText.text = myUnitController.GetUnitName();
    }

    public void DisableButtons()
    {
        _myLeftButton.gameObject.SetActive(false);
        _myRightButton.gameObject.SetActive(false);
    }

    public GameObject GetUnitPrefab()
    {
        return _myUnitPrefab;
    }

    public void DisableMe()
    {
        gameObject.SetActive(false);
    }

    public void EnableMe()
    {
        gameObject.SetActive(true);
    }
}
