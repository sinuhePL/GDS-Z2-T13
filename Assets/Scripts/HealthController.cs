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

    public void InitializeHealth(int health)
    {
        GameObject newPoint;
        _healthPoints = health;
        _initialHealthPoints = health;
        _healthPointsList = new List<SpriteRenderer>();
        for (int i=0; i<health;i++)
        {
            newPoint = Instantiate(_healthPointPrefab, Vector3.zero, Quaternion.identity);
            newPoint.transform.SetParent(this.transform);
            newPoint.transform.localPosition = new Vector3(-0.3f + 0.2f * i, 0.5f, 0.0f);
            _healthPointsList.Add(newPoint.GetComponent<SpriteRenderer>());
        }
    }

    // returns true if unit dead
    public bool ChangeHealth(int healthChange)
    {
        int i;
        _healthPoints += healthChange;
        if (_healthPoints < 0) _healthPoints = 0;
        if (_healthPoints > _initialHealthPoints) _healthPoints = _initialHealthPoints;
        i = 0;
        foreach(SpriteRenderer healthPointRenderer in _healthPointsList)
        {
            if (i < _healthPoints) healthPointRenderer.sprite = _healthSprite;
            else healthPointRenderer.sprite = _lostHealthSprite;
            i++;
        }
        if (_healthPoints == 0) return true;
        else return false;
        
    }
}
