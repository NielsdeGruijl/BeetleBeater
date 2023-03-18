using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "Enemy")]
public class Enemy : ScriptableObject
{
    public string enemyName;

    [Header("Movement")]
    public float movementSpeed;
    public float keepDistance;

    [Header("Attack")]
    public int damage;
}
