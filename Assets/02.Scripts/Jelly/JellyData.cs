using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "JellyData", menuName = "Data/JellyData", order = 0)]
public class JellyData : ScriptableObject
{
    public float aimRadius;
    public float jellySpreadTime;
    public float jellyBulletSpeed;
}
