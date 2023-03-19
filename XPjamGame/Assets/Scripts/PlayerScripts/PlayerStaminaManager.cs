using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStaminaManager : MonoBehaviour
{
    public float stamina;
    [SerializeField] private float staminaRegenCooldown;
    [SerializeField] private float staminaRegenSpeed;

    private float maxStamina;

    private bool regeneratingStamina;

    private void Start()
    {
        maxStamina = stamina;
    }

    public void UseStamina(int staminaUsed)
    {
        if (regeneratingStamina)
            StopAllCoroutines();

        stamina -= staminaUsed;

        Debug.Log($"Stamina: {stamina} / {maxStamina}");

        UIManager.instance.AdjustStamBar(stamina, maxStamina);

        StartCoroutine(RegenerateStamina());
    }

    private IEnumerator RegenerateStamina()
    {
        regeneratingStamina = true;
        yield return new WaitForSeconds(staminaRegenCooldown);

        while(stamina < maxStamina)
        {
            stamina += staminaRegenSpeed * Time.deltaTime;
            UIManager.instance.AdjustStamBar(stamina, maxStamina);

            if (stamina > maxStamina)
            {
                stamina = maxStamina;
            }
            yield return null;
        }

        regeneratingStamina = false;
    }
}
