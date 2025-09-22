using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
public class Enemy : MonoBehaviour
{
    public EnemyData data;

    [Header("Combat")]
    public Transform attackPoint;
    public float attackRadius = 0.5f;
    public LayerMask playerLayers;

    [Header("Patrol Settings")]
    public Transform pointA;
    public Transform pointB;
    private Transform patrolTarget;

    [Header("UI")]
    public EnemyHealthBar healthBar;

    [Header("Knockback")]
    public float knockbackForce = 5f;
    public float hurtDuration = 0.2f;

    private float currentHP;
    private Animator animator;
    private Rigidbody2D rb;
    private Transform target;

    private float nextAttackTime = 0f;
    private bool isHurt = false;
    private bool isDead = false;

    private enum EnemyState { Patrol, Chase, Attack, Hurt, Die }
    private EnemyState state;

    void Start()
    {
        currentHP = data.maxHP;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        if (healthBar != null)
            healthBar.SetHealth(currentHP, data.maxHP);

        // bắt đầu ở Patrol
        state = EnemyState.Patrol;
        patrolTarget = pointA;
    }

    void Update()
    {
        if (isDead) return;

        switch (state)
        {
            case EnemyState.Patrol: Patrol(); break;
            case EnemyState.Chase: Chase(); break;
            case EnemyState.Attack: AttackState(); break;
            case EnemyState.Hurt: break;
            case EnemyState.Die: break;
        }
    }

    private void Patrol()
    {
        // tìm player
        FindPlayer();
        if (target != null)
        {
            state = EnemyState.Chase;
            return;
        }

        MoveTo(patrolTarget.position);

        float distance = Vector2.Distance(transform.position, patrolTarget.position);
        if (distance < 0.2f)
        {
            // đổi hướng
            patrolTarget = (patrolTarget == pointA) ? pointB : pointA;
        }
    }

    private void Chase()
    {
        FindPlayer();
        if (target == null)
        {
            // mất player -> quay lại patrol
            state = EnemyState.Patrol;
            animator.SetBool("isMoving", true);
            return;
        }

        float distance = Vector2.Distance(transform.position, target.position);

        if (distance > data.attackRange)
        {
            MoveTo(target.position);
        }
        else
        {
            animator.SetBool("isMoving", false);
            state = EnemyState.Attack;
        }
    }

    private void AttackState()
    {
        if (Time.time >= nextAttackTime)
        {
            int attackIndex = Random.Range(0, 2);

            if (attackIndex == 0)
                animator.SetTrigger("Attack1");
            else
                animator.SetTrigger("Attack2");

            nextAttackTime = Time.time + data.attackCooldown;
        }

        // kiểm tra nếu player ra khỏi tầm
        if (target == null || Vector2.Distance(transform.position, target.position) > data.attackRange)
        {
            state = EnemyState.Chase;
        }
    }


    private void FindPlayer()
    {
        Collider2D playerCollider = Physics2D.OverlapCircle(transform.position, data.detectRange, playerLayers);
        target = playerCollider ? playerCollider.transform : null;
    }

    private void MoveTo(Vector2 targetPos)
    {
        transform.position = Vector2.MoveTowards(
            transform.position,
            targetPos,
            data.moveSpeed * Time.deltaTime
        );

        animator.SetBool("isMoving", true);

        // xoay mặt
        if (targetPos.x > transform.position.x)
            transform.localScale = new Vector3(-1, 1, 1);
        else
            transform.localScale = new Vector3(1, 1, 1);
    }

    public void DealDamage()
    {
        Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(attackPoint.position, attackRadius, playerLayers);

        foreach (Collider2D player in hitPlayers)
        {
            Character character = player.GetComponent<Character>();
            if (character != null)
            {
                character.TakeDamage(data.attackDamage);
                Debug.Log($"{data.enemyName} đánh {character.data.characterName} gây {data.attackDamage} sát thương!");
            }
        }
    }

    public void TakeDamage(float damage, Transform attacker = null)
    {
        if (isDead) return;

        currentHP -= damage;
        if (healthBar != null)
            healthBar.SetHealth(currentHP, data.maxHP);

        if (currentHP <= 0)
        {
            Die();
            return;
        }

        animator.SetTrigger("Hurt");
        if (attacker != null)
        {
            Vector2 knockDir = (transform.position - attacker.position).normalized;
            StartCoroutine(ApplyKnockback(knockDir));
        }
    }

    private IEnumerator ApplyKnockback(Vector2 direction)
    {
        state = EnemyState.Hurt;
        isHurt = true;

        rb.AddForce(direction * knockbackForce, ForceMode2D.Impulse);

        yield return new WaitForSeconds(hurtDuration);

        rb.linearVelocity = Vector2.zero;
        isHurt = false;

        // quay lại Chase nếu thấy player, ngược lại về Patrol
        if (target != null)
        {
            state = EnemyState.Chase;
        }
        else
        {
            state = EnemyState.Patrol;
        }
    }

    private void Die()
    {
        isDead = true;
        state = EnemyState.Die;

        if (healthBar != null) healthBar.gameObject.SetActive(false);

        animator.SetTrigger("Die");
        rb.linearVelocity = Vector2.zero;

        Destroy(gameObject, 1.5f); // hoặc gọi qua Animation Event
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, data.detectRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
    }
}
