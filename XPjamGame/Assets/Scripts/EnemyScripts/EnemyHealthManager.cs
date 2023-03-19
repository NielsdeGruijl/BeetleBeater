using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyHealthManager : MonoBehaviour
{
    [SerializeField] private int health;

    private int maxHP;

    private void Start()
    {
        maxHP = health;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        AudioManager.manager.PlayAudio("EnemyHit");

        if (health <= 0)
        {
            Die();
        }

        UIManager.instance.AdjustBossHealthBar(health, maxHP);
    }

    private void Die()
    {
        Destroy(gameObject);
        SceneManager.LoadScene(3);
    }
}
