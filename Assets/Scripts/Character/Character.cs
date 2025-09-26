using UnityEngine;

public class Character : MonoBehaviour
{
    [Header("Character Data")]
    public CharacterData data;

    [Header("Attack")]
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;

    [Header("Cooldowns")]
    public float attackCooldown = 0.5f;
    private float nextAttackTime = 0f;
    private float nextSkillTime = 0f;

    // Stats
    private float currentHP;
    private float currentMana;
    public float CurrentMana => currentMana;

    // Components
    private Animator animator;
    private PlayerUI playerUI;
    private SpriteRenderer spriteRenderer;
    private bool isDead = false;

    void Start()
    {
        currentHP = data.maxHP;
        currentMana = data.maxMana;
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        playerUI = FindAnyObjectByType<PlayerUI>();

        if (playerUI != null)
        {
            playerUI.SetHealth(currentHP, data.maxHP);
            playerUI.SetMana(currentMana, data.maxMana);
        }
    }


    public void Attack()
    {
        if (isDead) return;

        if (Time.time >= nextAttackTime)
        {
            animator.SetTrigger("Attack");
            nextAttackTime = Time.time + attackCooldown;
        }
    }

    public void DealDamage()
    {
        if (data.attackType == AttackType.Melee)
        {
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
            foreach (Collider2D enemy in hitEnemies)
            {
                Enemy enemyChar = enemy.GetComponent<Enemy>();
                if (enemyChar != null)
                    enemyChar.TakeDamage(data.normalAttackDamage, transform);
            }
        }
        else if (data.attackType == AttackType.Ranged)
        {
            Vector2 dir = spriteRenderer.flipX ? Vector2.left : Vector2.right;
            GameObject proj = Instantiate(data.projectilePrefab, attackPoint.position, Quaternion.identity);
            proj.GetComponent<Projectile>().Initialize(dir, data.normalAttackDamage);
        }
    }


    public bool UseSkill()
    {
        if (isDead) return false;

        if (Time.time >= nextSkillTime && currentMana >= data.skillManaCost)
        {
            animator.SetTrigger(data.skillAnimationTrigger);
            UseMana(data.skillManaCost);
            nextSkillTime = Time.time + data.skillCooldown;
            return true;
        }
        return false;
    }

    public void DealSkillDamage()
    {
        if (isDead) return;

        if (data.attackType == AttackType.Melee)
        {
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
            foreach (Collider2D enemy in hitEnemies)
            {
                Enemy enemyChar = enemy.GetComponent<Enemy>();
                if (enemyChar != null)
                    enemyChar.TakeDamage(data.skillDamage, transform);
            }
        }
        else if (data.attackType == AttackType.Ranged && data.skillProjectilePrefab != null)
        {
            Vector2 dir = spriteRenderer.flipX ? Vector2.left : Vector2.right;
            GameObject proj = Instantiate(data.skillProjectilePrefab, attackPoint.position, Quaternion.identity);
            proj.GetComponent<Projectile>().Initialize(dir, data.skillDamage, data.skillIsAOE, data.skillExplosionRadius);
        }
    }

    public void UseMana(float amount)
    {
        currentMana -= amount;
        if (currentMana < 0) currentMana = 0;

        if (playerUI != null)
            playerUI.SetMana(currentMana, data.maxMana);
    }

    public void Heal(float amount)
    {
        if (isDead) return;

        currentHP += amount;
        if (currentHP > data.maxHP) currentHP = data.maxHP;

        if (playerUI != null)
            playerUI.SetHealth(currentHP, data.maxHP);
    }

    public void RestoreMana(float amount)
    {
        if (isDead) return;

        currentMana += amount;
        if (currentMana > data.maxMana) currentMana = data.maxMana;

        if (playerUI != null)
            playerUI.SetMana(currentMana, data.maxMana);
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;

        currentHP -= damage;
        animator.SetTrigger("Hurt");

        if (playerUI != null)
            playerUI.SetHealth(currentHP, data.maxHP);

        if (currentHP <= 0) Die();
    }

    private void Die()
    {
        if (isDead) return;
        isDead = true;

        animator.SetTrigger("Die");
        if (playerUI != null)
            playerUI.gameObject.SetActive(false);

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null) rb.linearVelocity = Vector2.zero;

        CapsuleCollider2D col = GetComponent<CapsuleCollider2D>();
        if (col != null) col.enabled = false;

        CharacterMovement move = GetComponent<CharacterMovement>();
        if (move != null) move.enabled = false;
    }

    // =============== DEBUG ===============
    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
