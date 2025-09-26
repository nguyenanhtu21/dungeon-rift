using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f;
    public int damage = 10;
    public LayerMask enemyLayers;
    public bool isSkill = false; // phân biệt thường / skill
    public float explosionRadius = 0f; // nếu skill thì nổ

    private Vector2 direction;

    public void Initialize(Vector2 dir, int dmg, bool skill = false, float radius = 0f)
    {
        direction = dir.normalized;
        damage = dmg;
        isSkill = skill;
        explosionRadius = radius;
        Destroy(gameObject, 5f);
    }

    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();
        if (enemy != null || isSkill)
        {
            if (isSkill && explosionRadius > 0)
            {
                // Nổ AOE
                Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, explosionRadius, enemyLayers);
                foreach (Collider2D hit in hitEnemies)
                {
                    Enemy e = hit.GetComponent<Enemy>();
                    if (e != null)
                        e.TakeDamage(damage, transform);
                }
            }
            else if (enemy != null)
            {
                // Đạn thường
                enemy.TakeDamage(damage, transform);
            }

            Destroy(gameObject);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (isSkill && explosionRadius > 0)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, explosionRadius);
        }
    }
}
