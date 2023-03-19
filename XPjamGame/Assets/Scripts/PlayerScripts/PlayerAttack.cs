using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private GameObject swordParent;
    [SerializeField] private GameObject SwordAnchor;

    [SerializeField] private int slashStaminaCost;
    [SerializeField] private float slashDuration;
    [SerializeField] private float slashSpeed;
    [SerializeField] private float slashCooldown;

    public int damage;

    private PlayerMovement pm;
    private PlayerStaminaManager staminaManager;

    private bool slashing = false;
    private bool canSlash = true;

    private Vector3 startRotation;
    private Vector2 attackDir;

    private void Start()
    {
        SwordAnchor.SetActive(false);
        pm = GetComponent<PlayerMovement>();
        staminaManager = GetComponent<PlayerStaminaManager>();

        startRotation = SwordAnchor.transform.localEulerAngles;
    }

    private void Update()
    {
        if (pm.isDashing) return;

        if (Input.GetMouseButtonDown(0) && canSlash && staminaManager.stamina >= slashStaminaCost)
        {
            //slashing = true;
            StartCoroutine(SwingSwordCo());
            
        }
    }

    public void RotateSword(Transform target, Vector2 dir)
    {
        float rotation = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        target.Rotate(0, 0, rotation);
    }


    private IEnumerator SwingSwordCo()
    {
        canSlash = false;
        pm.SetAttacking(true);
        SwordAnchor.SetActive(true);

        attackDir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;

        attackDir = attackDir.normalized;

        RotateSword(swordParent.transform, attackDir);
        
        if(attackDir.x > 0)
        {
            pm.FlipPlayer(false);
        }
        if(attackDir.x < 0)
        {
            pm.FlipPlayer(true);
        }

        float swung = 0f;
        while (swung < 80f)
        {
            SwordAnchor.transform.Rotate(Vector3.forward * (slashSpeed * Time.deltaTime) * -1);
            swung += slashSpeed * Time.deltaTime;
            yield return null;
        }

        SwordAnchor.transform.localRotation = Quaternion.Euler(startRotation);
        swordParent.transform.rotation = Quaternion.identity;
        SwordAnchor.SetActive(false);
        staminaManager.UseStamina(slashStaminaCost);
        yield return new WaitForSeconds(slashDuration);
        pm.SetAttacking(false);
        yield return new WaitForSeconds(slashCooldown);
        canSlash = true;
    }
}
