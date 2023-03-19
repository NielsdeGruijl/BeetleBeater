using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyScript : MonoBehaviour
{
    [SerializeField] private EnemyHealthManager healthManager;

    [SerializeField] private Collider2D col;
    
    [Header("Base stats")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float keepDistance;
    public int slamDamage;
    public int chargeDamage;

    [Header("Ground slam")]
    [SerializeField] private AnimationCurve JumpCurve;
    [SerializeField] private Collider2D shockWave;

    [Header("Ground slam variables")]
    [SerializeField] private float shockWaveDuration;
    [SerializeField] private float slamCooldown;
    [SerializeField] private float slamRange;
    [SerializeField] private float jumpDuration;
    [SerializeField] private float jumpHeight;

    [Header("Charge attack variables")]
    [SerializeField] private float windUpDuration;
    [SerializeField] private float chargeCooldown;
    [SerializeField] private float chargeSpeed;

    private SpriteRenderer sr;

    private Rigidbody2D rb;
    private GameObject player;

    private bool playerInRange = false;

    private bool canSlam = true;
    private bool isCharging = false;
    private bool chargeDirSet = false;

    private bool canAttack = false;

    private int totalCharges;

    private Vector2 moveDir;
    private float playerDist;

    private Vector2 startPos;
    private Vector2 chargeDir;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        sr = GetComponentInChildren<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player");

        StartCoroutine(attackCooldown());
    }

    private void Update()
    {
        MoveTowardsPlayer();

        if (playerDist > keepDistance)
            playerInRange = false;
        else
            playerInRange = true;

        if (canAttack)
            SelectAttack();
    }

    private void MoveTowardsPlayer()
    {
        moveDir = player.transform.position - transform.position;

        playerDist = moveDir.magnitude;

        moveDir.Normalize();
    }
    
    private void SelectAttack()
    {
        if (canAttack)
        {
            Debug.Log(playerDist);
            if (playerDist <= slamRange)
            {
                StartCoroutine(GroundSlam());
            }
            else
            {
                totalCharges = 2;
                StartCoroutine(Charge(windUpDuration));
            }
        }
    }

    IEnumerator attackCooldown()
    {
        float randCooldown = Random.Range(3.0f, 5.0f);
        yield return new WaitForSeconds(randCooldown);
        canAttack = true;
    }

    private IEnumerator GroundSlam()
    {
        sr.color = Color.blue;
        canAttack = false;
        canSlam = false;
        col.enabled = false;
        float elapsedTime = 0;
        startPos = transform.position;
        rb.velocity = Vector2.zero;

        while(elapsedTime < jumpDuration)
        {
            elapsedTime += Time.deltaTime;
            float strength = JumpCurve.Evaluate(elapsedTime / jumpDuration);
            transform.position = new Vector2(startPos.x, startPos.y + strength * jumpHeight);

            yield return null;
        }
        StartCoroutine(ScreenShake.instance.Shake(1, 1));
        col.enabled = true;
        shockWave.enabled = true;
        sr.color = Color.red;
        yield return new WaitForSeconds(shockWaveDuration);
        shockWave.enabled = false;

        yield return new WaitForSeconds(slamCooldown);
        canSlam = true;
        StartCoroutine(attackCooldown());
    }

    private IEnumerator Charge(float windup)
    {
        sr.color = Color.green;
        canAttack = false;
        isCharging = true;
        rb.velocity = Vector2.zero;
        yield return new WaitForSeconds(windup);

        InitiateCharge(true);
    }

    private void InitiateCharge(bool leading)
    {
        if (!chargeDirSet)
        {
            chargeDir = player.transform.position - transform.position;

            if (leading)
                chargeDir += player.GetComponent<Rigidbody2D>().velocity;

            chargeDir.Normalize();
            chargeDirSet = true;
        }
        rb.velocity = chargeDir * chargeSpeed;
        totalCharges--;
    }

    IEnumerator ChargeCooldown()
    {
        rb.velocity = Vector2.zero;
        chargeDirSet = false;
        sr.color = Color.red;
        yield return new WaitForSeconds(chargeCooldown);
        isCharging = false;
        StartCoroutine(attackCooldown());
    }

    private void FixedUpdate()
    {
        if (isCharging) return;

        if(!playerInRange)
        {
            rb.velocity = moveDir * moveSpeed;
        }

        if(playerInRange)
        {
            rb.velocity = Vector2.zero;
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Sword"))
        {
            Debug.Log("Enemy Hit!");
            healthManager.TakeDamage(player.GetComponent<PlayerAttack>().damage);

            StartCoroutine(ScreenShake.instance.Shake(0.2f, 0.5f));
        }

        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerHealthManager>().TakeDamage(slamDamage);
        }

        if (collision.CompareTag("Border"))
        { 
            if (isCharging)
            {
                if(totalCharges > 0)
                {
                    chargeDirSet = false;
                    StartCoroutine(Charge(0.1f));
                }
                else
                {
                    StartCoroutine(ChargeCooldown());
                }
                
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            collision.collider.GetComponent<PlayerHealthManager>().TakeDamage(chargeDamage);
            StartCoroutine(ChargeCooldown());
        }
    }
}
