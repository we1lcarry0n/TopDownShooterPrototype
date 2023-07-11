using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private enum EnemyType
    {
        melee,
        ranged
    }

    private Transform player;
    private Health playerHealth;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Shooter shooter;
    private Health health;
    private CapsuleCollider2D colliderEnemy;

    [SerializeField] private float speedRanged;
    [SerializeField] private float speedMelee;
    [SerializeField] private float attackCooldown;
    [SerializeField] private float decayTime;
    [SerializeField] private float distanceToAttackMelee;
    [SerializeField] private float distanceToAttackRanged;
    [SerializeField] private int meleeDamage;

    private bool canAttack;
    private float distanceToAttack;
    private float speed;
    private bool isDead;

    [SerializeField] private EnemyType enemyType;
    private TMP_Text killcountText;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerHealth = player.GetComponent<Health>();
        killcountText = GameObject.FindGameObjectWithTag("Killcounter").GetComponent<TMP_Text>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        shooter = GetComponent<Shooter>();
        health = GetComponent<Health>();
        animator = GetComponent<Animator>();
        colliderEnemy = GetComponent<CapsuleCollider2D>();
        health.Death += Health_Death;
        canAttack = true;
    }

    private void Health_Death(object sender, System.EventArgs e)
    {
        animator.SetBool("isDead", true);
        isDead = true;
        colliderEnemy.enabled = false;
        killcountText.text = $"{Int32.Parse(killcountText.text)+1}";
        StartCoroutine(DeathDissapearCoroutine(decayTime));
    }

    private void OnEnable()
    {
        isDead = false;
        animator.SetBool("isDead", false);
        colliderEnemy.enabled = true;
        health.ResetHealth();
        DetermineEnemyType();
    }

    private void Update()
    {
        if (isDead)
        {
            return;
        }
        MoveToPlayer();
        if (IsInAttackRange())
        {
            TryAttack();
        }
        AdjustFlipping();
    }

    private void MoveToPlayer()
    {
        transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
    }

    private void AdjustFlipping()
    {
        if (player.position.x < transform.position.x)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
        }
    }

    private IEnumerator AttackCooldownCoroutine()
    {
        canAttack = false;
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    private IEnumerator DeathDissapearCoroutine(float decayTime)
    {
        yield return new WaitForSeconds(decayTime);
        gameObject.SetActive(false);
    }

    private bool IsInAttackRange()
    {
        return Vector3.Distance(transform.position, player.position) <= distanceToAttack;
    }

    private void TryAttack()
    {
        if (canAttack)
        {
            if (enemyType == EnemyType.ranged)
            {
                shooter.Shoot((player.position - transform.position).normalized);
            }
            else
            {
                animator.SetTrigger("attack");
                playerHealth.TakeDamage(meleeDamage);
            }
            StartCoroutine(AttackCooldownCoroutine());
        }
    }

    private void DetermineEnemyType()
    {
        if (UnityEngine.Random.Range(0, 2) == 0)
        {
            enemyType = EnemyType.melee;
            distanceToAttack = distanceToAttackMelee;
            speed = speedMelee;
        }
        else
        {
            enemyType = EnemyType.ranged;
            distanceToAttack = distanceToAttackRanged;
            speed = speedRanged;
        }
    }
}
