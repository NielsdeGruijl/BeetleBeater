using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyScript : MonoBehaviour
{
    [SerializeField] private Enemy enemyType;

    [SerializeField] private EnemyHealthManager healthManager;

    [SerializeField] private Collider2D col;

    [Header("Ground slam")]
    [SerializeField] private AnimationCurve JumpCurve;
    [SerializeField] private Collider2D shockWave;

    [Header("Ground slam variables")]
    [SerializeField] private float shockWaveDuration;
    [SerializeField] private float slamCooldown;
    [SerializeField] private float jumpDuration;
    [SerializeField] private float jumpHeight;

    private Rigidbody2D rb;
    private GameObject player;

    private bool playerInRange = false;

    private bool canSlam = true;

    private float moveSpeed;
    private float keepDistance;
    public int damage;

    private Vector2 moveDir;
    private float playerDist;

    private Vector2 startPos;
    private Vector2 destination;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        player = GameObject.FindGameObjectWithTag("Player");

        SetValues();
    }

    private void Update()
    {
        MoveTowardsPlayer();

        if (playerDist > keepDistance)
        {
            playerInRange = false;
        }
        else
        {
            playerInRange = true;
            if (canSlam)
                StartCoroutine(GroundSlam());
        }
    }

    private void SetValues()
    {
        moveSpeed = enemyType.movementSpeed;
        keepDistance = enemyType.keepDistance;

        damage = enemyType.damage;
    }

    private void MoveTowardsPlayer()
    {
        moveDir = player.transform.position - transform.position;

        playerDist = moveDir.magnitude;

        moveDir.Normalize();
    }
    
    private IEnumerator GroundSlam()
    {
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
        yield return new WaitForSeconds(shockWaveDuration);
        shockWave.enabled = false;

        yield return new WaitForSeconds(slamCooldown);
        canSlam = true;
    }

    private void FixedUpdate()
    {
        if(!playerInRange)
        {
            rb.velocity = moveDir * moveSpeed;
        }
        else
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
            collision.GetComponent<PlayerHealthManager>().TakeDamage(damage);
        }
    }
}
