using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SaveData", menuName = "Data/SaveData", order = 0)]
public class SaveData : ScriptableObject
{
    public int sceneIndex;
    public int deathCount;
    public Vector2 respawnPoint;
    public Coloring jellyColoring;
    public Coloring mainColoring;
    public List<bool> isEyeBallList = new List<bool>();
}
