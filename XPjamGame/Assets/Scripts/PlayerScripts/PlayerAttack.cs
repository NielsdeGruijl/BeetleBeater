using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private GameObject SwordAnchor;

    [SerializeField] private float slashDuration;
    [SerializeField] private float slashSpeed;
    [SerializeField] private float slashCooldown;

    public int damage;

    private PlayerMovement pm;

    private bool slashing = false;
    private bool canSlash = true;

    private Vector3 startRotation;
    private Vector2 attackDir;

    private void Start()
    {
        SwordAnchor.SetActive(false);
        pm = GetComponent<PlayerMovement>();
        startRotation = SwordAnchor.transform.localEulerAngles;
    }

    private void Update()
    {
        if (pm.isDashing) return;

        if (Input.GetMouseButtonDown(0) && canSlash)
        {
            //slashing = true;
            StartCoroutine(SwingSwordCo());

        }
    }

    private IEnumerator SwingSwordCo()
    {
        canSlash = false;
        pm.SetAttacking(true);
        SwordAnchor.SetActive(true);

        attackDir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;

        attackDir = attackDir.normalized;

        pm.RotatePlayer(attackDir);

        float swung = 0f;
        while (swung < 80f)
        {
            SwordAnchor.transform.Rotate(Vector3.forward * (slashSpeed * Time.deltaTime) * -1);
            swung += slashSpeed * Time.deltaTime;
            yield return null;
        }

        SwordAnchor.transform.localRotation = Quaternion.Euler(startRotation);
        SwordAnchor.SetActive(false);
        yield return new WaitForSeconds(slashDuration);
        pm.SetAttacking(false);
        yield return new WaitForSeconds(slashCooldown);
        canSlash = true;
    }
}
