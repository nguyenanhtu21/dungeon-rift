using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemyData", menuName = "Game/Enemy Data")]
public class EnemyData : ScriptableObject
{
    public string enemyName;
    public Sprite portrait;
    public GameObject prefab;

    public float maxHP = 100f;
    public float attackDamage = 10f;

    public float detectRange = 5f;
    public float attackRange = 1.2f;
    public float moveSpeed = 2f;
    public float attackCooldown = 1f;
}
