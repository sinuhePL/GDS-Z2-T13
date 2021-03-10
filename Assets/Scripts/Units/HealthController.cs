using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    [SerializeField] GameObject _healthPointPrefab;
    [SerializeField] Sprite _healthSprite;
    [SerializeField] Sprite _lostHealthSprite;

    private List<SpriteRenderer> _healthPointsList;
    private int _healthPoints, _initialHealthPoints;

    private void CreateHealthBar()
    {
        GameObject newPoint;
        for (int i = 0; i < _initialHealthPoints; i++)
        {
            newPoint = Instantiate(_healthPointPrefab, Vector3.zero, Quaternion.identity);
            newPoint.transform.SetParent(this.transform);
            if(i<5) newPoint.transform.localPosition = new Vector3(-0.3f + 0.2f * i, 0.6f, 0.0f);
            else newPoint.transform.localPosition = new Vector3(-0.3f + 0.2f * (i-5), 0.4f, 0.0f);
            _healthPointsList.Add(newPoint.GetComponent<SpriteRenderer>());
        }
    }

    private void UpdateHealthBar()
    {
        int i = 0;
        foreach (SpriteRenderer healthPointRenderer in _healthPointsList)
        {
            if (i < _healthPoints) healthPointRenderer.sprite = _healthSprite;
            else healthPointRenderer.sprite = _lostHealthSprite;
            i++;
        }
    }

    public void InitializeHealth(int health)
    {
        _healthPoints = health;
        _initialHealthPoints = health;
        _healthPointsList = new List<SpriteRenderer>();
        CreateHealthBar();   
    }

    // returns true if unit dead
    public bool ChangeHealth(int healthChange)
    {
        _healthPoints += healthChange;
        if (_healthPoints < 0) _healthPoints = 0;
        if (_healthPoints > _initialHealthPoints) _healthPoints = _initialHealthPoints;
        UpdateHealthBar();
        if (_healthPoints == 0) return true;
        else return false;   
    }

    // returns true if unit dead
    public bool ChangeHPNumber(int change)
    {
        for(int i=_healthPointsList.Count-1; i>=0; i--)
        {
            Destroy(_healthPointsList[i]);
        }
        _healthPointsList.Clear();
        _initialHealthPoints += change;
        if (_initialHealthPoints <= 0) return true;
        _healthPoints += change;
        CreateHealthBar();
        UpdateHealthBar();
        if (_healthPoints <= 0) return true;
        else return false;
    }

    public int GetCurrentHealth()
    {
        return _healthPoints;
    }
}
