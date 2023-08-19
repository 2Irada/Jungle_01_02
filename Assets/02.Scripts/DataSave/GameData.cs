using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class GameData
{
    public int sceneIndex;
    public int deathCount;
    public Vector2 respawnPoint;
    public Coloring jellyColoring;
    public Coloring mainColoring;
    public List<bool> isEyeBallList = new List<bool>();
}
