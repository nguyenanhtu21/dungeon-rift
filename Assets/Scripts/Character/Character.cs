using UnityEngine;

public class Character : MonoBehaviour
{
    public CharacterData data;

    public Transform attackPoint;   
    public float attackRange = 0.5f; 
    public LayerMask enemyLayers;   

    public float attackCooldown = 0.5f; 
    public float skillCooldown = 2f; 

    private float currentHP;
    private float currentMana;
    private Animator animator;

    private float nextAttackTime = 0f;
    private float nextSkillTime = 0f;

    void Start()
    {
        currentHP = data.maxHP;
        currentMana = data.maxMana;
        animator = GetComponent<Animator>();
    }

    public void Attack()
    {
        if (Time.time >= nextAttackTime)
        {
            animator.SetTrigger("Attack");
            nextAttackTime = Time.time + attackCooldown;
        }
    }

    public void SkillAttack()
    {
        if (Time.time >= nextSkillTime && CanUseSkill())
        {
            animator.SetTrigger("Skill Attack"); 
            currentMana -= data.skillManaCost; 
            nextSkillTime = Time.time + skillCooldown;
        }
        else if (!CanUseSkill())
        {
            Debug.Log(data.characterName + " không đủ mana!");
        }
    }

    public void DealDamage()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            Enemy enemyChar = enemy.GetComponent<Enemy>();
            if (enemyChar != null)
            {
                int damage = data.normalAttackDamage;
                enemyChar.TakeDamage(damage, transform);
            }
        }
    }

    public void DealSkillDamage()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            Enemy enemyChar = enemy.GetComponent<Enemy>();
            if (enemyChar != null)
            {
                int damage = data.skillDamage;
                enemyChar.TakeDamage(damage, transform);
            }
        }
    }

    private bool CanUseSkill()
    {
        return currentMana >= data.skillManaCost;
    }

    public void TakeDamage(float damage)
    {
        currentHP -= damage;
        if (currentHP <= 0) Die();
    }

    private void Die()
    {
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
