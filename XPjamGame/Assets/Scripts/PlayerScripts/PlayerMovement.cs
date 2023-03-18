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
    private Vector2 attackDir;

    public bool isAttacking = false;
    public bool isDashing = false;
    private bool canDash = true;


    private float xInput;
    private float yInput;

    private float elapsedTime;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
    }

    private void Update()
    {
        if (isDashing || isAttacking) return;
        
        xInput = Input.GetAxis("Horizontal");
        yInput = Input.GetAxis("Vertical");

        moveDir = new Vector2(xInput, yInput);
        moveDir = moveDir.normalized;

        if(rb.velocity.magnitude >= 0.1f)
        {
            RotatePlayer(moveDir);
        }
        
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dash());
        }
    }

    public void RotatePlayer(Vector2 dir)
    {
        float zRotation = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, zRotation);
    }

    public void SetAttacking(bool attacking)
    {
        isAttacking = attacking;
    }

    private void FixedUpdate()
    {
        if (isDashing) return;

        if (isAttacking)
        {
            rb.velocity = moveDir * speed * 0.5f;
        }
        else
        {
            rb.velocity = moveDir * speed;
        }
        
    }

    IEnumerator Dash()
    {
        isDashing = true;
        canDash = false;
        elapsedTime += Time.deltaTime;
        rb.velocity = moveDir * dashForce;
        yield return new WaitForSeconds(dashDuration);
        isDashing = false;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
        elapsedTime = 0;
    }
}
