using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    [SerializeField] GameObject _healthPointPrefab;
    [SerializeField] Sprite _healthDesignerSprite;
    [SerializeField] Sprite _lostHealthDesignerSprite;

    private List<SpriteRenderer> _designerHealthPointsList;
    private List<SpriteRenderer> _healthPointsList;
    private int _healthPoints, _initialHealthPoints;
    private bool _showPotentialDamage;
    private bool _isDesignerMode;

    private void CreateHealthBar()
    {
        GameObject newPoint;
        for (int i = 0; i < _initialHealthPoints; i++)
        {
            newPoint = Instantiate(_healthPointPrefab, Vector3.zero, Quaternion.identity);
            newPoint.transform.SetParent(this.transform);
            if (i < 5) newPoint.transform.localPosition = new Vector3(-0.3f + 0.2f * i, 0.8f, 0.0f);
            else if (i < 10) newPoint.transform.localPosition = new Vector3(-0.3f + 0.2f * (i - 5), 0.6f, 0.0f);
            else newPoint.transform.localPosition = new Vector3(-0.3f + 0.2f * (i - 10), 0.4f, 0.0f);
            _designerHealthPointsList.Add(newPoint.GetComponent<SpriteRenderer>());
        }
    }

    private void UpdateHealthBar()
    {
        int i = 0;
        foreach (SpriteRenderer healthPointRenderer in _designerHealthPointsList)
        {
            if (i < _healthPoints) healthPointRenderer.sprite = _healthDesignerSprite;
            else healthPointRenderer.sprite = _lostHealthDesignerSprite;
            i++;
        }
    }

    public void InitializeHealth(int health)
    {
        _healthPoints = health;
        _initialHealthPoints = health;
        _designerHealthPointsList = new List<SpriteRenderer>();
        _healthPointsList = new List<SpriteRenderer>();
        _showPotentialDamage = false;
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
        for(int i=_designerHealthPointsList.Count-1; i>=0; i--)
        {
            Destroy(_designerHealthPointsList[i]);
        }
        _designerHealthPointsList.Clear();
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

    public void StopShowingPotentialDamage()
    {
        _showPotentialDamage = false;
    }

    public IEnumerator ShowPotentialDamage(int damage)
    {
        int currentHealth, minHealth;
        if (_showPotentialDamage) yield break;
        else
        {
            _showPotentialDamage = true;
            currentHealth = _healthPoints;
            minHealth = _healthPoints - damage;
            if (minHealth < 0) minHealth = 0;
            while (_showPotentialDamage)
            {
                for (int i = _healthPoints - 1; i >= minHealth; i--)
                {
                    _designerHealthPointsList[i].sprite = _lostHealthDesignerSprite;
                }
                yield return new WaitForSeconds(0.25f);
                for (int i = _healthPoints - 1; i >= minHealth; i--) _designerHealthPointsList[i].sprite = _healthDesignerSprite;
                yield return new WaitForSeconds(0.5f);
            }
        }
    }

    public void SetMode(string newMode)
    {
        if(newMode == "designer")
        {
            _isDesignerMode = true;
            foreach(SpriteRenderer healthPointRenderer in _designerHealthPointsList)
            {
                healthPointRenderer.gameObject.SetActive(true);
            }
            foreach (SpriteRenderer healthPointRenderer in _healthPointsList)
            {
                healthPointRenderer.gameObject.SetActive(false);
            }
        }
        else
        {
            _isDesignerMode = false;
            foreach (SpriteRenderer healthPointRenderer in _designerHealthPointsList)
            {
                healthPointRenderer.gameObject.SetActive(false);
            }
            foreach (SpriteRenderer healthPointRenderer in _healthPointsList)
            {
                healthPointRenderer.gameObject.SetActive(true);
            }
        }
    }
}
