using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [SerializeField] private RectTransform healthBar;
    [SerializeField] private RectTransform bossHealthBar;

    float healthWidth;
    float bossHealthWidth;

    private void Awake()
    {
        if (instance != null && instance != this)
            Destroy(this);
        if (instance == null)
            instance = this;

        healthWidth = healthBar.transform.localScale.x;
        bossHealthWidth = bossHealthBar.transform.localScale.x;
    }

    public void AdjustHealthBar(int health, int maxHealth)
    {
        float healthBarChunk = healthWidth / maxHealth;

        healthBar.transform.localScale = new Vector3(healthBarChunk * health, healthBar.transform.localScale.y, healthBar.transform.localScale.z);
    }

    public void AdjustBossHealthBar(int health, int maxHealth)
    {
        float healthBarChunk = bossHealthWidth / maxHealth;

        bossHealthBar.transform.localScale = new Vector3(healthBarChunk * health, bossHealthBar.transform.localScale.y, bossHealthBar.transform.localScale.z);
    }
}
