using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float speed;

    [Header("Dash Variables")]
    [SerializeField] float dashForce;
    [SerializeField] private float dashDuration;
    [SerializeField] private float dashCooldown;

    private Rigidbody2D rb;

    private Vector2 moveDir;
    private Vector2 dashDir;


    private bool isDashing = false;
    private bool canDash = true;


    private float xInput;
    private float yInput;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (isDashing) return;
        RotatePlayer();

        xInput = Input.GetAxis("Horizontal");
        yInput = Input.GetAxis("Vertical");

        moveDir = new Vector2(xInput, yInput);
        moveDir = moveDir.normalized;

        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dash());
        }
    }

    private void RotatePlayer()
    {
        dashDir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;

        dashDir = dashDir.normalized;

        float zRotation = Mathf.Atan2(dashDir.y, dashDir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, zRotation);
    }

    private void FixedUpdate()
    {
        if (isDashing) return;

        rb.velocity = moveDir * speed;
    }

    IEnumerator Dash()
    {
        isDashing = true;
        canDash = false;
        rb.velocity = dashDir * dashForce;
        yield return new WaitForSeconds(dashDuration);
        rb.velocity = Vector2.zero;
        isDashing = false;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
}
