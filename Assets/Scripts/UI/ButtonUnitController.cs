using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ButtonUnitController : MonoBehaviour
{
    private UnitController _myUnit;
    private Image _myImage;

    // Start is called before the first frame update
    void Awake()
    {
        _myImage = GetComponent<Image>();
        _myUnit = null;
    }

    public void ClickedMe()
    {
        EventManager._instance.UnitClicked(_myUnit);
    }

    public void SetUnit(UnitController unit)
    {
        _myUnit = unit;
        _myImage.sprite = unit.GetUnitImage();
    }

    public void EnlargeUnit(UnitController unit)
    {
        if(unit == _myUnit)
        {
            transform.DOScale(1.4f, 0.3f).SetEase(Ease.InOutBack);
        }
        else
        {
            transform.DOScale(1.0f, 0.3f).SetEase(Ease.InOutBack);
        }
    }
}
