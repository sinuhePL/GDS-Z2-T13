using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Unit", menuName = "Designers/Unit")]
public class ScriptableUnit : ScriptableObject
{
    public Sprite unitSprite;
    public Sprite unitDesignerSprite;
    public string unitName;
    public int unitHealth;
    public int moveRange;
    public int playerId;
}
