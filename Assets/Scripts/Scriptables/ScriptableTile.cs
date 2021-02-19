﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Tile", menuName = "Designers/Tile")]
public class ScriptableTile : ScriptableObject
{
    public string letter;
    public Sprite tileSprite;
    public Sprite tileDesignerSprite;
    public bool isPassable;
    public string tileName;
}
