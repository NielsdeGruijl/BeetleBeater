using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthManager : MonoBehaviour
{
    [SerializeField] private int health;

    private PlayerMovement pm;

    private int maxHP;

    private void Start()
    {
        maxHP = health;
        pm = GetComponent<PlayerMovement>();
    }

    public void TakeDamage(int damage)
    {
        if (pm.isDashing) return;

        health -= damage;
        Debug.Log($"PlayerHP: {health} / {maxHP}");

        if(health <= 0)
        {
            Die();
        }

        UIManager.instance.AdjustHealthBar(health, maxHP);
    }

    public void Die()
    {

    }
}
