using UnityEngine;

[CreateAssetMenu(fileName = "NewCharacterData", menuName = "Game/Character Data")]
public class CharacterData : ScriptableObject
{
    [Header("Thông tin cơ bản")]
    public string characterName;
    public int maxHP;
    public int maxMana;

    [Header("Tấn công thường")]
    public int normalAttackDamage;

    [Header("Skill")]
    public Sprite skillIcon;
    public int skillDamage;
    public int skillManaCost;
    public float skillCooldown;
    public string skillAnimationTrigger;
    public GameObject skillProjectilePrefab;
    public bool skillIsAOE;
    public float skillExplosionRadius;

    [Header("Kiểu tấn công")]
    public AttackType attackType;   
    public GameObject projectilePrefab; 
}

public enum AttackType { Melee, Ranged }
