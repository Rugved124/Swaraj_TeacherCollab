using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newLookingStateData", menuName = "Data/State Data/Looking State Data")]
public class EnemyLookingStateData : ScriptableObject
{
    public float lookingAngle = 25f;
    public float lookingTime = 1f;
}
