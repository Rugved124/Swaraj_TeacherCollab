using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newCharacterData", menuName = "Data/Character Data/Base Data")]
public class CharacterData : ScriptableObject
{
    public float wallCheckDistance = 0.2f;
    public float ledgeCheckDistance = 0.4f;

    public LayerMask whatIsGround;

    public float visionAngle = 30f;
    public float visionDistance = 10f;
    public int raycastCount = 10;
} 
